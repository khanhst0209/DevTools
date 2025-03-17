using DevTools.Dto.user;
using DevTools.Exceptions.AccountManager.LoginException;
using DevTools.Exceptions.AccountManager.RegisterException;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.data;
using MyWebAPI.Dto;
using MyWebAPI.Dto.user;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.controllers
{
    [Route("Account")]
    [ApiController]
    public class AccountManagerController : ControllerBase
    {
        private readonly IAccountManagerService _accountManagerService;
        public AccountManagerController(IAccountManagerService _Accountservices)
        {
            this._accountManagerService = _Accountservices;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO logindto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorRespones(ModelState.ToString()));
            }
            try
            {
                var user = await _accountManagerService.Login(logindto);
                if (user == null)
                {
                    return Unauthorized("Invalid Username or Password");
                }

                return Ok(user);
            }
            catch(InvalidUsernameOrPassword ex)
            {
                return Unauthorized(new ErrorRespones(ex.Message));
            }

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorRespones(registerDto.ToString()));
            }
            try
            {
                var userDto = await _accountManagerService.Register(registerDto);
                return Ok(userDto);
            }
            catch (UserCreationFailedException ex)
            {
                return BadRequest(new ErrorRespones(ex.Message));
            }
            catch (RoleAssignmentFailedException ex)
            {
                return StatusCode(500, new ErrorRespones(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorRespones("An unexpected error occurred: " + ex.Message));
            }
        }

    }
}