using DevTools.Dto.user;
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
            Console.WriteLine("check -2");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Console.WriteLine("check -1");
            var user = _accountManagerService.Login(logindto);
            if (user == null)
            {
                return Unauthorized("Invalid Username or Password");
            }

            return Ok(user);

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(registerDto);
            }
            try
            {
                var userDto = await _accountManagerService.Register(registerDto);
                return Ok(userDto);
            }
            catch (UserCreationFailedException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (RoleAssignmentFailedException ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An unexpected error occurred: " + ex.Message });
            }
        }

    }
}