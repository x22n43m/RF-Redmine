using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RF_Redmine.Classes;
using RF_Redmine.Classes.Db_Classes;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

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

        [HttpGet]
        public IActionResult Get()
        {
            List<Projects> projektek = Statics<Projects>.ListTypeGetter();
            Dictionary<int, object> jsons = new Dictionary<int, object>();
            for (int i = 0; i < projektek.Count; i++)
            {
                jsons[i] = projektek[i].ToJson.Value;
                _logger.LogInformation(projektek[i].ToJson.Value+"");
            }
            return Ok(jsons);
        }
    }
}
