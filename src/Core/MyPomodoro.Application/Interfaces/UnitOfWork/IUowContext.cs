
using System.Data;

namespace MyPomodoro.Application.Interfaces.UnitOfWork
{
    public interface IUowContext
    {
        IDbTransaction CurrentTransaction { get; set; }
        IDbConnection GetConnection();
        IUow GetUow();
    }
}