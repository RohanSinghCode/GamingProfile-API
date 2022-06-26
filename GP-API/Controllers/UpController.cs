namespace GP_API.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("up")]
    [Controller]
    public class UpController : Controller
    {
        [HttpGet]
        public IActionResult UpStatus()
        {
            return Ok(new { data = "Gaming Profile is up and ready to go !!" });
        }
    }
}
