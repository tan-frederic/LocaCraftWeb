using LocaCraftAPI.LocaCraftAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LocaCraftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        private string? GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var user = await _userManager.FindByIdAsync(GetUserId()!);
            if (user is null) return NotFound();
            return Ok(new { user.Email, user.ProfilePictureBase64 });
        }

        [HttpPatch("email")]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailRequest request)
        {
            var user = await _userManager.FindByIdAsync(GetUserId()!);
            if (user is null) return NotFound();

            var result = await _userManager.SetEmailAsync(user, request.NewEmail);
            if (!result.Succeeded) return BadRequest(result.Errors);

            var usernameResult = await _userManager.SetUserNameAsync(user, request.NewEmail);
            if (!usernameResult.Succeeded) return BadRequest(usernameResult.Errors);

            return Ok();
        }

        [HttpPatch("password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(GetUserId()!);
            if (user is null) return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok();
        }

        [HttpPatch("profile-picture")]
        public async Task<IActionResult> UpdateProfilePicture([FromBody] UpdateProfilePictureRequest request)
        {
            var user = await _userManager.FindByIdAsync(GetUserId()!);
            if (user is null) return NotFound();

            user.ProfilePictureBase64 = request.ProfilePictureBase64;
            await _userManager.UpdateAsync(user);
            return Ok();
        }

        public record ChangeEmailRequest(string NewEmail);
        public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
        public record UpdateProfilePictureRequest(string ProfilePictureBase64);
    }
}
