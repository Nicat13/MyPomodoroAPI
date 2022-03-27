using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using MyPomodoro.Application.Interfaces.Repositories;

namespace MyPomodoro.Application.Interfaces.UnitOfWork
{
    public interface IUow : IDisposable
    {
        ITestRepo TestRepo { get; }
        IPomodoroRepository PomodoroRepository { get; }
        IUserConfigurationRepository UserConfigurationRepository { get; }
        IDbContextTransaction BeginTransaction();
        void Commit();
        void RollBack();
        void Close();
        Task SaveChangesAsync();
    }
}