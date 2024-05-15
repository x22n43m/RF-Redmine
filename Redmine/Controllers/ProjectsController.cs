using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Redmine; 

namespace Redmine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        // Optionally filters projects by type when typeId is provided
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects([FromQuery] int? typeId)
        {
            IQueryable<Project> query = _context.Projects.Include(p => p.ProjectType);

            if (typeId.HasValue && typeId != 0) 
            {
                query = query.Where(p => p.TypeId == typeId);
            }

            return await query.ToListAsync();
        }
        // GET: api/Projects/5/tasks
        [HttpGet("{id}/tasks")]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasksForProject(int id)
        {
            var tasks = await _context.Tasks
                .Where(t => t.ProjectId == id)
                .ToListAsync();

            if (tasks == null)
            {
                return NotFound();
            }

            return tasks;
        }
    }
}
