using Microsoft.AspNetCore.Mvc;
using DevTools.Dto.Plugins;
using DevTools.Services.Interfaces;
using MyWebAPI.Dto;
using DevTools.Helper.Converter;
using DevTools.Dto.Querry;
using System.Text.RegularExpressions;
using DevTools.Exceptions.Plugins.PluginsException.cs;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace DevTools.controllers
{
    [Route("Plugin")]
    [ApiController]
    public class PluginsController : ControllerBase
    {


        private readonly IPluginManagerService _pluginmanagerService;
        public PluginsController(IPluginManagerService _pluginmanagerService)
        {
            this._pluginmanagerService = _pluginmanagerService;
        }

        [HttpGet]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPluginsById(int id)
        {
            try
            {
                var plugin = await _pluginmanagerService.GetPLuginById(id);
                return Ok(plugin);
            }
            catch (PluginNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorRespones(ex.Message));
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetPluginsByQuerry([FromQuery] PluginQuerry querry)
        {
            try
            {
                var plugin = await _pluginmanagerService.GetAllByQuerry(querry);
                return Ok(plugin);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorRespones(ex.Message));
            }
        }


        [HttpPost("{id}/execute")]
        public async Task<IActionResult> ExecutePlugin(int id, [FromBody] object request)
        {
            try
            {
                var result = await _pluginmanagerService.Execute(id, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorRespones(ex.Message));
            }
        }


        [HttpGet("{id}/schema1")]
        public async Task<IActionResult> GetSchema1(int id)
        {
            try
            {
                var pluginUI = await _pluginmanagerService.GetScheme(id);

                if (pluginUI == null)
                {
                    return NotFound($"Plugin with id {id} not found.");
                }

                return Ok(pluginUI);
            }
            catch (PluginNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}/schema")]
        public async Task<IActionResult> GetSchema(int id)
        {
            try
            {
                var schema = await _pluginmanagerService.GetScheme(id);

                var json = JsonSerializer.Serialize(schema, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                return Ok(json);
            }
            catch (NotImplementedException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorRespones(ex.Message));
            }
        }

    }
}