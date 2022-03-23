using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;

namespace MyPomodoro.Infrastructure.Persistence.Dapper
{
    public interface IDapper
    {
        T Get<T>(string sql, DynamicParameters parameters = null, CommandType commandType = CommandType.Text);
        IEnumerable<T> GetAll<T>(string sql, DynamicParameters parameters = null, CommandType commandType = CommandType.Text);
        public Task<IEnumerable<T>> GetAllAsync<T>(string sql, DynamicParameters parameters = null, CommandType commandType = CommandType.Text);
        int Execute(string sql, DynamicParameters parameters, CommandType commandType = CommandType.Text);
        IDbConnection GetConnection();
        IDbTransaction CurrentTransaction { get; }
    }
}