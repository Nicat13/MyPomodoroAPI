using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using MyPomodoro.Application.Interfaces.UnitOfWork;
using MyPomodoro.Infrastructure.Persistence.Contexts;
using MyPomodoro.Infrastructure.Persistence.Settings;

namespace MyPomodoro.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWorkContext : IUowContext
    {
        readonly IDbConnection sqlConnection;
        readonly Uow _uow;
        public UnitOfWorkContext(IdentityContext context, IOptions<APIAppSettings> apiSettings)
        {
            sqlConnection = context.Database.GetDbConnection();
            sqlConnection.Open();
            _uow = new Uow(this, context, apiSettings);
        }

        public IDbContextTransaction CurrentTransaction { get; set; }
        public IDbConnection GetConnection()
        {
            return sqlConnection;
        }

        public IUow GetUow()
        {
            return _uow;
        }
    }
}