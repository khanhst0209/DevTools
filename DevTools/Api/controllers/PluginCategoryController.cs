using DevTools.Exceptions.Plugins.PluginCategoryException.cs;
using DevTools.Services;
using DevTools.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Dto;

namespace DevTools.controllers
{

    [Route("plugin-category")]
    [ApiController]
    public class PluginCategoryController : ControllerBase
    {
        private readonly IPluginCategoryService _pluginCategoryService;
        public PluginCategoryController(IPluginCategoryService pluginCategoryService)
        {
            this._pluginCategoryService = pluginCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesWithPlugins()
        {
            try
            {
                var categories = await _pluginCategoryService.GetAllCategoryWithplugin();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500 ,new ErrorRespones(ex.Message));
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _pluginCategoryService.GetCategoryById(id);
                return Ok(category);
            }
            catch(PluginCategoryNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500,new ErrorRespones(ex.Message));
            }
        }
    }
}