using System;
using System.Runtime.InteropServices.ComTypes;

namespace Dewey.Dms.Core
{
    public class ResultService<T>
    {
        public bool IsError { get; private set; }
        public string ErrorMessage { get; private set; }
        public T Value { get; private set; }

        private ResultService(T value)
        {
            this.Value = value;
            this.IsError = false;
            this.ErrorMessage = string.Empty;
        }
        
        private ResultService(string errorMessage)
        {
            //this.Result = null;
            this.IsError = false;
            this.ErrorMessage = errorMessage;
        }


        public static ResultService<T> Ok(T result)
        {
            return new ResultService<T>(result);
        }
        
        public static ResultService<T> Error(string errorMsg)
        {
            return new ResultService<T>(errorMsg);
        }

        public ResultService<T> OnSuccess(Action<T> action )
        {
            if (!IsError)
                action(Value);
            return this;
        }
        
        public ResultService<T> OnError(Action<string> action )
        {
            if (IsError)
                action(ErrorMessage);
            return this;
        }
        
        
        public ResultService<U> Map<U> (Func<T,U> action )
        {
            if (!IsError)
                ResultService<U>.Error(ErrorMessage);
            return ResultService<U>.Ok(action(Value));
        }
        
        public ResultService<T> Ensure(Func<T, bool> predicate, string errorMessage)
        {
            if (IsError)
                return this;

            if (!predicate(Value))
                return new ResultService<T>(errorMessage);

            return this;
        }

        public ResultService<T> NextResult(Func<T, ResultService<T>> function)
        {
            if (IsError)
                return this;
            
            return function(Value);
        }
        
        
        
    }
}