using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MyPomodoro.Application.Interfaces.UnitOfWork;

namespace MyPomodoro.Infrastructure.Persistence.Dapper
{
    public class DapperClass : IDapper
    {
        IUowContext connectionContext;
        public IDbTransaction CurrentTransaction => (connectionContext.CurrentTransaction as IInfrastructure<DbTransaction>)?.Instance;

        public DapperClass(IUowContext connectionContext)
        {
            this.connectionContext = connectionContext;
        }

        public int Execute(string sql, DynamicParameters parameters, CommandType commandType = CommandType.Text)
        {
            var cnn = GetConnection();
            return cnn.ExecuteScalar<int>(sql, parameters, commandType: commandType, transaction: CurrentTransaction);
        }

        public T Get<T>(string sql, DynamicParameters parameters = null, CommandType commandType = CommandType.Text)
        {
            var cnn = GetConnection();
            return cnn.QueryFirstOrDefault<T>(sql, parameters, commandType: commandType, transaction: CurrentTransaction);
        }

        public IEnumerable<T> GetAll<T>(string sql, DynamicParameters parameters = null, CommandType commandType = CommandType.Text)
        {
            var cnn = GetConnection();
            return cnn.Query<T>(sql, parameters, commandType: commandType, transaction: CurrentTransaction);
        }
        public async Task<IEnumerable<T>> GetAllAsync<T>(string sql, DynamicParameters parameters = null, CommandType commandType = CommandType.Text)
        {
            var cnn = GetConnection();
            return await cnn.QueryAsync<T>(sql, parameters, commandType: commandType, transaction: CurrentTransaction);
        }
        public IDbConnection GetConnection()
        {
            return connectionContext.GetConnection();
        }
    }
}