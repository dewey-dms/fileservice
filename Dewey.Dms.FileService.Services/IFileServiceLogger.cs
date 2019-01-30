using System;

namespace Dewey.Dms.FileService.Services
{
    public interface IFileServiceLogger
    {
        void LogInfo(string info);
        void LogError(string error);
        void LogError(string error , Exception ex);
      
        void LogDebug(string debug);
        void LogWarning(string warning);
    }
}