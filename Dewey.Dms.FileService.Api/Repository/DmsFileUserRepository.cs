using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dewey.Dms.FileService.Api.Models.View;
using Dewey.Dms.FileService.Hbase.Service;
using Dewey.Dms.FileService.Services;
using Microsoft.Extensions.Logging;
using File = Dewey.Dms.FileService.Api.Models.View.FileRest;

namespace Dewey.Dms.FileService.Api.Repository
{
    public class DmsFileUserRepository:IFileUserRepository
    {
        private readonly IDmsFileOperations _databaseService;
        private readonly ILogger _logger;
        private readonly IFileServiceLogger _serviceLogger;
        public DmsFileUserRepository(IDmsFileOperations databaseService, ILogger<DmsFileUserRepository> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
            this._serviceLogger = new ApiFileServiceLogger(logger);
        }


        public async Task<ResultRest<List<FileRest>>> GetInfoFilesToUser(string userKey)
        {
           _logger.Log(LogLevel.Information,$"Dewey.Dms.FileService.Api.Repository.HbaseFileUserRepository.GetInfoFilesToUser(userKey={userKey}");
           //IEnumerable<Dewey.Dms.FileService.Hbase.Views.File> files = await _databaseService.GetUserFile(userKey, isDelete: false);

           ResultService<IEnumerable<Dewey.Dms.FileService.Hbase.Views.File>> result =
               await _databaseService.GetInfoFilesToUser(_serviceLogger,userKey, isDelete: false);

           return new ResultRest<List<FileRest>>(result.Map<List<FileRest>>(a => a.Select(b=>new FileRest(b)).ToList())
               .Ensure(a=>a.Count>0,"List files is empty"));
        }
        
        
        public async Task<ResultRest<FileRest>> GetInfoFile(string userKey , string userFileKey)
        {
            _logger.Log(LogLevel.Information,$"Dewey.Dms.FileService.Api.Repository.HbaseFileUserRepository.GetInfoFile(userFileKey={userFileKey} , userFileKey= {userFileKey})");
             ResultService<Dewey.Dms.FileService.Hbase.Views.File> result = await _databaseService.GetInfoFile(_serviceLogger,userKey, userFileKey);
             return new ResultRest<FileRest>(result.Map<FileRest>(a=> new FileRest(a)));
        }


        public async Task<ResultRest<FileRest>> AddFileToUser(string userKey, Stream stream, string fileName, string extension)
        {
            _logger.Log(LogLevel.Information, message:$"Dewey.Dms.FileService.Api.Repository.HbaseFileUserRepository.AddFileToUser(userFile={userKey},fileName={fileName},extensions={extension}");
            ResultService<Dewey.Dms.FileService.Hbase.Views.File> result =
                await _databaseService.AddFileToUser(_serviceLogger, userKey, stream, fileName, extension);
            return new ResultRest<FileRest>(result.Map<FileRest>(a=>new FileRest(a)));
        }

        public async Task<ResultRest<(Dewey.Dms.FileService.Hbase.Views.File File,Stream Stream)>> GetFileToUser(string userKey, string userFileKey)
        {
            _logger.Log(LogLevel.Information,$"Dewey.Dms.FileService.Api.Repository.HbaseFileUserRepository.GetFileToUser(userFileKey={userFileKey} , userFileKey= {userFileKey})");
            ResultService<(Dewey.Dms.FileService.Hbase.Views.File,Stream)> result =
                await _databaseService.GetFileToUser(_serviceLogger, userKey, userFileKey);
            return new ResultRest<(Dewey.Dms.FileService.Hbase.Views.File,Stream)>(result);

        }
    }
}