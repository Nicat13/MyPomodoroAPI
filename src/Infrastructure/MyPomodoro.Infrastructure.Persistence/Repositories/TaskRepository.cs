using System;
using MyPomodoro.Application.Interfaces.Repositories;
using MyPomodoro.Domain.Entities;
using MyPomodoro.Infrastructure.Persistence.Contexts;
using MyPomodoro.Infrastructure.Persistence.Dapper;

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
    }
}