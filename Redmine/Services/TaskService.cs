using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Redmine;
using Redmine.Models;
using Redmine;

namespace Redmine.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public TaskService(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public List<Task> GetTasksApproachingDeadline(string managerId, TimeSpan timeSpan)
        {
            DateTime now = DateTime.Now;
            DateTime threshold = now.Add(timeSpan);
            var tasks = _context.Tasks
                                .Where(t => t.ManagerId == int.Parse(managerId) && t.Deadline <= threshold && t.Deadline > now)
                                .ToList();

            return tasks;
        }

        public async System.Threading.Tasks.Task SendTaskReminders(string managerId)
        {
            var tasks = GetTasksApproachingDeadline(managerId, TimeSpan.FromDays(14)); 
            if (tasks.Any())
            {
                await _hubContext.Clients.User(managerId).SendAsync("NotifyTasksApproachingDeadline", tasks);
            }
        }
    }
}
