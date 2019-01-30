using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dewey.Dms.FileService.Api.Models.View;
using Dewey.Dms.FileService.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dewey.Dms.FileService.Api.Controllers
{
   [Route("api/[controller]")]
  // [Route("api/dms")] 
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
        
        [HttpGet("files")]
        public async Task<IActionResult> GetFilesToUser()
        {
            string userKey = GetLoggedUserKey();
            _logger.Log(LogLevel.Information, $"Dewey.Dms.FileService.Api.Controllers.DmsController.GetFile(userKey={userKey}".ToString());
            try
            {
               var value = await _fileUserRepository.GetFilesToUser(GetLoggedUserKey());
              
                return Ok(value);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            
        }
        
        [HttpGet("files/{fileKey}")]
        public async Task<IActionResult> GetFile(string fileKey)
        {
            string userKey = GetLoggedUserKey();
            
            _logger.Log(LogLevel.Information, $"Dewey.Dms.FileService.Api.Controllers.DmsController.GetFile(userKey={userKey} , fileKey={fileKey})");
            try
            {
                var value= await _fileUserRepository.GetFile(userKey,fileKey);
                
                return Ok(value);

            }
            catch (Exception ex)
            {
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