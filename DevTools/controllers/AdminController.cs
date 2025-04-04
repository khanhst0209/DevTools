using DevTools.Exceptions.Plugins.PluginsException.cs;
using DevTools.Repositories.Interfaces;
using DevTools.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevTools.controllers
{
    [Route("Account")]
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

    }
}