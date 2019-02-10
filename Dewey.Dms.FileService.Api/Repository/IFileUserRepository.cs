using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Dewey.Dms.FileService.Api.Models.View;
using File = Dewey.Dms.FileService.Api.Models.View.FileRest;

namespace Dewey.Dms.FileService.Api.Repository
{
    public interface IFileUserRepository
    {
        Task<ResultRest<List<FileRest>>> GetInfoFilesToUser(string userKey,bool? isDelete =null);

        Task<ResultRest<FileRest>> GetInfoFile(string userKey, string userFileKey);

        Task<ResultRest<FileRest>> AddFileToUser(string userKey, Stream stream, string fileName, string extension);
        
        
        Task<ResultRest<(Dewey.Dms.FileService.Hbase.Views.File File,Stream Stream)>> GetFileToUser(string userKey, string userFileKey);

        Task<ResultRest<FileRest>> DeleteFileToUser(string userKey, string userFileKey);


        Task<ResultRest<FileRest>> ChangeFileToUser(string userKey, string userFileKey, Stream stream, string fileName,
            string extension);


    }
}