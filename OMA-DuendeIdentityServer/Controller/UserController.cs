using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OMA_DuendeIdentityServer.DTO;
using OMA_DuendeIdentityServer.Entity;
using System.Security.Claims;

namespace OMA_DuendeIdentityServer.Controller
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Creates a new user with the specified username and email.
        /// </summary>
        /// <remarks>
        /// This endpoint creates a user and assigns them to the "Hotline-User" role.
        /// </remarks>
        /// <response code="200">Returns OK if the user was created successfully</response>
        /// <response code="400">Returns BadRequest if there was an error creating the user</response>
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(UserDTO userDTO)
        {
            if (string.IsNullOrEmpty(userDTO.Email) || string.IsNullOrEmpty(userDTO.Password))
            {
                return BadRequest("Email and password are required.");
            }

            if (await _userManager.FindByEmailAsync(userDTO.Email) != null)
            {
                return BadRequest("A user with this email already exists.");
            }
            PasswordHasher<User> passwordHasher = new();
            var username = userDTO.Email.Split('@')[0];
            User identityUser = new() { UserName = username, Email = userDTO.Email, PhoneNumber = userDTO.Phone, FullName = userDTO.FullName };
            passwordHasher.HashPassword(identityUser, userDTO.Password);
            var result = await _userManager.CreateAsync(identityUser, userDTO.Password);

            if (!result.Succeeded)
            {
                return BadRequest("Failed to create user.");
            }

            await _userManager.AddToRoleAsync(identityUser, "Hotline-User");
            return Ok(identityUser.Id);
        }


        [HttpGet("claims")]
        public IActionResult GetClaims()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            return Ok(claims);
        }

        [HttpGet("role-check")]
        public IActionResult CheckRole()
        {
            bool Admin = User.IsInRole("Admin");
            return Ok(new { Admin = Admin });
        }
        /// <summary>
        /// Updates a user's information.
        /// </summary>
        /// <remarks>
        /// Updates the specified user's details such as full name, phone, and password if provided.
        /// </remarks>
        /// <response code="200">Returns OK if the user was updated successfully</response>
        /// <response code="400">Returns BadRequest if there was an error updating the user</response>
        /// <response code="404">Returns NotFound if the user does not exist</response>
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserDTO userDTO)
        {
            if (string.IsNullOrEmpty(userDTO.Email))
                return BadRequest("Email is required to identify the user.");

            var identityUser = await _userManager.FindByEmailAsync(userDTO.Email);

            if (identityUser == null)
                return NotFound("User not found.");

            if (!string.IsNullOrEmpty(userDTO.FullName))
                identityUser.UserName = userDTO.FullName;

            if (!string.IsNullOrEmpty(userDTO.Phone))
                identityUser.PhoneNumber = userDTO.Phone;

            if (!string.IsNullOrEmpty(userDTO.Password))
            {
                var passwordHasher = new PasswordHasher<User>();
                identityUser.PasswordHash = passwordHasher.HashPassword(identityUser, userDTO.Password);
            }

            var result = await _userManager.UpdateAsync(identityUser);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to update user.");
            }

            return Ok("User updated successfully.");
        }

        /// <summary>
        /// Toggles the "Admin" role for a specified user. 
        /// </summary>
        /// <remarks>
        /// If the user is already in the "Admin" role, removes them from it.
        /// If the user is not in the "Admin" role, adds them to it.
        /// </remarks>
        /// <param name="id">The ID of the user</param>
        /// <response code="200">Returns OK if the role was toggled successfully</response>
        /// <response code="404">Returns NotFound if the user does not exist</response>
        [HttpPatch("ToggleAdminRole")]
        public async Task<IActionResult> ToggleAdminRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
                return Ok("Admin role removed from the user.");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                return Ok("Admin role added to the user.");
            }
        }

        /// <summary>
        /// Resets a user's password NOT TESTED!!!!
        /// </summary>
        /// <remarks>
        /// This endpoint resets a user's password to a new specified password.
        /// </remarks>
        /// <param name="id">The ID of the user</param>
        /// <param name="newPassword">The new password for the user</param>
        /// <response code="200">Returns OK if the password was reset successfully</response>
        /// <response code="400">Returns BadRequest if there was an error resetting the password</response>
        /// <response code="404">Returns NotFound if the user does not exist</response>
        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string id, string newPassword)
        {
            var identityUser = await _userManager.FindByIdAsync(id);
            if (identityUser == null)
                return NotFound("User not found.");

            await _userManager.RemovePasswordAsync(identityUser);
            var result = await _userManager.AddPasswordAsync(identityUser, newPassword);

            if (!result.Succeeded)
            {
                return BadRequest("Failed to reset password.");
            }

            return Ok("Password reset successfully.");
        }

        /// <summary>
        /// Retrieves a list of all users.
        /// </summary>
        /// <remarks>
        /// Returns a list of users with their IDs, emails, and phone numbers.
        /// </remarks>
        /// <response code="200">Returns a list of users</response>
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = _userManager.Users;
            var userList = new List<UserDTO>();

            foreach (var item in users)
            {
                userList.Add(new UserDTO
                {
                    Id = Guid.Parse(item.Id),
                    Email = item.Email,
                    Phone = item.PhoneNumber,
                    FullName = item.FullName

                });
            }

            return Ok(userList);
        }
    }
}
