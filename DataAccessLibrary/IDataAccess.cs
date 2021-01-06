using System.Collections.Generic;

namespace DataAccessLibrary
{
    public interface IDataAccess
    {
        // creates interface for data access
        ResultSet<int> Count(string sqlStatement);
        ResultSet<int> Create <T, U>(string sqlStatement, U parameters);

        ResultSet<int> Delete<T, U>(string sqlStatement, U parameters);
        ResultSet<T> Read<T, U>(string sqlStatement, U parameters);
        ResultSet<List<T>> ReadList<T, U>(string sqlStatement, U parameters);
    }
}