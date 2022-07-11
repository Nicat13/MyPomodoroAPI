using System;
using System.Collections.Generic;
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
            string sql = @"SELECT PS.Id,P.Name,P.PomodoroTime,P.LongBreakTime,P.ShortBreakTime,P.LongBreakInterval,P.PeriodCount, PS.CurrentStatus, PS.CurrentStep, PS.SessionShareCode, PS.CurrentTime, PS.StatusChangeTime FROM PomodoroSessions PS
                            LEFT JOIN Pomodoros P ON PS.PomodoroId = P.Id
                            WHERE PS.UserId = @USER_ID AND PS.IsActive = 1 AND P.IsDeleted = 0";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("USER_ID", userId);
            return dapper.Get<PomodoroSessionDetailsViewModel>(sql, parameters);
        }

        public JoinedPomodoroSessionViewModel GetJoinedActivePomodoroSession(string userId)
        {
            string sql = @"SELECT PS.Id ,PS.SessionShareCode, U.UserName as UserName, P.Name as PomodoroName FROM PomodoroSessions PS
                            LEFT JOIN Users U ON PS.UserId=U.Id
                            LEFT JOIN Pomodoros P ON P.Id=PS.PomodoroId
                            WHERE PS.Id=(SELECT TOP 1 SessionId
                            FROM SessionParticipiants
                            WHERE UserId=@USER_ID AND IsJoined=1
                            ORDER BY Id DESC) AND PS.IsActive=1 AND P.IsDeleted=0";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("USER_ID", userId);
            return dapper.Get<JoinedPomodoroSessionViewModel>(sql, parameters);
        }

        public JoinedPomodoroSessionDetailsViewModel GetJoinedActivePomodoroSessionDetails(string userId)
        {
            string sql = @"SELECT PS.SessionShareCode, U.UserName as UserName, P.Name as PomodoroName, P.PomodoroTime,
                            P.ShortBreakTime,P.LongBreakTime,P.LongBreakInterval,P.PeriodCount,PS.CurrentTime,PS.CurrentStatus,PS.CurrentStep, PS.StatusChangeTime
                            FROM PomodoroSessions PS
                            LEFT JOIN Users U ON PS.UserId=U.Id
                            LEFT JOIN Pomodoros P ON P.Id=PS.PomodoroId
                            WHERE PS.Id=(SELECT TOP 1 SessionId
                            FROM SessionParticipiants
                            WHERE UserId=@USER_ID AND IsJoined=1
                            ORDER BY Id DESC) AND PS.IsActive=1 AND P.IsDeleted=0";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("USER_ID", userId);
            return dapper.Get<JoinedPomodoroSessionDetailsViewModel>(sql, parameters);
        }

        public LatestJoinedSessionViewModel GetLatestJoinedSession(string userId)
        {
            string sql = @" SELECT TOP 1 SP.Id,SP.SessionId,SP.IsJoined
                            FROM SessionParticipiants SP
                            LEFT JOIN PomodoroSessions PS ON SP.SessionId=PS.Id
                            LEFT JOIN Pomodoros P ON PS.PomodoroId=P.Id
                            WHERE SP.UserId=@USER_ID AND PS.IsActive=1 AND P.IsDeleted=0
                            ORDER BY Id DESC";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("USER_ID", userId);
            return dapper.Get<LatestJoinedSessionViewModel>(sql, parameters);
        }

        public async Task<PomodoroSession> GetSessionBySessionShareCode(string sessionShareCode)
        {
            return await _context.PomodoroSessions.FirstOrDefaultAsync(x => x.SessionShareCode == sessionShareCode && x.IsActive == true);
        }

        public List<SessionLobbyViewModel> GetSessionLobbies(string userId)
        {
            string sql = @"SELECT U.UserName,P.Name as PomodoroName,PS.SessionShareCode,CASE WHEN [Password] IS NULL THEN 0 ELSE 1 END 'HasPass' FROM PomodoroSessions PS
                            LEFT JOIN Pomodoros P ON PS.PomodoroId=P.Id
                            LEFT JOIN Users U ON PS.UserId=U.Id
                            WHERE PS.SessionType=1 AND PS.IsActive=1 and PS.UserId<>@USER_ID";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("USER_ID", userId);
            return dapper.GetAll<SessionLobbyViewModel>(sql, parameters).ToList();
        }

        public List<SessionParticipiantViewModel> GetSessionParticipiants(int sessionId)
        {
            string sql = @"SELECT DISTINCT U.UserName FROM SessionParticipiants SP
                            LEFT JOIN PomodoroSessions PS ON SP.SessionId=PS.Id
                            LEFT JOIN Users U ON SP.UserId=U.Id OR PS.UserId=U.Id
                            WHERE SP.SessionId=@SESSION_ID AND SP.IsJoined=1";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("SESSION_ID", sessionId);
            return dapper.GetAll<SessionParticipiantViewModel>(sql, parameters).ToList();
        }
    }
}