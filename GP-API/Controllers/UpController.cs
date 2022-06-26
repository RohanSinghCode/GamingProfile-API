namespace GP_API.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("[controller]")]
    [ApiController]
    public class UpController : ControllerBase
    {
        [HttpGet]
        public IActionResult UpStatus()
        {
            return Ok(new { data = "Gaming Profile is up and ready to go !!" });
        }
    }
}
