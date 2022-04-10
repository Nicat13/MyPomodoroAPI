using System;
using MyPomodoro.Application.Interfaces.Repositories;
using MyPomodoro.Domain.Entities;
using MyPomodoro.Infrastructure.Persistence.Contexts;
using MyPomodoro.Infrastructure.Persistence.Dapper;

namespace MyPomodoro.Infrastructure.Persistence.Repositories
{
    public class SessionParticipiantRepository : GenericRepository<SessionParticipiant>, ISessionParticipiantRepository
    {
        IDapper dapper;
        IdentityContext _context;
        public SessionParticipiantRepository(IDapper dapper, IdentityContext dbContext) : base(dbContext)
        {
            this.dapper = dapper;
            this._context = dbContext;
        }
    }
}