using System;
using System.Data;
using Microsoft.EntityFrameworkCore;
using MyPomodoro.Application.Interfaces.UnitOfWork;
using MyPomodoro.Infrastructure.Persistence.Contexts;

namespace MyPomodoro.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWorkContext : IUowContext
    {
        readonly IDbConnection sqlConnection;
        readonly Uow _uow;

        public UnitOfWorkContext(IdentityContext context)
        {
            sqlConnection = context.Database.GetDbConnection();
            sqlConnection.Open();
            _uow = new Uow(this, context);
        }

        public IDbTransaction CurrentTransaction { get; set; }
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