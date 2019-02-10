using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Dewey.Dms.FileService.Api.Models.View;
using Dewey.Dms.FileService.Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dewey.Dms.FileService.Api.Controllers
{
   [Route("api/[controller]")]
  // [Route("api/dms")] 
  // [Consumes("multipart/form-data","application/json")]
   [ApiController]
   public class DmsController: ControllerBase
    {

        private readonly IFileUserRepository _fileUserRepository;
        private readonly ILogger _logger;
        public DmsController(IFileUserRepository fileUserRepository, ILogger<DmsController> logger)
        {
            _fileUserRepository = fileUserRepository;
            _logger = logger;
        }
        
        [HttpGet("files/info")]
        public async Task<IActionResult> GetInfoFilesToUser()
        {
            string userKey = GetLoggedUserKey();
            _logger.Log(LogLevel.Information, $"Dewey.Dms.FileService.Api.Controllers.DmsController.GetInfoFilesToUser(userKey={userKey}".ToString());
            try
            {
               var value = await _fileUserRepository.GetInfoFilesToUser(GetLoggedUserKey(),false);
              
                return Ok(value);

            }
            catch (Exception ex)
            {
                
                _logger.Log(LogLevel.Error,$"Dewey.Dms.FileService.Api.Controllers.DmsController.GetInfoFilesToUser(userKey={userKey}",ex);
                return BadRequest();
            }
            
        }
        
        [HttpGet("files/info/all")]
        public async Task<IActionResult> GetInfoFilesToUserAll()
        {
            string userKey = GetLoggedUserKey();
            _logger.Log(LogLevel.Information, $"Dewey.Dms.FileService.Api.Controllers.DmsController.GetInfoFilesToUserAll(userKey={userKey})".ToString());
            try
            {
                var value = await _fileUserRepository.GetInfoFilesToUser(GetLoggedUserKey());
              
                return Ok(value);

            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error,$"Dewey.Dms.FileService.Api.Controllers.DmsController.GetInfoFilesToUserAll(userKey={userKey})",ex);

                return BadRequest();
            }
            
        }
        
        
        [HttpGet("files/info/{userFileKey}")]
        public async Task<IActionResult> GetInfoFile(string userFileKey)
        {
            string userKey = GetLoggedUserKey();
            
            _logger.Log(LogLevel.Information, $"Dewey.Dms.FileService.Api.Controllers.DmsController.GetInfoFile(userKey={userKey} , fileKey={userFileKey})");
            try
            {
                var value= await _fileUserRepository.GetInfoFile(userKey,userFileKey);
                
                return Ok(value);

            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error,$"Dewey.Dms.FileService.Api.Controllers.DmsController.GetInfoFile(userKey={userKey} , fileKey={userFileKey})",ex);
                return BadRequest();
            }
            
        }
       
      
        
        [HttpPut("files/{userFileKey}")]
        public async Task<IActionResult> ChangeFileToUser(string userFileKey, 
            [FromForm(Name="file")] IFormFile formFile)
        {

            string userKey = GetLoggedUserKey();
            string fileName = formFile.FileName;
            string extension = "application/octet-stream";
            
            _logger.Log(LogLevel.Information,
                $"Dewey.Dms.FileService.Api.Controllers.DmsController.ChangeFileToUser(userKey={userKey} , userFileKey={userFileKey}, fileName = {fileName}, extension={extension})");

            try
            {
                var value = await _fileUserRepository.ChangeFileToUser(userKey, userFileKey , formFile.OpenReadStream(), fileName, extension);
                return Ok(value);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error,$"Dewey.Dms.FileService.Api.Controllers.DmsController.ChangeFileToUser(userKey={userKey} , userFileKey={userFileKey}, fileName = {fileName}, extension={extension})",ex);
                return BadRequest();
            }
        }
        
        
        [HttpPost("files")]
        public async Task<IActionResult> AddFileToUser([FromForm(Name="file")] IFormFile formFile)
        {

           

            string userKey = GetLoggedUserKey();
            string fileName = formFile.FileName;
            string extension = "application/octet-stream";
            
            _logger.Log(LogLevel.Information,
                $"Dewey.Dms.FileService.Api.Controllers.DmsController.AddFileToUser(userKey={userKey} , fileName = {fileName}, extension={extension})");

            try
            {
               var value = await _fileUserRepository.AddFileToUser(userKey, formFile.OpenReadStream(), fileName, extension);
               return Ok(value);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error,$"Dewey.Dms.FileService.Api.Controllers.DmsController.AddFileToUser(userKey={userKey} , fileName = {fileName}, extension={extension})",ex);
                return BadRequest();
            }
        }

        
        
        

        [HttpGet("files/{userFileKey}")]
        public async Task<IActionResult> GetFile(string userFileKey)
        {
            string userKey = GetLoggedUserKey();
            _logger.Log(LogLevel.Information, $"Dewey.Dms.FileService.Api.Controllers.DmsController.GetFile(userKey={userKey} , fileKey={userFileKey})");

            try
            {
                ResultRest<(Dewey.Dms.FileService.Hbase.Views.File File,Stream Stream)> value = await _fileUserRepository.GetFileToUser(userKey, userFileKey);
                if (value.IsError)
                    return Ok(value);
                return File(value.Result.Stream, value.Result.File.Extension, value.Result.File.FileName);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error,$"Dewey.Dms.FileService.Api.Controllers.DmsController.GetFile(userKey={userKey} , fileKey={userFileKey})",ex);
                return BadRequest();
            }
        }
        
        
        [HttpDelete("files/{userFileKey}")]
        public async Task<IActionResult> DeleteFile(string userFileKey)
        {
            string userKey = GetLoggedUserKey();
            _logger.Log(LogLevel.Information, $"Dewey.Dms.FileService.Api.Controllers.DmsController.DeleteFile(userKey={userKey} , fileKey={userFileKey})");

            try
            {
                ResultRest<FileRest> value = await _fileUserRepository.DeleteFileToUser(userKey, userFileKey);
                if (value.IsError)
                    return Ok(value);
                return Ok(value);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error,"Dewey.Dms.FileService.Api.Controllers.DmsController.DeleteFile(userKey={userKey} , fileKey={userFileKey})",ex);
                return BadRequest();
            }
        }
        
        
        
        private string GetLoggedUserKey()
        {
            return "46bfcc4d-4478-4a42-9a57-dfa049ca112f";
            ;
        }
    }
}