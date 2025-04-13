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
    [Route("api/v1/plugin")]
    [ApiController]
    public class PluginsController : ControllerBase
    {
        private readonly IPluginManagerService _pluginManagerService;
        public PluginsController(IPluginManagerService pluginManagerService)
        {
            this._pluginManagerService = pluginManagerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlugins()
        {
            try
            {
                var plugin = await _pluginManagerService.GetAllActivePlugin();
                return Ok(plugin);
            }
            catch (Exception ex)
            {
                return StatusCode(500 ,new ErrorRespones(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPluginsById(int id)
        {
            try
            {
                var plugin = await _pluginManagerService.GetPLuginById(id);
                return Ok(plugin);
            }
            catch (PluginNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500 ,new ErrorRespones(ex.Message));
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetPluginsByQuerry([FromQuery] PluginQuerry querry)
        {
            try
            {
                var plugin = await _pluginManagerService.GetAllByQuerry(querry);
                return Ok(plugin);
            }
            catch (Exception ex)
            {
                return StatusCode(500 ,new ErrorRespones(ex.Message));
            }
        }


        [HttpPost("{id}/execution")]
        public async Task<IActionResult> ExecutePlugin(int id, [FromBody] object request)
        {
            try
            {
                var result = await _pluginManagerService.Execute(id, request);
                return Ok(result);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500 ,new ErrorRespones(ex.Message));
            }
        }


        [HttpGet("{id}/schema1")]
        public async Task<IActionResult> GetSchema1(int id)
        {
            try
            {
                var pluginUI = await _pluginManagerService.GetScheme(id);

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
                return StatusCode(500 ,new ErrorRespones(ex.Message));
            }
        }

        [HttpGet("{id}/schema")]
        public async Task<IActionResult> GetSchema(int id)
        {
            try
            {
                var schema = await _pluginManagerService.GetScheme(id);

                var json = JsonSerializer.Serialize(schema, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                return Ok(json);
            }
            catch (NotImplementedException ex)
            {
                return NotFound(new ErrorRespones(ex.Message));
            }
            catch(PluginNotFound ex)
            {
                return NotFound(new ErrorRespones(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500 ,new ErrorRespones(ex.Message));
            }
        }

    }
}