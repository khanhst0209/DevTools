using Microsoft.AspNetCore.Mvc;
using DevTools.Dto.Plugins;
using System.Security.Claims;
using DevTools.Services.Interfaces;
using MyWebAPI.Dto;
using System.Threading.Tasks;

namespace DevTools.controllers
{
    [Route("api/plugins")]
    [ApiController]
    public class PluginsController : ControllerBase
    {


        private readonly IPluginManagerService _pluginmanagerService;
        public PluginsController(IPluginManagerService _pluginmanagerService)
        {
            this._pluginmanagerService = _pluginmanagerService;
        }
        [HttpGet("plugins")]
        public async Task<IActionResult> GetPlugins()
        {
            try
            {
                var plugin = await _pluginmanagerService.GetAllActivePlugin();
                return Ok(plugin);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorRespones(ex.Message));
            }
        }

        [HttpPost("execute")]
        public IActionResult ExecutePlugin([FromBody] PluginRequest request)
        {
            try
            {
                var result = _pluginmanagerService.Execute(request.id, request.Input.ToString());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorRespones(ex.Message));
            }
        }
    }
}