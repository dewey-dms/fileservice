using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using File = Dewey.Dms.FileService.Hbase.Views.File;

namespace Dewey.Dms.FileService.Services
{
    public interface IDmsFileOperations
    {
         Task<ResultService<IEnumerable<File>>>GetUserFile(IFileServiceLogger fileServicelogger , string userKey, bool? isDelete = null);
         Task<ResultService<File>> GetFile(IFileServiceLogger fileServicelogger , string userKey , string userFileKey);

         Task<ResultService<File>> AddFile(IFileServiceLogger fileServiceLogger, string userKey, Stream stream,
             string fileName, string extension);


    }
}