using Microsoft.AspNetCore.Mvc;
using Plugins.Manager;
using DevTools.Dto.Plugins;

namespace DevTools.controllers
{
    [Route("api/plugins")]
    [ApiController]
    public class PluginsController : ControllerBase
    {
        [HttpGet("list")]
        public IActionResult GetPlugins()
        {
            var plugins = PluginManager.GetPlugins().Select(p => new { p.Name, p.Category });
            return Ok(plugins);
        }

        [HttpPost("execute")]
        public IActionResult ExecutePlugin([FromBody] PluginRequest request)
        {
            var plugin = PluginManager.GetPlugins().FirstOrDefault(p => p.Name == request.PluginName);
            if (plugin == null) return NotFound("Plugin not found");

            string result = plugin.Execute(request.Input.ToString()).ToString();
            return Ok(new { Result = result });
        }
    }
}