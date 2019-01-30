using Dewey.Dms.FileService.Services;

namespace Dewey.Dms.FileService.Api.Models.View
{
    public class ResultRest<T>
    {
      
        public T Result { get; }
        
        public bool IsError { get; }
        
        public string ErrorMessage { get; }

        public ResultRest(ResultService<T> resultService)
        {
            this.IsError = resultService.IsError;
            this.Result = resultService.Value;
            this.ErrorMessage = resultService.ErrorMessage;
        }
        
    }
}