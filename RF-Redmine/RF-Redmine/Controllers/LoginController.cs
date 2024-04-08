﻿using Microsoft.AspNetCore.Http;
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

        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        public IActionResult Login()
        {
            Login LoginAttempt = new Login(Request.Form);
            if (LoginAttempt.LoginState == Enums.ELoginState.Valid_Credentials)
            {
                return Redirect("/projects.html");
            }
            return NotFound();
        }
    }
}
