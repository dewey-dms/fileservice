using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Dewey.Dms.FileService.Hbase.Operations;
using Dewey.Dms.FileService.Hbase.Service;
using Dewey.Dms.FileService.Hbase.Views;
using Dewey.Dms.FileService.Hdfs.Client;
using File = Dewey.Dms.FileService.Hbase.Views.File;
using Dewey.Dms.Core;


namespace Dewey.Dms.FileService.Services
{
    public class DmsFileOperations:IDmsFileOperations
    {
        private readonly IDatabaseService _databaseService;
        private readonly IHdfsClient _hdfsClient;
      
        
        public DmsFileOperations(IDatabaseService databaseService, IHdfsClient hdfsClient )
        {
            this._databaseService = databaseService;
            this._hdfsClient = hdfsClient;
           
        }

        public async Task<ResultService<File>> ChangeFileToUser(IFileServiceLogger fileServiceLogger, string userKey,
            string userFileKey, Stream stream, string fileName, string extension)
        {
            fileServiceLogger.LogInfo($"Dewey.Dms.FileService.Services.ChangeFileToUser(userKey={userKey}, userFileKey={userFileKey}, fileName={fileName},extension={extension})");
            try
            {
                User user = await _databaseService.GetUser(userKey);
                if (user == null)
                {
                    fileServiceLogger.LogInfo(
                        $"Dewey.Dms.FileService.Services.ChangeFileToUser(userKey={userKey}, userFileKey={userFileKey},fileName={fileName},extension={extension}) - no such user");

                    return ResultService<File>.Error($"No such user {userKey}");
                }
                
                File file = await _databaseService.GetFile(userFileKey);
                if (file == null)
                {
                    fileServiceLogger.LogDebug(
                        $"Dewey.Dms.FileService.Services.ChangeFileToUser(userKey={userKey}, userFileKey={userFileKey},fileName={fileName},extension={extension}): No such file");
                    return ResultService<File>.Error("No such file");
                }

                if (file.KeyUser != userKey)
                {
                    fileServiceLogger.LogDebug(
                        $"Dewey.Dms.FileService.Services.ChangeFileToUser(userKey={userKey}, userFileKey={userFileKey},fileName={fileName},extension={extension}): Permission denied to file");
                    return ResultService<File>.Error("No such file");
                }

                ResultService<ChangeFileOperations> resultChangeFileOperations =
                    ChangeFileOperations.CreateFileOperations(file)
                        .NextResult(a=>a.ChangeFileName(fileName))
                        .NextResult(a=>a.ChangeExtension(extension));
                
                ChangeFileOperations changeFileOperations = resultChangeFileOperations.Value;
                
                if (resultChangeFileOperations.IsError)
                {
                    
                        fileServiceLogger.LogDebug(
                            $"Dewey.Dms.FileService.Services.ChangeFileToUser(userKey={userKey}, userFileKey={userFileKey},fileName={fileName},extension={extension}): {resultChangeFileOperations.ErrorMessage}");
                    return ResultService<File>.Error(resultChangeFileOperations.ErrorMessage);
                }
                bool result = await _hdfsClient.WriteStream(stream, $"/dewey/{changeFileOperations.Key}");
                if (!result)
                {
                    fileServiceLogger.LogInfo(
                        $"Dewey.Dms.FileService.Services.ChangeFileToUser(userKey={userKey}, userFileKey={userFileKey},fileName={fileName},extension={extension}):- problem writing file");
                    return ResultService<File>.Error($"Internal server error");
                }
                await _databaseService.DoFileOperations(changeFileOperations);
                File fileChange = await _databaseService.GetFile(changeFileOperations.KeyUserFile);
                if (fileChange ==null)
                {
                    fileServiceLogger.LogInfo(
                        $"Dewey.Dms.FileService.Services.ChangeFileToUser(userKey={userKey}, userFileKey={userFileKey},fileName={fileName},extension={extension}):- problem with read updating file");
                    return ResultService<File>.Error($"Internal server error");

                 } 
                return ResultService<File>.Ok(fileChange);

            }
            catch (Exception ex)
            {
                fileServiceLogger.LogError(
                    $"Dewey.Dms.FileService.Services.AddFileToUser(userKey={userKey}, userFileKey={userFileKey},fileName={fileName},extension={extension})",ex);
                return ResultService<File>.Error("Internal server error");
            }
        } 
        
        public async Task<ResultService<File>> AddFileToUser(IFileServiceLogger fileServiceLogger , string userKey, Stream stream, string fileName, string extension)
        {
            fileServiceLogger.LogInfo($"Dewey.Dms.FileService.Services.AddFileToUser(userKey={userKey},fileName={fileName},extension={extension})");

            try
            {
                User user = await _databaseService.GetUser(userKey);
                if (user == null)
                {
                    fileServiceLogger.LogInfo(
                        $"Dewey.Dms.FileService.Services.AddFileToUser(userKey={userKey},fileName={fileName},extension={extension}) - no such user");

                    return ResultService<File>.Error($"No such user {userKey}");
                }

                ResultService<AddFileOperations> resultAddFileOperations = AddFileOperations.CreateFileOperations(user, fileName, extension);
                if (resultAddFileOperations.IsError)
                {
                    fileServiceLogger.LogInfo(
                        $"Dewey.Dms.FileService.Services.AddFileToUser(userKey={userKey},fileName={fileName},extension={extension}) - {resultAddFileOperations.ErrorMessage}");

                    return ResultService<File>.Error(resultAddFileOperations.ErrorMessage);
                }

                AddFileOperations addFileOperations = resultAddFileOperations.Value;
                
                
                bool result = await _hdfsClient.WriteStream(stream, $"/dewey/{addFileOperations.Key}");
                if (!result)
                {
                    fileServiceLogger.LogInfo(
                        $"Dewey.Dms.FileService.Services.AddFileToUser(userKey={userKey},fileName={fileName},extension={extension}) - problem writing file");
                    return ResultService<File>.Error($"Internal server error");
                }

                await _databaseService.DoFileOperations(addFileOperations);

                File file = await _databaseService.GetFile(addFileOperations.KeyUserFile);
                if (file ==null)
                {
                    fileServiceLogger.LogInfo(
                        $"Dewey.Dms.FileService.Services.AddFileToUser(userKey={userKey},fileName={fileName},extension={extension}) - problem with read writing file");

                    return ResultService<File>.Error($"Internal error");
                } 
                return ResultService<File>.Ok(file);

            }
            catch (Exception ex)
            {
                fileServiceLogger.LogError(
                    $"Dewey.Dms.FileService.Services.AddFileToUser(userKey={userKey},fileName={fileName},extension={extension})",ex);
                return ResultService<File>.Error("Internal server error");
            }
        }

        public async Task<ResultService<File>> DeleteFileToUser(IFileServiceLogger fileServiceLogger, string userKey, string userFileKey)
        {
            try
            {
                User user = await _databaseService.GetUser(userKey);
                if (user == null)
                {
                    fileServiceLogger.LogDebug(
                        $"Dewey.Dms.FileService.Services.GetInfoFile(userKey={userKey},userFileKey={userFileKey}) - no such user");
                    return ResultService<File>.Error($"No such user {userKey}");
                }

                File file = await _databaseService.GetFile(userFileKey);
                if (file == null)
                {
                    fileServiceLogger.LogDebug(
                        $"Dewey.Dms.FileService.Services.GetInfoFile(userKey={userKey},userFileKey={userFileKey}: No such file");
                    return ResultService<File>.Error("No such file");
                }

                if (file.KeyUser != userKey)
                {
                    fileServiceLogger.LogDebug(
                        $"Dewey.Dms.FileService.Services.GetInfoFile(userKey={userKey},userFileKey={userFileKey}: Permission denied to file");
                    return ResultService<File>.Error("No such file");
                }

                ResultService<DeleteFileOperations> resultDeleteFileOperations =
                    DeleteFileOperations.CreateFileOperations(file);
                if (resultDeleteFileOperations.IsError)
                {
                    fileServiceLogger.LogDebug(
                        $"Dewey.Dms.FileService.Services.GetInfoFile(userKey={userKey},userFileKey={userFileKey}: {resultDeleteFileOperations.ErrorMessage}");
                    return ResultService<File>.Error(resultDeleteFileOperations.ErrorMessage);
                }

                DeleteFileOperations deleteFileOperations = resultDeleteFileOperations.Value;
                await _databaseService.DoFileOperations(deleteFileOperations);
                
                
                File fileDelete = await _databaseService.GetFile(userFileKey);
                if (fileDelete == null)
                {
                    fileServiceLogger.LogDebug(
                        $"Dewey.Dms.FileService.Services.GetInfoFile(userKey={userKey},userFileKey={userFileKey}: Problem with reading deleted file");
                    return ResultService<File>.Error("Internal Error");
                }
                
                
                return ResultService<File>.Ok(fileDelete);

            }
            catch (Exception ex)
            {
                fileServiceLogger.LogError(
                     $"Dewey.Dms.FileService.Services.GetInfoFile(userKey={userKey},userFileKey={userFileKey}",ex);
                return ResultService<File>.Error("Internal server error");
            }
        }


        public async Task<ResultService<(File File,Stream Stream)>> GetFileToUser(IFileServiceLogger fileServiceLogger, string userKey, string userFileKey)
        {
            fileServiceLogger.LogInfo($"Dewey.Dms.FileService.Services.GetFileToUser(userKey={userKey},userFileKey={userFileKey}");

            try
            {
                User user = await _databaseService.GetUser(userKey);
                if (user == null)
                {
                    fileServiceLogger.LogInfo(
                        $"Dewey.Dms.FileService.Services.GetFileToUser(userKey={userKey},userFileKey={userFileKey}) - No such user");

                    return ResultService<(File,Stream)>.Error($"No such user {userKey}");
                }

                File file = await _databaseService.GetFile(userFileKey);
                if (file == null)
                {
                    fileServiceLogger.LogDebug(
                        $"Dewey.Dms.FileService.Services.GetInfoFile(userKey={userKey},userFileKey={userFileKey}: No such file");
                    return ResultService<(File,Stream)>.Error("No such file");
                }
                    
                if (file.KeyUser != userKey)
                {
                    fileServiceLogger.LogDebug(
                        $"Dewey.Dms.FileService.Services.GetInfoFile(userKey={userKey},userFileKey={userFileKey}: Perimission denied");
                    return ResultService<(File,Stream)>.Error("No such file");
                }
                
                MemoryStream streamToRead = new MemoryStream();
                streamToRead.Position = 0;


                bool result = _hdfsClient.ReadStream(streamToRead, $"/dewey/{file.LongKey}").Result;
                if (!result)
                {
                    fileServiceLogger.LogInfo(
                        $"Dewey.Dms.FileService.Services.GetFileToUser(userKey={userKey},userFileKey={userFileKey}) - Problem reading file");
                    return ResultService<(File,Stream)>.Error($"Internal server error");
                }

                streamToRead.Position = 0;
                return ResultService<(File,Stream)>.Ok((file,streamToRead));


            }
            catch (Exception ex)
            {
                fileServiceLogger.LogError(
                    $"Dewey.Dms.FileService.Services.GetFileToUser(userKey={userKey},userFileKey={userFileKey})",ex);
                return ResultService<(File,Stream)>.Error($"Internal server error");
            }
            
            
        }


        public async Task<ResultService<IEnumerable<File>>> GetInfoFilesToUser(IFileServiceLogger fileServiceLogger , string userKey, bool? isDelete = null  )
        {

          
                
            fileServiceLogger.LogInfo($"Dewey.Dms.FileService.Services.GetInfoFilesToUser(userKey={userKey},isDelete={isDelete})");
            try
            {
                User user =  await _databaseService.GetUser(userKey);
                if (user == null)
                {
                    fileServiceLogger.LogInfo($"Dewey.Dms.FileService.Services.GetInfoFilesToUser(userKey={userKey},isDelete={isDelete}) - No such user" );
                    return ResultService<IEnumerable<File>>.Error($"No such user {userKey}");
                }

                IEnumerable<File> file = await _databaseService.GetUserFile(userKey, isDelete);
                return ResultService<IEnumerable<File>>.Ok(file);
            }
            catch (Exception ex)
            {
                fileServiceLogger.LogError($"Dewey.Dms.FileService.Services.GetInfoFilesToUser(userKey={userKey},isDelete={isDelete})",ex);
                return ResultService<IEnumerable<File>>.Error("Internal server error");
            }
        }

        public async Task<ResultService<File>> GetInfoFile(IFileServiceLogger fileServiceLogger , string userKey , string userFileKey)
        {
            
            fileServiceLogger.LogInfo($"Dewey.Dms.FileService.Services.GetInfoFile(userKey={userKey},userFileKey={userFileKey}");
            try
            {
                User user =  await _databaseService.GetUser(userKey);
                if (user==null)
                    return ResultService<File>.Error($"No such user {userKey}");
                
                File file = await _databaseService.GetFile(userFileKey);
                if (file == null)
                {
                    fileServiceLogger.LogDebug(
                        $"Dewey.Dms.FileService.Services.GetInfoFile(userKey={userKey},userFileKey={userFileKey}: No such file");
                    return ResultService<File>.Error("No such file");
                }
                
                
                if (file.KeyUser == userKey)
                    return ResultService<File>.Ok(file);
                else
                {
                    fileServiceLogger.LogDebug(
                        $"Dewey.Dms.FileService.Services.GetInfoFile(userKey={userKey},userFileKey={userFileKey}: Permission denied to file");
                    return ResultService<File>.Error("No such file");
                    
                }
            }
            catch (Exception ex)
            {
                fileServiceLogger.LogError($"Dewey.Dms.FileService.Services.GetInfoFile(userKey={userKey},userFileKey={userFileKey}",ex);
                return ResultService<File>.Error("Internal server error");
            }
        }
        
        
     
        
        
    }
    
}