using System.Collections.Generic;
using Dewey.Dms.FileService.Hbase.Operations;
using Dewey.Dms.FileService.Hbase.Views;

namespace Dewey.Dms.FileService.Hbase.Service
{
    public interface IDatabaseService
    {
       bool DoUserOperations(UserOperations operations);
       User GetUser(string key);
       List<User> GetUsers(bool? isDelete = null);
       bool DoFileOperations(FileOperations operations);
       File GetFile(string key);
    }
}