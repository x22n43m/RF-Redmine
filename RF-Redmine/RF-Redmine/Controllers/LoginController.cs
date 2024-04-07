using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RF_Redmine.Models;
using static System.Data.Entity.Infrastructure.Design.Executor;
using System.Net;
using System.Web;

namespace RF_Redmine.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;

        public LoginController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        public IActionResult Login()
        {
            Login LoginAttempt = new Login(Request.Form);
            return LoginAttempt.LoginState == Enums.ELoginState.Valid_Credentials ? Ok(new { Message = "valid" }) : Ok(new { Message = "bruh" });
        }

        //public ContentResult 
    }
}
