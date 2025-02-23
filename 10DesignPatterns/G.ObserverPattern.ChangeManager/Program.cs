using System;
using System.Collections.Generic;
using System.Linq;

#region Client Code

// 使用简单更改管理器
ChangeManager.Instance = SimpleChangeManager.Instance;
        
var subject = new ConcreteSubject();
var observer = new ConcreteObserver();
        
ChangeManager.Instance.Register(subject, observer);
        
subject.State = 42;
subject.Notify();
ChangeManager.Instance.Commit();
        
// 使用DAG更改管理器
ChangeManager.Instance = DAGChangeManager.Instance;
var subjectA = new ConcreteSubject();
var subjectB = new ConcreteSubject();
var dagObserver = new ConcreteObserver();
        
ChangeManager.Instance.Register(subjectA, dagObserver);
ChangeManager.Instance.Register(subjectB, dagObserver);
((DAGChangeManager)ChangeManager.Instance).AddDependency(subjectA, subjectB);
        
subjectA.State = 10;
subjectB.State = 20;
subjectA.Notify();
subjectB.Notify();
ChangeManager.Instance.Commit();
#endregion

#region IObserver

// 观察者接口
public interface IObserver
{
    void Update(ISubject subject);
}


#endregion

#region IObservable
// 目标对象接口
public interface ISubject
{
    void Notify();
}
#endregion

#region ChangeManager

// 更改管理器抽象类
public abstract class ChangeManager
{
    public static ChangeManager Instance { get;  set; }

    public abstract void Register(ISubject subject, IObserver observer);
    public abstract void Unregister(ISubject subject, IObserver observer);
    public abstract void Notify(ISubject subject);
    public abstract void Commit();
}

#endregion

#region SimpleChangeManager

// 简单更改管理器实现
public sealed class SimpleChangeManager : ChangeManager
{
    private readonly Dictionary<ISubject, HashSet<IObserver>> _observers = new();
    private readonly HashSet<ISubject> _dirtySubjects = new();

    static SimpleChangeManager()
    {
    }
    private static SimpleChangeManager _instance;
    public  static SimpleChangeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SimpleChangeManager();
            }
            return _instance;
        }
    }

    public override void Register(ISubject subject, IObserver observer)
    {
        if (!_observers.ContainsKey(subject))
        {
            _observers[subject] = new HashSet<IObserver>();
        }
        _observers[subject].Add(observer);
    }

    public override void Unregister(ISubject subject, IObserver observer)
    {
        if (_observers.TryGetValue(subject, out var observers))
        {
            observers.Remove(observer);
        }
    }

    public override void Notify(ISubject subject)
    {
        lock (_dirtySubjects)
        {
            _dirtySubjects.Add(subject);
        }
    }

    public override void Commit()
    {
        HashSet<IObserver> notified = new();
        List<ISubject> toProcess;

        lock (_dirtySubjects)
        {
            toProcess = _dirtySubjects.ToList();
            _dirtySubjects.Clear();
        }

        foreach (var subject in toProcess)
        {
            if (_observers.TryGetValue(subject, out var observers))
            {
                foreach (var observer in observers.Where(observer => notified.Add(observer)))
                {
                    observer.Update(subject);
                }
            }
        }
    }
}

#endregion

#region DAGChangeManager

// 基于DAG的复杂更改管理器
public sealed class DAGChangeManager : ChangeManager
{
    private readonly Dictionary<ISubject, HashSet<IObserver>> _observers = new();
    private readonly Dictionary<ISubject, HashSet<ISubject>> _dependencies = new();
    private readonly HashSet<ISubject> _dirtySubjects = new();

    static DAGChangeManager()
    {
       
    }
    private static DAGChangeManager _instance;
    public  static DAGChangeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DAGChangeManager();
            }
            return _instance;
        }
    }

    // 添加依赖关系（dependent 依赖于 dependency）
    public void AddDependency(ISubject dependent, ISubject dependency)
    {
        if (!_dependencies.ContainsKey(dependent))
        {
            _dependencies[dependent] = new HashSet<ISubject>();
        }
        _dependencies[dependent].Add(dependency);
    }

    public override void Register(ISubject subject, IObserver observer)
    {
        if (!_observers.ContainsKey(subject))
        {
            _observers[subject] = new HashSet<IObserver>();
        }
        _observers[subject].Add(observer);
    }

    public override void Unregister(ISubject subject, IObserver observer) 
    {
        if (_observers.TryGetValue(subject, out var observers))
        {
            observers.Remove(observer);
        }
    }

    /// <summary>
    /// 处理所有“脏”主题，并按照拓扑排序的顺序通知观察者
    /// </summary>
    /// <param name="subject"></param>
    public override void Notify(ISubject subject)
    {
        lock (_dirtySubjects)
        {
            _dirtySubjects.Add(subject);
        }
    }

    public override void Commit()
    {
        List<ISubject> processingOrder;
        lock (_dirtySubjects)
        {
            processingOrder = TopologicalSort(_dirtySubjects);
            _dirtySubjects.Clear();
        }

        HashSet<IObserver> notified = new();
        foreach (var subject in processingOrder)
        {
            if (_observers.TryGetValue(subject, out var observers))
            {
                foreach (var observer in observers.Where(observer => notified.Add(observer)))
                {
                    observer.Update(subject);
                }
            }
        }
    }

    /// <summary>
    /// TopologicalSort 方法用于对主题进行拓扑排序，确保依赖关系的顺序正确：
    /// </summary>
    /// <param name="subjects"></param>
    /// <returns></returns>
    private List<ISubject> TopologicalSort(HashSet<ISubject> subjects)
    {
        var sorted = new List<ISubject>();
        var visited = new HashSet<ISubject>();

        foreach (var subject in subjects.OrderBy(s => s.GetHashCode()))
        {
            Visit(subject, visited, sorted);
        }

        return sorted;
    }

    private void Visit(ISubject subject, HashSet<ISubject> visited, List<ISubject> sorted)
    {
        if (!visited.Add(subject)) return;

        if (_dependencies.TryGetValue(subject, out var dependencies))
        {
            foreach (var dependency in dependencies)
            {
                Visit(dependency, visited, sorted);
            }
        }

        sorted.Add(subject);
    }
}



#endregion

#region ConcreteSubject

// 示例使用
public class ConcreteSubject : ISubject
{
    public int State { get; set; }

    public void Notify()
    {
        ChangeManager.Instance.Notify(this);
    }
}


#endregion

#region ConcreteObserver
public class ConcreteObserver : IObserver
{
    public void Update(ISubject subject)
    {
        if (subject is ConcreteSubject concreteSubject)
        {
            Console.WriteLine($"Received update: {concreteSubject.State}");
        }
    }
}
#endregion

