using System;
using Dapper;
using MyPomodoro.Application.DTOs.ViewModels;
using MyPomodoro.Application.Interfaces.Repositories;
using MyPomodoro.Domain.Entities;
using MyPomodoro.Infrastructure.Persistence.Contexts;
using MyPomodoro.Infrastructure.Persistence.Dapper;

namespace MyPomodoro.Infrastructure.Persistence.Repositories
{
    public class PomodoroSessionRepository : GenericRepository<PomodoroSession>, IPomodoroSessionRepository
    {
        IDapper dapper;
        IdentityContext _context;
        public PomodoroSessionRepository(IDapper dapper, IdentityContext dbContext) : base(dbContext)
        {
            this.dapper = dapper;
            this._context = dbContext;
        }

        public PomodoroSessionDetailsViewModel GetActivePomodoroSession(string userId)
        {
            string sql = @"SELECT P.Name,P.PomodoroTime,P.LongBreakTime,P.ShortBreakTime,P.LongBreakInterval,P.PeriodCount, PS.CurrentStatus FROM PomodoroSessions PS
                            LEFT JOIN Pomodoros P ON PS.PomodoroId = P.Id
                            WHERE PS.UserId = @USER_ID AND PS.IsActive = 1 AND P.IsDeleted = 0";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("USER_ID", userId);
            return dapper.Get<PomodoroSessionDetailsViewModel>(sql, parameters);
        }
    }
}