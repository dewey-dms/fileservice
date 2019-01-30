using System;
using Dewey.Dms.FileService.Services;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Dewey.Dms.FileService.Api
{
    public class ApiFileServiceLogger:IFileServiceLogger
    {
        private ILogger logger;
        public ApiFileServiceLogger(ILogger logger)
        {
            this.logger = logger;
        }
        
        public void LogInfo(string info)
        {
           logger.Log(LogLevel.Information,info);
        }

        public void LogError(string error)
        {
            logger.Log(LogLevel.Error,error);
        }

      
        public void LogError(string error, Exception ex)
        {
            logger.Log(LogLevel.Error,error,ex);
        }

        public void LogDebug(string debug)
        {
            logger.Log(LogLevel.Debug, debug);
        }

        public void LogWarning(string warning)
        {
            logger.Log(LogLevel.Warning, warning);
        }
    }
}