using System.Security.Claims;
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
    [Route("api/v1/me/favorite")]
    [ApiController]
    public class UserPluginController : ControllerBase
    {
        private readonly IUserPluginService _userPluginService;
        public UserPluginController(IUserPluginService userPluginService)
        {
            this._userPluginService = userPluginService;
        }

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetAllByUserId()
        {
            try
            {
                var userId = User.FindFirst("Id")?.Value;

                if (userId == null)
                    return Unauthorized();

                var items = await _userPluginService.GetAllByUserId(userId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorRespones(ex.Message));
            }
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> AddFavoritePlugins(CreateUserPluginDTO createUserPlugin)
        {
            try
            {
                var userId = User.FindFirst("Id")?.Value;

                if (userId == null)
                    return Unauthorized();

                await _userPluginService.AddFavoritePlugin(userId, createUserPlugin.PluginId);
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
                return StatusCode(500, new ErrorRespones(ex.Message));
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveFavoritePlugins(CreateUserPluginDTO createUserPlugin)
        {
            try
            {
                var userId = User.FindFirst("Id")?.Value;

                if (userId == null)
                    return Unauthorized();

                await _userPluginService.RemoveFavoritePlugin(userId, createUserPlugin.PluginId);
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
                return StatusCode(500, new ErrorRespones(ex.Message));
            }
        }
    }
}