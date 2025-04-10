using DevTools.Dto.UserPlugin;
using DevTools.Exceptions.AccountManager.UserException;
using DevTools.Exceptions.Plugins.PluginsException.cs;
using DevTools.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Dto;

namespace DevTools.controllers
{
    [Route("favorite")]
    [ApiController]
    public class UserPluginController : ControllerBase
    {
        private readonly IUserPluginService _userPluginService;
        public UserPluginController(IUserPluginService userPluginService)
        {
            this._userPluginService = userPluginService;
        }

        [HttpGet("{userid}")]
        [Authorize]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            try
            {
                var items = await _userPluginService.GetAllByUserId(userId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500 ,new ErrorRespones(ex.Message));
            }
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> AddFavoritePlugins(CreateUserPluginDTO createUserPlugin)
        {
            try
            {
                await _userPluginService.AddFavoritePlugin(createUserPlugin);
                return Created();
            }
            catch (UserNotFound ex)
            {
                return NotFound(new ErrorRespones(ex.Message));
            }
            catch (PluginNotFound ex)
            {
                return NotFound(new ErrorRespones(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500 ,new ErrorRespones(ex.Message));
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveFavoritePlugins(CreateUserPluginDTO createUserPlugin)
        {
            try
            {
                await _userPluginService.RemoveFavoritePlugin(createUserPlugin);
                return Ok("Successfully");
            }
            catch (UserNotFound ex)
            {
                return NotFound(new ErrorRespones(ex.Message));
            }
            catch (PluginNotFound ex)
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