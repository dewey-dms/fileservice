using System.Collections.Generic;
using System.Threading.Tasks;
using Dewey.Dms.FileService.Hbase.Operations;
using Dewey.Dms.FileService.Hbase.Views;

namespace Dewey.Dms.FileService.Hbase.Service
{
    public interface IDatabaseService
    {
       Task DoUserOperations(UserOperations operations);
       Task<User> GetUser(string key);
       Task<IEnumerable<User>> GetUsers(bool? isDelete = null);
       Task DoFileOperations(FileOperations operations);
       Task<File> GetFile(string key);

       Task<IEnumerable<File>> GetUserFile(string userKey, bool? isDelete = null);

    }
}