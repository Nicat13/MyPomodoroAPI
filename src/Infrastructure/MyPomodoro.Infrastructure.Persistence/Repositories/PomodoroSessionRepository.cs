using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MyPomodoro.Application.DTOs.ViewModels;
using MyPomodoro.Application.Interfaces.Repositories;
using MyPomodoro.Domain.Entities;
using MyPomodoro.Infrastructure.Persistence.Contexts;
using MyPomodoro.Infrastructure.Persistence.Dapper;
using Task = System.Threading.Tasks.Task;

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
            string sql = @"SELECT PS.Id,P.Name,P.PomodoroTime,P.LongBreakTime,P.ShortBreakTime,P.LongBreakInterval,P.PeriodCount, PS.CurrentStatus, PS.CurrentStep, PS.SessionShareCode, PS.CurrentTime FROM PomodoroSessions PS
                            LEFT JOIN Pomodoros P ON PS.PomodoroId = P.Id
                            WHERE PS.UserId = @USER_ID AND PS.IsActive = 1 AND P.IsDeleted = 0";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("USER_ID", userId);
            return dapper.Get<PomodoroSessionDetailsViewModel>(sql, parameters);
        }

        public JoinedPomodoroSessionViewModel GetJoinedActivePomodoroSession(string userId)
        {
            string sql = @"SELECT PS.SessionShareCode, U.FirstName as UserName, P.Name as PomodoroName FROM PomodoroSessions PS
                            LEFT JOIN Users U ON PS.UserId=U.Id
                            LEFT JOIN Pomodoros P ON P.Id=PS.PomodoroId
                            WHERE PS.Id=(SELECT TOP 1 SessionId
                            FROM SessionParticipiants
                            WHERE UserId=@USER_ID
                            ORDER BY Id DESC) AND PS.IsActive=1 AND P.IsDeleted=0";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("USER_ID", userId);
            return dapper.Get<JoinedPomodoroSessionViewModel>(sql, parameters);
        }

        public JoinedPomodoroSessionDetailsViewModel GetJoinedActivePomodoroSessionDetails(string userId)
        {
            string sql = @"SELECT PS.SessionShareCode, U.FirstName as UserName, P.Name as PomodoroName, P.PomodoroTime,
                            P.ShortBreakTime,P.LongBreakTime,P.LongBreakInterval,P.PeriodCount,PS.CurrentTime,PS.CurrentStatus,PS.CurrentStep
                            FROM PomodoroSessions PS
                            LEFT JOIN Users U ON PS.UserId=U.Id
                            LEFT JOIN Pomodoros P ON P.Id=PS.PomodoroId
                            WHERE PS.Id=(SELECT TOP 1 SessionId
                            FROM SessionParticipiants
                            WHERE UserId=@USER_ID
                            ORDER BY Id DESC) AND PS.IsActive=1 AND P.IsDeleted=0";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("USER_ID", userId);
            return dapper.Get<JoinedPomodoroSessionDetailsViewModel>(sql, parameters);
        }

        public async Task<PomodoroSession> GetSessionBySessionShareCode(string sessionShareCode)
        {
            return await _context.PomodoroSessions.FirstOrDefaultAsync(x => x.SessionShareCode == sessionShareCode);
        }
    }
}