using Microsoft.AspNetCore.Mvc;
using DevTools.Dto.Plugins;
using DevTools.Services.Interfaces;
using MyWebAPI.Dto;
using DevTools.Helper.Converter;
using DevTools.Dto.Querry;
using System.Text.RegularExpressions;


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
                Console.WriteLine($"Data type of input: {inputData.GetType()}");
                var result = await _pluginmanagerService.Execute(request.Id, inputData);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorRespones(ex.Message));
            }
        }

        [HttpGet("{id}/schema1")]
        public async Task<IActionResult> GetSchema(int id)
        {
            var result = await _pluginmanagerService.GetScheme1(id);
            return Ok(result);
        }


    }
}