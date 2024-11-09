using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OMA_DuendeIdentityServer.Controller
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "HotlineUserOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(UserManager<IdentityUser> userManager)
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
        public async Task<IActionResult> CreateUser(string username, string email, string password)
        {
            //TODO: Add validation for username, email, and password
            //TODO: Make UserDTO class to handle user data
            if (await _userManager.FindByNameAsync(username) != null || await _userManager.FindByEmailAsync(email) != null)
            {
                return BadRequest("A user with this username or email already exists.");
            }

            IdentityUser identityUser = new() { UserName = username, Email = email };
            var result = await _userManager.CreateAsync(identityUser, password);

            if (!result.Succeeded)
            {
                return BadRequest("Failed to create user.");
            }

            await _userManager.AddToRoleAsync(identityUser, "Hotline-User");
            return Ok("User created successfully.");
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
            bool isHotlineUser = User.IsInRole("Hotline-User");
            return Ok(new { IsHotlineUser = isHotlineUser });
        }

        /// <summary>
        /// Updates a user's information.
        /// </summary>
        [HttpPatch("UpdateUser")]
        public async Task<IActionResult> UpdateUser()
        {
            return Ok();
        }

        /// <summary>
        /// Adds a specified role to a user.
        /// </summary>
        [HttpPatch("AddRoleToUser")]
        public async Task<IActionResult> AddRoleToUser()
        {
            await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync(""), "");
            return Ok();
        }

        /// <summary>
        /// Resets a user's password.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> ResetPassword()
        {
            IdentityUser identityUser = await _userManager.FindByIdAsync("");
            await _userManager.RemovePasswordAsync(identityUser);
            await _userManager.AddPasswordAsync(identityUser, "");
            return Ok();
        }




        /// <summary>
        /// Retrieves a list of all users.
        /// </summary>
        /// <returns>A list of users with their IDs, usernames, and emails</returns>


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = _userManager.Users;
            var userList = new List<IdentityUser>();

            foreach (var item in users)
            {
                userList.Add(new IdentityUser
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    Email = item.Email
                });
            }

            return Ok(userList);
        }
    }
}
