using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Unit = System.ValueTuple;
using Dapper;
using LaYumba.Functional;

namespace Chapt14;

public delegate dynamic Middleware<T>(Func<T, dynamic> cont);

public class DbLogger
{
    private Middleware<SqlConnection> Connect;
    private Func<string, Middleware<Unit>> Time;
    
    public DbLogger(ConnectionString connString, ILogger log)
    {
        Connect = f => ConnectionHelper.Connect(connString, f);
        Time = op => f => Instrumentation.Time(log, op, f.ToNullary());
    }

    public void Log(LogMessage message) => (Connect.Map(conn =>
        conn.Execute("sp_create_log", message, commandType: CommandType.StoredProcedure))).Run();
    
    public void DeleteOldLogs() => (
        from _ in Time("DeleteOldLogs")
        from conn in Connect
        select conn.Execute ("DELETE {Logs] WHERE [Timestamp] < @upTo"), new {upTo = 7.Days().Ago()})).Run()
    )
}

public static class ConnectionHelper
{
    // string -> (SqlConnection -> R) -> R
    public static R Connect<R> (ConnectionString connString, Func<SqlConnection, R>f)
    {
        using var conn = new SqlConnection(connString);
        conn.Open();
        return f(conn);
    }    
}

public static class Instrumentation
{
    public static T Time<T>(ILogger log, string op, Func<T> f)
    {
        var sw = new Stopwatch();
        sw.Start();

        T t = f();
        sw.Stop();
        log.LogDebug($"{op} took {sw.ElapsedMilliseconds}ms");
        
        return t;
    }
}

public interface ILogger
{
    public void LogDebug(string s);
}

public record ConnectionString(string Value)
{
    public static implicit operator string(ConnectionString c) => c.Value;
    public static implicit operator ConnectionString(string s) => new(s);
}

public record LogMessage(string Value)
{
    public static implicit operator string(LogMessage c) => c.Value;
    public static implicit operator LogMessage(string s) => new(s);
}

public static class  MiddlewareExt
{
    public static T Run<T>(this Middleware<T> mw) => mw(t => t);
    public static Middleware<R> Map<T, R>(this Middleware<T> mw, Func<T, R> f)
        => cont => mw(t => cont(f(t)));
    
    public static Middleware<R> Bind<T, R>(this Middleware<T> mw, Func<T, Middleware<R>> f)
        => cont => mw(t => f(t)(cont));
    
    public static TimeSpan Days(this int @this)
        => TimeSpan.FromDays(@this);
    public static DateTime Ago(this TimeSpan @this)
        => DateTime.UtcNow - @this;
}