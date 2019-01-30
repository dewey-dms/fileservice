using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dewey.Dms.FileService.Api.Models.View;
using Dewey.Dms.FileService.Hbase.Service;
using Dewey.Dms.FileService.Services;
using Microsoft.Extensions.Logging;
using File = Dewey.Dms.FileService.Api.Models.View.File;

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


        public async Task<ResultRest<List<File>>> GetFilesToUser(string userKey)
        {
           _logger.Log(LogLevel.Information,$"Dewey.Dms.FileService.Api.Repository.HbaseFileUserRepository.GetFileToUser(userKey={userKey}");
           //IEnumerable<Dewey.Dms.FileService.Hbase.Views.File> files = await _databaseService.GetUserFile(userKey, isDelete: false);

           ResultService<IEnumerable<Dewey.Dms.FileService.Hbase.Views.File>> result =
               await _databaseService.GetUserFile(_serviceLogger,userKey, isDelete: false);

           return new ResultRest<List<File>>(result.Map<List<File>>(a => a.Select(b=>new File(b)).ToList())
               .Ensure(a=>a.Count>0,"List files is empty"));
        }
        
        
        public async Task<ResultRest<File>> GetFile(string userKey , string userFileKey)
        {
            _logger.Log(LogLevel.Information,$"Dewey.Dms.FileService.Api.Repository.HbaseFileUserRepository.GetFile(userFileKey={userFileKey}");
             ResultService<Dewey.Dms.FileService.Hbase.Views.File> result = await _databaseService.GetFile(_serviceLogger,userKey, userFileKey);
             return new ResultRest<File>(result.Map<File>(a=> new File(a)));
        }


        public async Task<ResultRest<File>> AddFile(string userKey, Stream stream, string fileName, string extension)
        {
            _logger.Log(LogLevel.Information, message:$"Dewey.Dms.FileService.Api.Repository.HbaseFileUserRepository.AddFile(userFile={userKey},fileName={fileName},extensions={extension}");
            ResultService<Dewey.Dms.FileService.Hbase.Views.File> result =
                await _databaseService.AddFile(_serviceLogger, userKey, stream, fileName, extension);
            return new ResultRest<File>(result.Map<File>(a=>new File(a)));
        }
        
        
      }
}