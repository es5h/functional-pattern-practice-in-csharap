﻿## Middleware Pattern
아래와 같이 있다. Connection 은 Db에 연결 , Trace 는 로그 기록
```c#
// Connection
public static class ConnectionHelper
{
    public static R Connect<R> (ConnectionString connString, Func<SqlConeection, R>f)
    {
        using var conn = new SqlConnection(connString);
        conn.Open();
        return f(conn);
    }    
}

public void Log(LogMessage message)
    => Connect(connString, c => c.Execute("sp_create_log", message, commandType: commandType.StoredProcedure);

// Trace
public static Instrumentation
{
    public static T Trace<T> (ILogger log, string op, Func<T> f)
    {
        log.LogTrace($"Entering {op}");
        T t = f(t);
        log.LogTrace($"Leaving {op}");
        return t;
    }
}
```
합치면 이렇게 될까?
```c#
public void Log(LogMEssage mssage)
    => Instrumentation.Trace("CreateLog", 
        () => ConnectionHelper.Connect(connStringg, 
            c => c.Execute("sp_create_log", message, commandType: commandType.StoredProcedure)));
```

Callback 지옥 때문에 읽기힘듬

위의 두메서드를 functional signature 로 표현하면 아래와 같다.

Connect : `ConnectionString ->`**`(SqlConnection -> R) -> R`**

Trace : `ILogger -> string -> `**`(() -> R) -> R`**

추가로 2개 더 상상하자 Time, Transact

Time : `ILogger -> String -> `**`(() -> R) -> R`**

Transact : `SqlConnection -> `**`(SqlTransaction -> R) -> R`**

bold 돼 있는 부분은 공통적이다

**`(T -> R) -> R`**: `(T -> dynamic) -> dynamic`
로 추상화 가능 
```c#
public delegate dynamic MiddleWare(Func<T, dynamic> cont);
```

`Middleware<T>` is monad over `T`

```c#
public static MiddleWare<R> Bind<T, R> (this MidleWare<T> mw, Func<T, MiddleWare<R>> f)
    => cont => mw(t => f(t)(cont));
```

```c#
public static MiddleWare<R> Map<T, R> (this MidleWare<T> mw, Func<T, R> f)
    => cont => mw(t => cont(f(t)));
```

```c#
public class DbLogger
{
    MiddleWare<SqlConnection> Connect;
    
    public DbLogger(ConnectionString connString){
        Connect = f => ConnectionHelper.Connect(connString, f);
    }
}
```