
using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace MyPomodoro.Application.Interfaces.UnitOfWork
{
    public interface IUowContext
    {
        IDbContextTransaction CurrentTransaction { get; set; }
        IDbConnection GetConnection();
        IUow GetUow();
    }
}