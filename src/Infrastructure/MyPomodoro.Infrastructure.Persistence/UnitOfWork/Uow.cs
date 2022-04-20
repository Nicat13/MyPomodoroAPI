using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MyPomodoro.Application.Interfaces.Repositories;
using MyPomodoro.Application.Interfaces.UnitOfWork;
using MyPomodoro.Infrastructure.Persistence.Contexts;
using MyPomodoro.Infrastructure.Persistence.Dapper;
using MyPomodoro.Infrastructure.Persistence.Repositories;

namespace MyPomodoro.Infrastructure.Persistence.UnitOfWork
{
    public class Uow : IUow
    {
        private IDbConnection _sqlConnection;
        private IDbContextTransaction _sqlTransaction;
        private IdentityContext _context;
        readonly ITestRepo _testrepo;
        readonly IPomodoroRepository _pomodoroRepository;
        readonly IUserConfigurationRepository _userConfigurationRepository;
        readonly IPomodoroSessionRepository _pomodoroSessionRepository;
        readonly ISessionParticipiantRepository _sessionParticipiantRepository;
        readonly ITaskRepository _taskRepository;
        public Uow(IUowContext connectionContext, IdentityContext context)
        {
            _context = context;
            ConnContext = connectionContext;
            _sqlConnection = connectionContext.GetConnection();
            var dapper = new DapperClass(ConnContext);
            _testrepo = new TestRepo(dapper, context);
            _pomodoroRepository = new PomodoroRepository(dapper, context);
            _userConfigurationRepository = new UserConfigurationRepository(dapper, context);
            _pomodoroSessionRepository = new PomodoroSessionRepository(dapper, context);
            _sessionParticipiantRepository = new SessionParticipiantRepository(dapper, context);
            _taskRepository = new TaskRepository(dapper, context);
        }
        public IPomodoroRepository PomodoroRepository => _pomodoroRepository;
        public IUserConfigurationRepository UserConfigurationRepository => _userConfigurationRepository;
        public IPomodoroSessionRepository PomodoroSessionRepository => _pomodoroSessionRepository;
        public ISessionParticipiantRepository SessionParticipiantRepository => _sessionParticipiantRepository;
        public ITaskRepository TaskRepository => _taskRepository;
        public ITestRepo TestRepo => _testrepo;

        public IUowContext ConnContext { get; set; }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public IDbContextTransaction BeginTransaction()
        {
            _sqlTransaction = _context.Database.BeginTransaction();
            ConnContext.CurrentTransaction = _sqlTransaction;
            return _sqlTransaction;
        }
        public void Commit()
        {
            _sqlTransaction.Commit();
        }

        public void Dispose()
        {
            _sqlConnection.Dispose();
            _context.Dispose();
        }

        public void RollBack()
        {
            _sqlTransaction.Rollback();
        }

        public void Close()
        {
            _sqlConnection.Close();
        }
    }
}