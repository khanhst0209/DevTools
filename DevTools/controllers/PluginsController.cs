using Microsoft.AspNetCore.Mvc;
using Plugins.Manager;
using DevTools.Dto.Plugins;
using System.Security.Claims;

namespace DevTools.controllers
{
    [Route("api/plugins")]
    [ApiController]
    public class PluginsController : ControllerBase
    {
        [HttpGet("plugins")]
        public IActionResult GetPlugins()
        {
            var plugins = PluginManager.GetPlugins().Select(p => new { p.Name, p.Category, p.id });
            return Ok(plugins);
        }

        [HttpPost("execute")]
        public IActionResult ExecutePlugin([FromBody] PluginRequest request)
        {
            try
            {
                var plugin = PluginManager.GetPlugins().FirstOrDefault(p => p.id == request.id);
                if (plugin == null) return NotFound("Plugin not found");
               

                string result = plugin.Execute(request.Input.ToString()).ToString();
                return Ok(new { Result = result });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}