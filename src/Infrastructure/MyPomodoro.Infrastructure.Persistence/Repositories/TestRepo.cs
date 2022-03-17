using MyPomodoro.Application.Interfaces.Repositories;
using MyPomodoro.Domain.Entities;
using MyPomodoro.Infrastructure.Persistence.Contexts;
using MyPomodoro.Infrastructure.Persistence.Dapper;

namespace MyPomodoro.Infrastructure.Persistence.Repositories
{
    public class TestRepo : ITestRepo
    {
        IDapper dapper;
        IdentityContext _context;
        public TestRepo(IDapper dapper, IdentityContext _context)
        {
            this.dapper = dapper;
            this._context = _context;
        }
        public void addDepart()
        {
            UserConfiguration model = new UserConfiguration
            {
                AutoStartBreaks = false,
                AutoStartPomodoros = true,
                PushNotification = true,
                UserId = "327b6e67-6193-4174-8410-9d2bd999eca8"
            };
            _context.UserConfigurations.Add(model);
        }
        public void addDepart2()
        {
            var addPomodoroQuery = $"INSERT INTO Pomodoros(PomodoroTime,ShortBreakTime,LongBreakTime,PeriodCount,Color,CreateDate,UserId) VALUES(25,5,20,2,1,GETDATE(),'327b6e67-6193-4174-8410-9d2bd999eca8');SELECT CAST(SCOPE_IDENTITY() as int)";
            var departmentId = dapper.Execute(addPomodoroQuery, null);
        }
    }
}