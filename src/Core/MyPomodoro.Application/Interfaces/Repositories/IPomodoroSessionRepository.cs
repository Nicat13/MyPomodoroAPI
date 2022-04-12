using System;
using System.Threading.Tasks;
using MyPomodoro.Application.DTOs.ViewModels;
using MyPomodoro.Domain.Entities;

namespace MyPomodoro.Application.Interfaces.Repositories
{
    public interface IPomodoroSessionRepository : IGenericRepository<PomodoroSession>
    {
        public PomodoroSessionDetailsViewModel GetActivePomodoroSession(string userId);
        public JoinedPomodoroSessionDetailsViewModel GetJoinedActivePomodoroSessionDetails(string userId);
        public JoinedPomodoroSessionViewModel GetJoinedActivePomodoroSession(string userId);
        public Task<PomodoroSession> GetSessionBySessionShareCode(string sessionShareCode);
        public LatestJoinedSessionViewModel GetLatestJoinedSession(string userId);
    }
}