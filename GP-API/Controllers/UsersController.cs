namespace GP_API.Controllers
{
    using GP_API.Models.Request;
    using GP_API.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public UsersController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginRequest loginRequest)
        {
            return Ok(new { data = _authenticationService.Login(loginRequest) });
        }

        [HttpPost("signup")]
        public IActionResult Signup([FromBody]UserRequest userRequest)  
        {
            return Ok(new {data = _authenticationService.SignUp(userRequest)});
        }
    }
}
