using System.Data;
using System.Data.SqlClient;
using Dapper;
using LanguageExt;
using Microsoft.Extensions.Configuration;
using static ConnectionHelper;

public class DapperPartialApplication
{
  public void Example()
  {
    IConfiguration configruration = null; // example
    ConnectionString conn = configruration.GetSection("ConnString").Value;

    SqlTemplate sql = "SELECT * FROM Member";
    SqlTemplate sqlByCategory = $"{sql} WHERE Category = @Category";
    SqlTemplate sqlById = $"{sql} WHERE Id = @Id";

    // getMemberById: (object -> IEnumerable<Member>)
    var getMembersById = conn.Query<Member>(sqlById);

    // getMemberByCategory: (object -> IEnumerable<Member>)
    var getMembersByCategory = conn.Query<Member>(sqlByCategory);

    Option<Member> getMemberById(Guid id) => getMembersById(new { Id = id }).SingleOrDefault();
  }
}

public record Member(Guid Id, string MemberCode);

// type ConnectionString = string
public record ConnectionString(string Value)
{
  public static implicit operator string(ConnectionString connectionString) => connectionString.Value;
  public static implicit operator ConnectionString(string value) => new(value);
}

// type SqlScript = string
public record SqlTemplate(string Value)
{
  public static implicit operator string(SqlTemplate sqlScript) => sqlScript.Value;
  public static implicit operator SqlTemplate(string value) => new(value);

  public override string ToString() => Value;
}

public static class ConnStringExt
{
  // [참고] 기존 Dapper Api, IEnumerable<T> Query<T>(this IDbConnection connection, string sql, object param)
  // Query: (sqlTemplate, connection -> object -> IEnumerable<T>)

  // DbConnection 은 생명주기가 짧지만, ConnectionString 은 길다. 또한, 매 연결 마다 쿼리와 함께 Connect가 맺어진다.
  // ConnectionHelper 테크닉을 통해, 함수 인자의 순서를 변경하고, 부분 적용을 통해, 연결 문자열을 먼저 적용할 수 있다.
  // (ConnectionString, SqlTemplate) -> object -> IEnumerable<T>

  public static Func<object, IEnumerable<T>> Query<T>(this ConnectionString connStr, SqlTemplate sql)
    => param => Connect(connStr, conn => conn.Query<T>(sql, param));
  
  public static Func<object, TryOption<T>> TryExecute<T>(this ConnectionString connStr, SqlTemplate sql)
    => param => Connect(connStr, conn => conn.ExecuteScalar<TryOption<T>>(sql, param));
}

public static class ConnectionHelper
{
  // ConnectionString을 먼저 적용 할 수 있도록 적용.
  public static T Connect<T>(ConnectionString connStr, Func<IDbConnection, T> f)
  {
    using SqlConnection conn = new(connStr);
    conn.Open();
    return f(conn);
  }

  public static IEnumerable<T> Connect<T>(ConnectionString connStr, Func<IDbConnection, IEnumerable<T>> f)
  {
    using SqlConnection conn = new(connStr);
    conn.Open();
    return f(conn);
  }
  
  public static TryOption<T> Connect<T>(ConnectionString connStr, Func<IDbConnection, TryOption<T>> f)
  {
    using SqlConnection conn = new(connStr);
    conn.Open();
    return f(conn);
  }
}
