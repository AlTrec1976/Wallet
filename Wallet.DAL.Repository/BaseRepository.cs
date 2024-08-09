using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;

namespace Wallet.DAL.Repository
{
    public abstract class BaseRepository
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public BaseRepository(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task ExecuteAsync(string sql, object param = null)
        {
            try
            {
                using IDbConnection connection = GetConnection();
                await connection.QueryAsync(sql, param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при работе Execute");
                throw;
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
        {
            try
            {
                using IDbConnection connection = GetConnection();
                return await connection.QueryAsync<T>(sql, param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при работе QueryAsync");
                throw;
            }
        }

        public async Task<T> QuerySingleAsync<T>(string sql, object param = null)
        {
            try
            {
                using IDbConnection connection = GetConnection();
                return await connection.QueryFirstAsync<T>(sql, param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при работе QuerySingleAsync");
                throw;
            }
        }

        protected NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_configuration.GetConnectionString("DBConnection"));
        }
    }
}
