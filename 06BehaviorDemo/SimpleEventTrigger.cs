using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace BehaviorDemo
{
    public static class EventTriggerBehavior
    {
        // 注册附加属性 EventName，用于存储事件名称
        public static readonly DependencyProperty EventNameProperty =
            DependencyProperty.RegisterAttached("EventName", typeof(string), typeof(EventTriggerBehavior), new PropertyMetadata(null, OnEventNameChanged));

        // 注册附加属性 Command，用于存储命令
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(EventTriggerBehavior), new PropertyMetadata(null));

        // 获取 EventName 附加属性的值
        public static string GetEventName(DependencyObject obj)
        {
            return (string)obj.GetValue(EventNameProperty);
        }

        // 设置 EventName 附加属性的值
        public static void SetEventName(DependencyObject obj, string value)
        {
            obj.SetValue(EventNameProperty, value);
        }

        // 获取 Command 附加属性的值
        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        // 设置 Command 附加属性的值
        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        // 当 EventName 属性改变时的回调方法
        private static void OnEventNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                string eventName = e.NewValue as string;
                if (!string.IsNullOrEmpty(eventName))
                {
                    // 获取事件信息
                    EventInfo eventInfo = element.GetType().GetEvent(eventName);
                    if (eventInfo != null)
                    {
                        // 验证事件是否有效
                        if (IsValidEvent(eventInfo))
                        {
                            // 获取 OnEventTriggered 方法的信息
                            MethodInfo methodInfo = typeof(EventTriggerBehavior).GetMethod("OnEventTriggered", BindingFlags.NonPublic | BindingFlags.Static);

                            // 创建事件处理程序委托
                            Delegate eventHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType, methodInfo);

                            // 将事件处理程序添加到事件中
                            eventInfo.AddEventHandler(element, eventHandler);
                        }
                    }
                }
            }
        }

        // 验证事件是否有效
        private static bool IsValidEvent(EventInfo eventInfo)
        {
            Type eventHandlerType = eventInfo.EventHandlerType;
            if (typeof(Delegate).IsAssignableFrom(eventInfo.EventHandlerType))
            {
                MethodInfo invokeMethod = eventHandlerType.GetMethod("Invoke");
                ParameterInfo[] parameters = invokeMethod.GetParameters();
                // 验证事件处理程序的参数是否符合要求
                return parameters.Length == 2 && typeof(object).IsAssignableFrom(parameters[0].ParameterType) && typeof(EventArgs).IsAssignableFrom(parameters[1].ParameterType);
            }
            return false;
        }

        // 事件触发时的回调方法
        private static void OnEventTriggered(object sender, EventArgs e)
        {
            if (sender is DependencyObject dependencyObject)
            {
                // 获取并执行命令
                ICommand command = GetCommand(dependencyObject);
                if (command != null && command.CanExecute(null))
                {
                    command.Execute(null);
                }
            }
        }
    }


    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class MainViewModel
    {
        private ICommand _messageCommand;

        public ICommand MessageCommand
        {
            get
            {
                if (_messageCommand == null)
                {
                    _messageCommand = new RelayCommand(param => this.ShowMessage(), param => this.CanShowMessage());
                }
                return _messageCommand;
            }
        }

        private void ShowMessage()
        {
            MessageBox.Show("this is a message!");
        }

        private bool CanShowMessage()
        {
            return true;
        }
    }
}

