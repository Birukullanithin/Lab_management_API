using System;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace LabManagement.Data
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            // Expect a connection string named "DefaultConnection" in appsettings
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public IDbConnection CreateConnection()
        {
            // Try to create NpgsqlConnection via reflection so the project compiles
            // even if the Npgsql package isn't yet installed. If Npgsql is available
            // the provider will be created; otherwise an informative exception is thrown.
            var npgsqlType = Type.GetType("Npgsql.NpgsqlConnection, Npgsql");
            if (npgsqlType != null)
            {
                var conn = Activator.CreateInstance(npgsqlType, _connectionString) as IDbConnection;
                if (conn != null) return conn;
            }
            throw new InvalidOperationException("Postgres provider 'Npgsql' not found. Add the Npgsql NuGet package (dotnet add package Npgsql) to use PostgreSQL.");
        }

        public string GetConnectionString() => _connectionString;
    }
}
