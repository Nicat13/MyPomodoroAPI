using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MyPomodoro.Application.DTOs.ViewModels;
using MyPomodoro.Application.Interfaces.Repositories;
using MyPomodoro.Domain.Entities;
using MyPomodoro.Domain.Enums;
using MyPomodoro.Infrastructure.Persistence.Contexts;
using MyPomodoro.Infrastructure.Persistence.Dapper;
using Task = System.Threading.Tasks.Task;

namespace MyPomodoro.Infrastructure.Persistence.Repositories
{
    public class PomodoroRepository : GenericRepository<Pomodoro>, IPomodoroRepository
    {
        IDapper dapper;
        IdentityContext _context;
        public PomodoroRepository(IDapper dapper, IdentityContext dbContext) : base(dbContext)
        {
            this.dapper = dapper;
            this._context = dbContext;
        }

        public List<PomodoroColors> GetPomodoroColors()
        {
            return PomodoroColors.Colors;
        }

        public PomodoroDetailsViewModel GetPomodoroDetails(string userId, int pomodoroId)
        {
            string sql = @"SELECT Id,Name, PomodoroTime, ShortBreakTime, LongBreakTime, LongBreakInterval, PeriodCount, Color 
                         FROM Pomodoros WHERE UserId = @USER_ID AND IsDeleted = 0 AND Id = @POMODORO_ID";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("USER_ID", userId);
            parameters.Add("POMODORO_ID", pomodoroId);
            return dapper.Get<PomodoroDetailsViewModel>(sql, parameters);
        }

        public async Task<IEnumerable<PomodoroViewModel>> GetUserPomodoros(string userId)
        {
            string sql = "SELECT Id,Name,PomodoroTime,Color FROM Pomodoros WHERE UserId=@USER_ID AND IsDeleted=0 ORDER by CreateDate DESC";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("USER_ID", userId);
            var UserPomodoros = await dapper.GetAllAsync<PomodoroViewModel>(sql, parameters);
            PomodoroColors defaultcolor = PomodoroColors.Colors.First();
            foreach (var pomodoro in UserPomodoros)
            {
                PomodoroColors color = PomodoroColors.Colors.FirstOrDefault(x => x.Id == pomodoro.Color);
                if (color != null)
                {
                    pomodoro.BgColor = color.BgColor;
                    pomodoro.TxtColor = color.TxtColor;
                }
                else
                {
                    pomodoro.BgColor = defaultcolor.BgColor;
                    pomodoro.TxtColor = defaultcolor.TxtColor;
                }
            }
            return await Task.FromResult(UserPomodoros);
        }
    }
}