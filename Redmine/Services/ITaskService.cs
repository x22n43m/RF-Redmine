using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Redmine.Models;

namespace Redmine.Services
{
    public interface ITaskService
    {
        List<Task> GetTasksApproachingDeadline(string managerId, TimeSpan timeSpan);
        System.Threading.Tasks.Task SendTaskReminders(string managerId);
    }
}
