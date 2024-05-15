using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Redmine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redmine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectTypesController> _logger;  

        
        public ProjectTypesController(ApplicationDbContext context, ILogger<ProjectTypesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/ProjectTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectType>>> GetProjectTypes()
        {
            try
            {
                return await _context.ProjectTypes.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to retrieve project types: {Exception}", ex.ToString());
                return StatusCode(500, "Internal Server Error: Unable to retrieve project types.");
            }
        }
    }
}
