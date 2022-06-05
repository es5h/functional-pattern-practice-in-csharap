using System.Data;
using System.Data.SqlClient;

namespace Chapt2.DbLoggerExample.AfterPattern;

public static class ConnectionHelper
{
    public static R Connect<R>(string connString, Func<IDbConnection, R> f)
    {
        using var conn = new SqlConnection(connString);
        conn.Open();
        return f(conn);
    }
}