using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using File = Dewey.Dms.FileService.Hbase.Views.File;
using Dewey.Dms.Core;

namespace Dewey.Dms.FileService.Services
{
    public interface IDmsFileOperations
    {
         Task<ResultService<IEnumerable<File>>>GetInfoFilesToUser(IFileServiceLogger fileServicelogger , string userKey, bool? isDelete = null);
         Task<ResultService<File>> GetInfoFile(IFileServiceLogger fileServicelogger , string userKey , string userFileKey);

         Task<ResultService<File>> AddFileToUser(IFileServiceLogger fileServiceLogger, string userKey, Stream stream,
             string fileName, string extension);

         Task<ResultService<File>> DeleteFileToUser(IFileServiceLogger fileServiceLogger, string userKey,
             string userFileKey);
         
        Task<ResultService<(File File, Stream Stream)>> GetFileToUser(IFileServiceLogger fileServiceLogger, string userKey , string userFilekey);
        
    }
}