using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace ASP.NET_Console_Project
{
    public class SQLDataAccess
    {
        private readonly string _connectionString;

        public SQLDataAccess()
        {
            _connectionString = GetConnectionString();
        }

        // SQL to Dapper Micro ORM Wrappers for CRUD operations
        public int Count(string sqlStatement)
        {
            return DapperCount(sqlStatement);
        }

        public List<T> Read<T, U>(string sqlStatement, U parameters)
        {
            return DapperRead<T, U>(sqlStatement, parameters);
        }

        // Dapper Micro ORM

        /// CRUD Opertaions, Create(insert), read(select), update, delete
        private int DapperCount(string sqlStatement)
        {
            int count = 0;

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                // Scalar means give me a count (integer) 
                count = connection.ExecuteScalar<int>(sqlStatement);
            }

            return count;
        }


        private List<T> DapperRead<T, U>(string sqlStatement, U parameters)
        {

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                List<T> rows = connection.Query<T>(sqlStatement, parameters).ToList();
                return rows;
            }

        }

        private string GetConnectionString(string connectionString = "Default")
        {
            string connection = "";

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            var config = builder.Build();

            connection = config.GetConnectionString(connectionString);
            return connection;
        }
    }
}
