using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Dewey.Dms.FileService.Api.Models.View;
using File = Dewey.Dms.FileService.Api.Models.View.File;

namespace Dewey.Dms.FileService.Api.Repository
{
    public interface IFileUserRepository
    {
        Task<ResultRest<List<File>>> GetFilesToUser(string userKey);

        Task<ResultRest<File>> GetFile(string userKey, string userFileKey);

        Task<ResultRest<File>> AddFile(string userKey, Stream stream, string fileName, string extension);

    }
}