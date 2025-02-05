using System.Data;
using MySql.Data;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace eStore.DAL
{

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;
    using MySql.Data.MySqlClient;
    using Microsoft.Extensions.Configuration;

    namespace eStore.DAL
    {
        public abstract class BaseDAL
        {
            private readonly string _connectionString;

            /// <summary>
            /// Initializes the BaseDAL with the connection string from IConfiguration.
            /// </summary>
            /// <param name="configuration">The IConfiguration instance to access the connection string.</param>
            /// <param name="connectionName">The name of the connection string in the configuration file.</param>
            protected BaseDAL(IConfiguration configuration, string connectionName)
            {
                _connectionString = configuration.GetConnectionString(connectionName)
                    ?? throw new ArgumentException($"Connection string '{connectionName}' not found in configuration.");
            }

            /// <summary>
            /// Creates and returns a new database connection.
            /// </summary>
            /// <returns>IDbConnection instance</returns>
            protected IDbConnection CreateConnection()
            {
                return new MySqlConnection(_connectionString);
            }

            /// <summary>
            /// Executes a query and returns a collection of results.
            /// </summary>
            protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text)
            {
                using (var connection = CreateConnection())
                {
                    return await connection.QueryAsync<T>(sql, parameters);
                }
            }

            /// <summary>
            /// Executes a query and returns a single result.
            /// </summary>
            protected async Task<T?> QuerySingleAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.Text)
            {
                using (var connection = CreateConnection())
                {
                    return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters,commandType:commandType);
                }
            }

            /// <summary>
            /// Executes a non-query command (e.g., INSERT, UPDATE, DELETE).
            /// </summary>
            protected async Task<int> ExecuteAsync(string sql, object? parameters = null,CommandType commandType = CommandType.Text)
            {
                using (var connection = CreateConnection())
                {
                    return await connection.ExecuteAsync(sql, parameters,commandType:commandType);
                }
            }

            /// <summary>
            /// Executes a scalar query and returns a single value.
            /// </summary>
            protected async Task<T> ExecuteScalarAsync<T>(string sql, object? parameters = null)
            {
                using (var connection = CreateConnection())
                {
                    return await connection.ExecuteScalarAsync<T>(sql, parameters);
                }
            }


        }
    }
}

