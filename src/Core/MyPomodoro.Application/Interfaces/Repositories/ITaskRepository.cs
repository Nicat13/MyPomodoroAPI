using System;
using System.Collections.Generic;
using MyPomodoro.Application.DTOs.ViewModels;
using MyPomodoro.Domain.Entities;
using ThreadTask = System.Threading.Tasks;

namespace MyPomodoro.Application.Interfaces.Repositories
{
    public interface ITaskRepository : IGenericRepository<Task>
    {
        public ThreadTask.Task<IEnumerable<CreatedTaskViewModel>> GetSessionTasks(int sessionId, string userId);
        public ThreadTask.Task<Task> GetTaskByIdAndUserId(int id, string userId);
    }
}