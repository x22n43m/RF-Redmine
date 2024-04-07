using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RF_Redmine.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(ILogger<ProjectsController> logger)
        {
            _logger = logger;
        }

    }
}
