using System.Collections.Generic;
using System.Threading.Tasks;
using MyPomodoro.Application.DTOs.ViewModels;
using MyPomodoro.Domain.Entities;
using MyPomodoro.Domain.Enums;

namespace MyPomodoro.Application.Interfaces.Repositories
{
    public interface IPomodoroRepository : IGenericRepository<Pomodoro>
    {
        public Task<IEnumerable<PomodoroViewModel>> GetUserPomodoros(string userId);
        public List<PomodoroColors> GetPomodoroColors();
    }
}