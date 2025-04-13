using DevTools.Application.Dto;
using DevTools.Application.Dto.File;
using DevTools.Application.Exceptions.UploadFile;
using DevTools.Exceptions.Plugins.PluginsException.cs;
using DevTools.Repositories.Interfaces;
using DevTools.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Dto;
using MyWebAPI.Services.Interfaces;

namespace DevTools.controllers
{
    [Route("api/v1/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IPluginManagerService _pluginManagerService;
        private readonly IAccountManagerService _accountManagerService;

        public AdminController(IPluginManagerService pluginManagerService,
        IAccountManagerService accountManagerService)
        {
            _pluginManagerService = pluginManagerService;
            _accountManagerService = accountManagerService;
        }

        [HttpPost("{pluginId}/premium")]
        public async Task<IActionResult> SetPremiumStatus(int pluginId, [FromQuery] bool isPremium)
        {
            try
            {
                await _pluginManagerService.SetPremiumStatus(pluginId, isPremium);
                return Ok($"Pluing with id: {pluginId} has been change IsPremium to : {isPremium}");
            }
            catch (Exception ex)
            {
                if (ex is PluginNotFound)
                {
                    return NotFound(ex.Message);
                }
                else
                {
                    return StatusCode(500, new ErrorRespones(ex.Message));
                }
            }
        }

        [HttpPost("{pluginId}/activation")]
        public async Task<IActionResult> SetActiveStatus(int pluginId, [FromQuery] bool isActive)
        {
            try
            {
                await _pluginManagerService.SetActiveStatus(pluginId, isActive);
                return Ok($"Pluing with id: {pluginId} has been change Activation to : {isActive}");
            }
            catch (Exception ex)
            {
                if (ex is PluginNotFound)
                {
                    return NotFound(ex.Message);
                }
                else
                {
                    return StatusCode(500, new ErrorRespones(ex.Message));
                }
            }
        }
        [HttpDelete("/plugin/{pluginId}")]
        public async Task<IActionResult> RemovePlugin(int pluginId)
        {
            try
            {
                await _pluginManagerService.DeletePluginByIdAsync(pluginId);
                return Ok(new SuccessRespone("Plugin was removed successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Plugin")]
        public async Task<IActionResult> GetAllPlugin()
        {
            try
            {
                var plugins = await _pluginManagerService.GetAllPlugin();

                return Ok(plugins);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorRespones(ex.Message));
            }
        }


        [HttpPost("plugin")]
        [RequestSizeLimit(100_100_100)]
        public async Task<IActionResult> UploadPlugin([FromForm] UploadFileDTO upload)
        {
            try
            {
                await _pluginManagerService.UploadLoadPlugin(upload);
                return Ok(new SuccessRespone("Files was uploaded successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorRespones(ex.Message));
            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _accountManagerService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorRespones(ex.Message));
            }
        }

    }
}