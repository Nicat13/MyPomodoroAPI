using System;
using MyPomodoro.Domain.Entities;

namespace MyPomodoro.Application.Interfaces.Repositories
{
    public interface IUserConfigurationRepository : IGenericRepository<UserConfiguration>
    {
        public UserConfiguration GetUserConfiguration(string userId);
    }
}