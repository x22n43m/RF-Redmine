using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Redmine;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Redmine.Models;
using Redmine.Classes;
using Redmine.Services;
using Microsoft.AspNetCore.SignalR;
using System.Xml;

namespace Redmine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TasksController> _logger;
        private readonly ITaskService _taskService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public TasksController(ApplicationDbContext context, ILogger<TasksController> logger, ITaskService taskService, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _logger = logger;
            _taskService = taskService;
            _hubContext = hubContext;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasks()
        {
            _logger.LogInformation("Fetching tasks. User Claim: {UserClaim}", User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            string managerName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            int managerIdClaim = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "ManagerId")?.Value);
            _logger.LogInformation($"managerName: {managerName}");
            var allTasks = await _context.Tasks.ToListAsync();
            var filteredTasks = allTasks.Where(t => t.ManagerId == managerIdClaim).ToList();
            if (!filteredTasks.Any())
            {
                return NotFound("No tasks found for the given manager.");
            }
            _logger.LogInformation($"Returning {filteredTasks.Count} tasks for manager {managerIdClaim}.");
            return filteredTasks;
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Task>> GetTask(int id)
        {
            _logger.LogInformation($"Task #{id}");
            var managerId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.ManagerId == managerId);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }
            if (!TaskExists(id))
            {
                return NotFound();
            }
            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddTask([FromBody] TaskDto taskDto)
        {
            var managerId = User.FindFirstValue("ManagerId");

            if (managerId == null)
            {
                return Unauthorized(new { message = "Manager ID not found in token" });
            }

            var project = await _context.Projects.FindAsync(taskDto.ProjectId);
            var developer = await _context.Developers.FindAsync(taskDto.DeveloperId);

            if (project == null)
            {
                return NotFound(new { message = "Project not found" });
            }

            if (developer == null)
            {
                return NotFound(new { message = "Developer not found" });
            }

            var task = new Task
            {
                Name = taskDto.Name,
                Description = taskDto.Description,
                ProjectId = taskDto.ProjectId,
                ManagerId = int.Parse(managerId),
                Deadline = taskDto.Deadline
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var developerTask = new DeveloperTask
            {
                DeveloperId = taskDto.DeveloperId,
                TaskId = task.Id
            };

            _context.DeveloperTasks.Add(developerTask);
            await _context.SaveChangesAsync();

            var taskForClient = new TaskDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Deadline = task.Deadline,
                ProjectId = task.ProjectId
            };

            await _hubContext.Clients.All.SendAsync("NotifyTasksApproachingDeadline", new List<TaskDto> { taskForClient });

            return Ok(new { message = "Task added successfully" });
        }


        [HttpGet("approaching-deadline")]
        public async Task<ActionResult<IEnumerable<TaskDto2>>> GetTasksApproachingDeadline([FromQuery] int days)
        {
            try
            {
                int managerId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "ManagerId")?.Value);

                var manager = await _context.Managers.FindAsync(managerId);
                if (manager == null)
                {
                    return Unauthorized("Manager not found");
                }

                TimeSpan timeSpan = TimeSpan.FromDays(days);
                var tasks = _taskService.GetTasksApproachingDeadline(manager.Id.ToString(), timeSpan);

                var taskDtos = tasks.Select(task => new TaskDto2
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    Deadline = task.Deadline,
                    ProjectId = task.ProjectId
                }).ToList();

                return Ok(taskDtos); // Wrap the list in an Ok result
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching tasks approaching deadline");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching tasks approaching deadline");
            }
        }

        [HttpPost("send-reminders")]
        public async Task<IActionResult> SendTaskReminders()
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (managerId == null)
            {
                return Unauthorized();
            }

            await _taskService.SendTaskReminders(managerId);
            return Ok(new { message = "Task reminders sent successfully" });
        }

        private bool TaskExists(int id)
        {
            var managerId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            return _context.Tasks.Any(e => e.Id == id && e.ManagerId == managerId);
        }
    }
}
