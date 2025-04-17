using System.Security.Claims;
using DevTools.Application.Dto;
using DevTools.Application.Exceptions.AccountManager.ChangePassword;
using DevTools.Application.Services.Interfaces;
using DevTools.Dto.user;
using DevTools.Exceptions.AccountManager.LoginException;
using DevTools.Exceptions.AccountManager.RegisterException;
using DevTools.Exceptions.AccountManager.UserException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.data;
using MyWebAPI.Dto;
using MyWebAPI.Dto.user;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.controllers
{
    [Route("api/v1/account")]
    [ApiController]
    public class AccountManagerController : ControllerBase
    {
        private readonly IAccountManagerService _accountManagerService;
        private readonly IPremiumUpgradeRequestService _premiumUpgradeService;
        public AccountManagerController(IAccountManagerService accountServices,
                                        IPremiumUpgradeRequestService premiumUpgradeService)
        {
            _accountManagerService = accountServices;
            _premiumUpgradeService = premiumUpgradeService;
        }

        [HttpPost("login")]
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
            catch (InvalidUsernameOrPassword ex)
            {
                return Unauthorized(new ErrorRespones(ex.Message));
            }

        }

        [HttpPost("register")]
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

        [HttpGet("me")]
        [Authorize()]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            try
            {
                var userIdClaim = User.FindFirst("Id");
                if (userIdClaim == null)
                    return Unauthorized("Please login to use this method");

                var userId = userIdClaim.Value;

                var user = await _accountManagerService.GetUserById(userId);

                return Ok(user);
            }
            catch (UserNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorRespones(ex.Message));
            }
        }

        [HttpPut("me/password-change")]
        [Authorize()]
        public async Task<IActionResult> ChangePassword(PasswordChangeDTO passwordChange)
        {
            try
            {
                var userIdClaim = User.FindFirst("Id");
                if (userIdClaim == null)
                    return Unauthorized(new ErrorRespones("Please login before use this method"));

                var userId = userIdClaim.Value;

                await _accountManagerService.PasswordChange(userId, passwordChange);

                return Ok(new SuccessRespone("Password was changed successfully"));
            }
            catch (UnvalidConfirmPasswordException ex)
            {
                return BadRequest(new ErrorRespones(ex.Message));
            }
            catch (PasswordChangeFailedException ex)
            {
                return BadRequest(new ErrorRespones(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorRespones(ex.Message));
            }
        }

        [HttpPost("me/premium/upgrade-submit")]
        [Authorize]
        public async Task<IActionResult> UpgradePremium()
        {
            try
            {
                var userIdClaim = User.FindFirst("Id");
                if (userIdClaim == null)
                    return Unauthorized(new ErrorRespones("Please login before use this method"));

                var userId = userIdClaim.Value;
                await _premiumUpgradeService.PremiumUpgradeSubmit(userId);
                return Ok(new SuccessRespone("Premium Upgrade submit was sent"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorRespones(ex.Message));
            }

        }

    }
}