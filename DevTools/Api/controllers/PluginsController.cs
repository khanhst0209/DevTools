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
using DevTools.Api.Response;
using DevTools.data;
using DevTools.Dto.Category;


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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPluginsById(int id)
        {
            var plugin = await _pluginManagerService.GetPLuginById(id);
            return Ok(new SuccessResponseBuilder<PluginsResponeDTO>()
            .WithData(plugin).WithMessage("Get Successfully").WithStatusCode(200).Build());
        }

        [HttpGet()]
        public async Task<IActionResult> GetPluginsByQuerry([FromQuery] PluginQuerry querry)
        {
            var plugin = await _pluginManagerService.GetAllByQuerry(querry);
            return Ok(plugin);
        }


        [HttpPost("{id}/execution")]
        public async Task<IActionResult> ExecutePlugin(int id, [FromBody] object request)
        {
            try
            {
                var result = await _pluginManagerService.Execute(id, request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorRespones(ex.Message));
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
            catch (PluginNotFound ex)
            {
                return NotFound(new ErrorRespones(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorRespones(ex.Message));
            }
        }

    }
}