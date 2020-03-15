using System.Threading.Tasks;
using Application.ResponseObject;
using Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    public class UserController : BaseController
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserObject>> Login(Login.Query query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserObject>> Register(Register.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet("currentUser")]
        public async Task<ActionResult<UserObject>> GetCurrentUser()
        {
            return await Mediator.Send(new CurrentUser.Query());
        }
    }
}
