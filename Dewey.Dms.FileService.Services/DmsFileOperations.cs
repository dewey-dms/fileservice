using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Dewey.Dms.FileService.Hbase.Operations;
using Dewey.Dms.FileService.Hbase.Service;
using Dewey.Dms.FileService.Hbase.Views;
using Dewey.Dms.FileService.Hdfs.Client;
using File = Dewey.Dms.FileService.Hbase.Views.File;


namespace Dewey.Dms.FileService.Services
{
    public class DmsFileOperations:IDmsFileOperations
    {
        private IDatabaseService databaseService;
        private IHdfsClient hdfsClient;
      
        
        public DmsFileOperations(IDatabaseService databaseService, IHdfsClient hdfsClient )
        {
            this.databaseService = databaseService;
            this.hdfsClient = hdfsClient;
           
        }

        public async Task<ResultService<File>> AddFile(IFileServiceLogger fileServiceLogger , string userKey, Stream stream, string fileName, string extension)
        {
            fileServiceLogger.LogInfo($"Dewey.Dms.FileService.Services.AddFile(userKey={userKey},fileName={fileName},extension={extension})");

            try
            {
                User user = await databaseService.GetUser(userKey);
                if (user == null)
                {
                    fileServiceLogger.LogInfo(
                        $"Dewey.Dms.FileService.Services.AddFile(userKey={userKey},fileName={fileName},extension={extension}) - no such user");

                    return ResultService<File>.Error($"No such user {userKey}");
                }

                AddFileOperations addFileOperations = AddFileOperations.CreateFileOperations(user, fileName, extension);
                bool result = await hdfsClient.WriteStream(stream, $"/dewey/{addFileOperations.Key}");
                if (!result)
                {
                    fileServiceLogger.LogInfo(
                        $"Dewey.Dms.FileService.Services.AddFile(userKey={userKey},fileName={fileName},extension={extension}) - problem writing file");
                    return ResultService<File>.Error($"Internal server error");
                }

                await databaseService.DoFileOperations(addFileOperations);

                File file = await databaseService.GetFile(addFileOperations.KeyUserFile);
                if (file ==null)
                {
                    fileServiceLogger.LogInfo(
                        $"Dewey.Dms.FileService.Services.AddFile(userKey={userKey},fileName={fileName},extension={extension}) - problem with read writing file");

                    return ResultService<File>.Error($"Internal error");
                } 
                return ResultService<File>.Ok(file);

            }
            catch (Exception ex)
            {
                fileServiceLogger.LogError(
                    $"Dewey.Dms.FileService.Services.AddFile(userKey={userKey},fileName={fileName},extension={extension})",ex);
                return ResultService<File>.Error("Internal server error");
            }
        }
        
        
        public async Task<ResultService<IEnumerable<File>>> GetUserFile(IFileServiceLogger fileServicelogger , string userKey, bool? isDelete = null  )
        {

          
                
            fileServicelogger.LogInfo($"Dewey.Dms.FileService.Services.GetUserFile(userKey={userKey},isDelete={isDelete}");
            try
            {
                User user =  await databaseService.GetUser(userKey);
                if (user==null)
                    return ResultService<IEnumerable<File>>.Error($"No such user {userKey}");
                
                IEnumerable<File> file = await databaseService.GetUserFile(userKey, isDelete);
                return ResultService<IEnumerable<File>>.Ok(file);
            }
            catch (Exception ex)
            {
                fileServicelogger.LogError($"Dewey.Dms.FileService.Services.GetUserFile(userKey={userKey},isDelete={isDelete}",ex);
                return ResultService<IEnumerable<File>>.Error("Internal server error");
            }
        }

        public async Task<ResultService<File>> GetFile(IFileServiceLogger fileServicelogger , string userKey , string userFileKey)
        {
            
            fileServicelogger.LogInfo($"Dewey.Dms.FileService.Services.GetUserFile(userKey={userKey},userFileKey={userFileKey}");
            try
            {
                User user =  await databaseService.GetUser(userKey);
                if (user==null)
                    return ResultService<File>.Error($"No such user {userKey}");
                
                File file = await databaseService.GetFile(userFileKey);
                if (file == null)
                {
                    fileServicelogger.LogDebug(
                        $"Dewey.Dms.FileService.Services.GetUserFile(userKey={userKey},userFileKey={userFileKey}: No such file");
                    return ResultService<File>.Error("No such file");
                }
                
                
                if (file.KeyUser == userKey)
                    return ResultService<File>.Ok(file);
                else
                {
                    fileServicelogger.LogDebug(
                        $"Dewey.Dms.FileService.Services.GetUserFile(userKey={userKey},userFileKey={userFileKey}: Permission denied to file");
                    return ResultService<File>.Error("No such file");
                    
                }
            }
            catch (Exception ex)
            {
                fileServicelogger.LogError($"Dewey.Dms.FileService.Services.GetUserFile(userKey={userKey},userFileKey={userFileKey}",ex);
                return ResultService<File>.Error("Internal server error");
            }
        }
        
        
    }
    
}