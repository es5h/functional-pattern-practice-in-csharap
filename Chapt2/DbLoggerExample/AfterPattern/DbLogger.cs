using System.Data;
using Dapper;

namespace Chapt2.DbLoggerExample.AfterPattern;

using static ConnectionHelper;

public class DbLogger
{
    private string _connString;

    public DbLogger(string connString)
    {
        _connString = connString;
    }

    public void Log(LogMessage msg)
        => Connect(_connString, c => c.Execute("sp_create_log", msg, commandType: CommandType.StoredProcedure));

    private string _sql = @"SELECT * FROM [LOG] WHERE [TimeStamp] > @since";

    public IEnumerable<LogMessage> GetLogs(DateTime since)
        => Connect(_connString, c => c.Query<LogMessage>(_sql, new { since = since }));
}