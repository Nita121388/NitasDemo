using System.Reactive.Linq;
using System.Reactive.Subjects;

// 创建可观察序列
var subject = new Subject<string>();
var observable = subject.AsObservable();

// 订阅观察者
var subscription = observable
    .Where(msg => msg.StartsWith("IMPORTANT"))
    .Subscribe(msg => Console.WriteLine($"Rx received: {msg}"));

// 推送消息
subject.OnNext("IMPORTANT: System update");
subject.OnNext("Normal message");  // 被过滤

// 取消订阅
subscription.Dispose();