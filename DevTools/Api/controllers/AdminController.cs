using DevTools.Exceptions.Plugins.PluginsException.cs;
using DevTools.Repositories.Interfaces;
using DevTools.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DevTools.controllers
{
    [Route("Admin")]
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
                if(ex is PluginNotFound)
                {
                    return NotFound(ex.Message);
                }
                else{
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost("{pluginId}/activation")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetActiveStatus(int pluginId, [FromQuery] bool IsActive)
        {
            try
            {
                await _pluginManagerService.SetActiveStatus(pluginId, IsActive);
                return Ok($"Pluing with id: {pluginId} has been change Activation to : {IsActive}");
            }
            catch (Exception ex)
            {
                if(ex is PluginNotFound)
                {
                    return NotFound(ex.Message);
                }
                else{
                    return BadRequest(ex.Message);
                }
            }
        }
        [HttpDelete("{pluginId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemovePlugin(int pluginId)
        {
            return NotFound("Cai nay chua lam, hoi bi luoi");
        }

    }
}