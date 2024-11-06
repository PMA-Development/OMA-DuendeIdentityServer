using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace OMA_DuendeIdentityServer.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser()
        {
            IdentityUser identityUser = new() { UserName = "", Email = "" };

            await _userManager.CreateAsync(identityUser, ""); //TODO: make this password
            await _userManager.AddToRoleAsync(identityUser, "Hotline-User");
            return Ok();
        }

        //TODO: What do we update?
        [HttpPatch]
        public async Task<IActionResult> UpdateUser()
        {
            
            return Ok();
        }
        //TODO: WIP
        [HttpPatch]
        public async Task<IActionResult> AddRoleToUser()
        {
           await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync(""), "");

            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> Get()
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
