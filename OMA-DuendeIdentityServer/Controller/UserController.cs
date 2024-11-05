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
