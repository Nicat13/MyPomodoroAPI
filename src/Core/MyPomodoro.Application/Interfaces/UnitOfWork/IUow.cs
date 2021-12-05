using System;
using System.Data;
using System.Threading.Tasks;
using MyPomodoro.Application.Interfaces.Repositories;

namespace MyPomodoro.Application.Interfaces.UnitOfWork
{
    public interface IUow : IDisposable
    {
        ITestRepo TestRepo { get; }
        IDbTransaction BeginTransaction();
        void Commit();
        void RollBack();
        void Close();
        Task SaveChangesAsync();
    }
}