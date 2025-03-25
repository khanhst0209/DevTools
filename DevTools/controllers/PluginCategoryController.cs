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
    }
}