using Microsoft.AspNetCore.Mvc;
using DevTools.Dto.Plugins;
using DevTools.Services.Interfaces;
using MyWebAPI.Dto;
using DevTools.Helper.Converter;
using DevTools.Dto.Querry;
using System.Text.RegularExpressions;
using DevTools.Exceptions.Plugins.PluginsException.cs;


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


        [HttpPost("execute")]
        public async Task<IActionResult> ExecutePlugin([FromBody] PluginRequest request)
        {
            try
            {
                object inputData = Converter.ConvertJsonElement(request.Input);

                if (inputData == null)
                {
                    Console.WriteLine("Input is null.");
                }
                else if (inputData is Dictionary<string, object> dict)
                {
                    Console.WriteLine("Input is an object with key-value pairs:");
                    foreach (var kvp in dict)
                    {
                        Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}, Type: {kvp.Value?.GetType().Name ?? "null"}");
                    }
                }
                else if (inputData is List<object> list)
                {
                    Console.WriteLine("Input is an array:");
                    for (int i = 0; i < list.Count; i++)
                    {
                        Console.WriteLine($"Index: {i}, Value: {list[i]}, Type: {list[i]?.GetType().Name ?? "null"}");
                    }
                }
                else
                {
                    Console.WriteLine($"Input is a single value: {inputData}, Type: {inputData.GetType().Name}");
                }

                var result = await _pluginmanagerService.Execute(request.Id, inputData);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorRespones(ex.Message));
            }
        }


        [HttpGet("{id}/schema")]
        public async Task<IActionResult> GetSchema(int id)
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

        [HttpGet("{id}/schema1")]
        public async Task<IActionResult> GetSchema1(int id)
        {
            try
            {
                var schema = await _pluginmanagerService.GetScheme1(id);
                return Ok(schema);
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