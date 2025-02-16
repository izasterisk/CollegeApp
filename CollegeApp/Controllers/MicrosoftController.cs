using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors(PolicyName = "AllowOnlyGoogle")]
    [Authorize(AuthenticationSchemes = "LoginforGoogleuser", Roles = "Superadmin, Admin")]
    public class MicrosoftController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Hello from Microsoft Controller");
        }
    }
}
