using System;
using Dewey.Dms.FileService.Services;

namespace Dewey.Dms.FileService.Api.Models.View
{
    public class ResultRest<T>
    {
      
        public T Result { get; }
        
        public bool IsError { get; }
        
        public string ErrorMessage { get; }

        /*
        private ResultRest(T result)
        {
            this.Result = result;
            this.IsError = false;
            this.ErrorMessage = string.Empty;
        }
        
        private ResultRest(string errorMessage)
        {
            //this.Result = null;
            this.IsError = false;
            this.ErrorMessage = errorMessage;
        }
        
        
        public static ResultRest<T> Ok(T result)
        {
            return new ResultRest<T>(result);
        }
        
        public static ResultRest<T> Error(string errorMsg)
        {
            return new ResultRest<T>(errorMsg);
        }
        */
        public ResultRest(ResultService<T> resultService)
        {
            this.IsError = resultService.IsError;
            this.Result = resultService.Value;
            this.ErrorMessage = resultService.ErrorMessage;
        }
        
        /*
        public ResultRest<U> Map<U> (Func<T,U> action )
        {
            if (!IsError)
                ResultService<U>.Error(ErrorMessage);
            return ResultRest<U>.Ok(action(Result));
        }*/
        
    }
}