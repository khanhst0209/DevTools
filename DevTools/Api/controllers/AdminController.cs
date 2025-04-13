using DevTools.Exceptions.Plugins.PluginsException.cs;
using DevTools.Repositories.Interfaces;
using DevTools.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Dto;

namespace DevTools.controllers
{
    [Route("api/v1/admin")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IPluginManagerService _pluginManagerService;

        public AdminController(IPluginManagerService _pluginManagerService)
        {
            this._pluginManagerService = _pluginManagerService;
        }

        [HttpPost("{pluginId}/premium")]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemovePlugin(int pluginId)
        {
            try
            {
                await _pluginManagerService.DeletePluginByIdAsync(pluginId);
                return Ok("oke");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}