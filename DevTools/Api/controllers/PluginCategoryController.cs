using DevTools.Exceptions.Plugins.PluginCategoryException.cs;
using DevTools.Services;
using DevTools.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Dto;

namespace DevTools.controllers
{

    [Route("PluginCategory")]
    [ApiController]
    public class PluginCategoryController : ControllerBase
    {
        private readonly IPluginCategoryService _plugincategoryservice;
        public PluginCategoryController(IPluginCategoryService _plugincategoryservice)
        {
            this._plugincategoryservice = _plugincategoryservice;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesWithPlugins()
        {
            try
            {
                var categories = await _plugincategoryservice.GetAllCategoryWithplugin();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorRespones(ex.Message));
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _plugincategoryservice.GetCategoryById(id);
                return Ok(category);
            }
            catch(PluginCategoryNotFound ex)
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