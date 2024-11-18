using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFArchitectureDemo.Service
{
    public class Result<T>
    {
        public State State { get; private set; }
        public T Data { get; private set; }
        public string Message { get; private set; }

        // 私有构造函数，防止直接实例化
        private Result(State state, T data = default, string message = null)
        {
            State = state;
            Data = data;
            Message = message;
        }

        // 静态工厂方法
        public static Result<T> Success(T data)
        {
            return new Result<T>(State.Success, data);
        }

        public static Result<T> Error(string message)
        {
            return new Result<T>(State.Error, message: message);
        }

        public static Result<T> Warning(string message)
        {
            return new Result<T>(State.Warning, message: message);
        }

        public static Result<T> Info(string message)
        {
            return new Result<T>(State.Info, message: message);
        }
    }
    public enum State
    {
        Success,
        Error,
        Warning,
        Info
    }
}
