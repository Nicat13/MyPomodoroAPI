using System;
using Dapper;
using MyPomodoro.Application.Interfaces.Repositories;
using MyPomodoro.Domain.Entities;
using MyPomodoro.Infrastructure.Persistence.Contexts;
using MyPomodoro.Infrastructure.Persistence.Dapper;

namespace MyPomodoro.Infrastructure.Persistence.Repositories
{
    public class UserConfigurationRepository : GenericRepository<UserConfiguration>, IUserConfigurationRepository
    {
        IDapper dapper;
        IdentityContext _context;
        public UserConfigurationRepository(IDapper dapper, IdentityContext dbContext) : base(dbContext)
        {
            this.dapper = dapper;
            this._context = dbContext;
        }
        public UserConfiguration GetUserConfiguration(string userId)
        {
            string sql = "SELECT * FROM UserConfigurations WHERE UserId=@USER_ID";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("USER_ID", userId);
            return dapper.Get<UserConfiguration>(sql, parameters);
        }
    }
}