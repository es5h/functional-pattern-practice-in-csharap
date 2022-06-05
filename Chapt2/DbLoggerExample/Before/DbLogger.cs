using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Chapt2.DbLoggerExample.before;

public class DbLogger
{
    private string _connString;

    public DbLogger(string connString)
    {
        _connString = connString;
    }

    public void Log(LogMessage msg)
    {
        using (var conn = new SqlConnection(_connString))
        {
            conn.Execute("sp_create_log", msg, commandType: CommandType.StoredProcedure);
        }
    }
    
    public IEnumerable<LogMessage> GetLogs(DateTime since)
    {
        var sql = "SELECT * from [Logs] WHERE [TIMESTAMP] > @since";
        
        using (var conn = new SqlConnection(_connString))
        {
            return conn.Query<LogMessage>(sql, new { since = since });
        }
    }

}