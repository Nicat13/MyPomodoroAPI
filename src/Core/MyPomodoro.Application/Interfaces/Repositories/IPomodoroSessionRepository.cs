using System;
using MyPomodoro.Application.DTOs.ViewModels;
using MyPomodoro.Domain.Entities;

namespace MyPomodoro.Application.Interfaces.Repositories
{
    public interface IPomodoroSessionRepository : IGenericRepository<PomodoroSession>
    {
        public PomodoroSessionDetailsViewModel GetActivePomodoroSession(string userId);
    }
}