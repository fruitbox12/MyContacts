using Dapper;
using Microsoft.Extensions.Configuration;
using Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DataAccessLibrary
{
    public class SQLDataAccess : IDataAccess
    {
        private readonly IAppSettings _appSettings;



        public SQLDataAccess(IAppSettings appSettings)
        {
            _appSettings = appSettings;


        }
        //U parameter prevents SQLinjections
        public ResultSet<int> Create<T, U>(string sqlStatement, U parameters)
        {
            return DapperCreate<T, U>(sqlStatement, parameters);
        }


        // public SQLDataAccess(IAppSettings appSettings) THIS IS NOT PORTABLE! Depends on the consoleapp
        //   {
        //      _connectionString = configuration.GetConnectionString("Default");
        //   }

        public ResultSet<int> Count(string sqlStatement)
        {
            return DapperCount(sqlStatement);
        }

        public ResultSet<T> Read<T, U>(string sqlStatement, U parameters)
        {
            return DapperRead<T, U>(sqlStatement, parameters);
        }
        public ResultSet<int> Delete<T, U>(string sqlStatement, U parameters)
        {
            return DapperDelete<T, U>(sqlStatement, parameters);
        }

        public ResultSet<List<T>> ReadList<T, U>(string sqlStatement, U parameters)
        {
            return DapperReadList<T, U>(sqlStatement, parameters);
        }


        // Dapper Micro ORM
        private ResultSet<int> DapperCount(string sqlStatement)
        {
            ResultSet<int> resultSet = new ResultSet<int>();
            resultSet.Result = 0;
            try
            {
                using (IDbConnection connection = new SqlConnection(_appSettings.CurrentConnectionString))
                {
                    resultSet.Result = connection.ExecuteScalar<int>(sqlStatement);
                }
            }
            catch (SqlException e)
            {
                resultSet.CriticalError = true;
                resultSet.Result = 0;

                Trace trace = new Trace();
                trace.ErrorType = Trace.ErrorTypes.Critical;
                trace.ClassName = "SQLDataAccess";
                trace.MemberName = "Count()";
                trace.ErrorNumber = e.Errors[0].Number;

                    foreach (SqlError error in e.Errors)
                    {
                        trace.ErrorMessages.Add("Error Message: " + error.Message);
                        trace.ErrorMessages.Add("Error Number: " + error.Number);

                    }

               
                resultSet.AddTrace(trace);
            }

            return resultSet;
        }
        //  int 
        // returns 0 
        // areturns sql error number 
        ResultSet<int> DapperCreate<T, U>(string sqlStatement, U parameters)
        {
            ResultSet<int> resultSet = new ResultSet<int>();

            sqlStatement += "SELECT CAST(SCOPE_IDENTITY() AS int);";
            try
            {
                using (IDbConnection connection = new SqlConnection(_appSettings.CurrentConnectionString))
                {
                    resultSet.Result = connection.Query<int>(sqlStatement, parameters).SingleOrDefault();
                }
            }
            catch (SqlException ex)
            {
                resultSet.Result = 0;
                resultSet.ErrorNumber = ex.Number;
                resultSet.ErrorMessage = ex.Message;

                Trace trace = new Trace();
                trace.ClassName = "SQLDataAccess";
                trace.MemberName = "Create()";
                if (ex.Number == 2601)
                {
                    resultSet.LogicalError = true;
                    trace.ErrorType = Trace.ErrorTypes.Logical;
                }
                else
                {
                    resultSet.CriticalError = true;
                    trace.ErrorType = Trace.ErrorTypes.Critical;
                }
              
                    trace.ErrorNumber = ex.Number;
                    trace.addErrorMessage(ex.Message);
                  
                    foreach (SqlError error in ex.Errors)
                    {
                        trace.ErrorMessages.Add("Error Message: " + error.Message);
                        trace.ErrorMessages.Add("Error Number: " + error.Number);
                      
                    }
                    resultSet.AddTrace(trace);

            }

            return resultSet;
        }

        private ResultSet<List<T>> DapperReadList<T, U>(string sqlStatement, U parameters)
        {
            ResultSet<List<T>> resultSet = new ResultSet<List<T>>();

            try
            {
                using (IDbConnection connection = new SqlConnection(_appSettings.CurrentConnectionString))
                {
                    resultSet.Result = connection.Query<T>(sqlStatement, parameters).ToList();
                }
            }
            catch (SqlException ex)
            {
                resultSet.CriticalError = true;
                resultSet.Result = new List<T>();
                Trace trace = new Trace();
                trace.ErrorType = Trace.ErrorTypes.Critical;
                trace.ClassName = "SQLDataAccess";
                trace.MemberName = "ReadList()";
                resultSet.ErrorNumber = ex.Number;
                resultSet.ErrorMessage = ex.Message;

                foreach (SqlError error in ex.Errors)
                {
                trace.ErrorMessages.Add("Error Message: " + error.Message);
                trace.ErrorMessages.Add("Error Number: " + error.Number);
                
                }
                    resultSet.AddTrace(trace);

            }

            return resultSet;

        }
        private ResultSet<T> DapperRead<T, U>(string sqlStatement, U parameters)
        {
            // Creating a ResultSet instance creates an instance of object fields result to prevent null exceptions
            ResultSet<T> resultSet = new ResultSet<T>();

            try
            {
                using (IDbConnection connection = new SqlConnection(_appSettings.CurrentConnectionString))
                {
                    resultSet.Result = connection.Query<T>(sqlStatement, parameters).FirstOrDefault();

                    if (resultSet.Result is null)
                    {
                        resultSet.Result = (T)Activator.CreateInstance(typeof(T));
                    }
                }
            }
            catch (SqlException e)
            {
                resultSet.CriticalError = true;
                resultSet.Result = (T)Activator.CreateInstance(typeof(T));
                resultSet.ErrorNumber = e.Number;
                resultSet.ErrorMessage = e.Message;
                Trace trace = new Trace();
                trace.ErrorType = Trace.ErrorTypes.Critical;
                trace.ClassName = "SQLDataAccess()";
                trace.MemberName = "Read()";
             
               

                foreach (SqlError error in e.Errors)
                {

                    trace.ErrorMessages.Add("Error Message: " + error.Message);
                    trace.ErrorMessages.Add("Error Number: " + error.Number);
                  
                }
                    resultSet.AddTrace(trace);
            }

            return resultSet;

        }

        private string GetConnectionString(string connectionString = "Default")
        {
            string connection = "";

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            var config = builder.Build();

            connection = config.GetConnectionString(connectionString);
            return connection;
        }
        //Result 0 unsuccessful, 1 suncuessful
        private ResultSet<int> DapperDelete<T, U>(string sqlStatement, U parameters)
        {
            ResultSet<int> resultSet = new ResultSet<int>();

            
            try
            {
                using (IDbConnection connection = new SqlConnection(_appSettings.CurrentConnectionString))
                {
                    resultSet.Result = connection.Execute(sqlStatement, parameters);
                }
            }
            catch (SqlException ex)
            {
                resultSet.CriticalError = true;
                resultSet.Result = 0;

                Trace trace = new Trace();

                trace.ErrorType = Trace.ErrorTypes.Critical;
                trace.ClassName = "SQLDataAccess";
                trace.MemberName = "Delete()";
                resultSet.ErrorNumber = ex.Number;
                resultSet.ErrorMessage = ex.Message;


                foreach (SqlError error in ex.Errors)
                    {
                        trace.ErrorMessages.Add("Error Message: " + error.Message);
                        trace.ErrorMessages.Add("Error Number: " + error.Number);
         
                    }

                resultSet.AddTrace(trace);
            }

            return resultSet;

        }
    }
}
