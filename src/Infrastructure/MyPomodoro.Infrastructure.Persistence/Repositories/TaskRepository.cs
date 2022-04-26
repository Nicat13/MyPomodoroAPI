using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MyPomodoro.Application.DTOs.ViewModels;
using MyPomodoro.Application.Interfaces.Repositories;
using MyPomodoro.Domain.Entities;
using MyPomodoro.Infrastructure.Persistence.Contexts;
using MyPomodoro.Infrastructure.Persistence.Dapper;
using ThreadTasks = System.Threading.Tasks;

namespace MyPomodoro.Infrastructure.Persistence.Repositories
{
    public class TaskRepository : GenericRepository<Task>, ITaskRepository
    {
        IDapper dapper;
        IdentityContext _context;
        public TaskRepository(IDapper dapper, IdentityContext dbContext) : base(dbContext)
        {
            this.dapper = dapper;
            this._context = dbContext;
        }

        public async ThreadTasks.Task<IEnumerable<CreatedTaskViewModel>> GetSessionTasks(int sessionId, string userId)
        {
            string sql = @"SELECT Id, IsDone, Name, Description ,EstimatePomodoros
                           FROM Tasks WHERE UserId=@USER_ID AND PomodoroSessionId=@SESSID";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("USER_ID", userId);
            parameters.Add("SESSID", sessionId);
            return await dapper.GetAllAsync<CreatedTaskViewModel>(sql, parameters);
        }

        public async ThreadTasks.Task<Task> GetTaskByIdAndUserId(int id, string userId)
        {
            return await _context.Tasks.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);
        }
    }
}