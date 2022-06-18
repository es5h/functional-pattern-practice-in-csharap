using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Dapper;
using Functional.F1;
using static Functional.F1.F;
using static System.Console;

// Example 1.
using Name = System.String;
using Greeting = System.String;
using PersonalizedGreeting = System.String;

var greet = (Greeting greet, Name name) => $"{greet}, {name}";

Name[] names = {"Tristan", "ivan"};
names.Map(n => greet("Hello", n)).ForEach(WriteLine);

// greet : (Greeting, Name) -> PersonalizedGreeting

// Example2.

var greetWith = (Greeting gr) => (Name name) => $"{gr}, {name}";
var greetFormally = greetWith("goodEvening");
names.Map(greetFormally).ForEach(WriteLine);

// greetWith : Greeting -> (Name- > PersonalizedGreeting)
// arrow notaion 은 right associative 를 갖고 있어  Greeting -> Name -> PersonalizedGreeting 로 표기 가능하다.
// greet with 같은걸 curried form 이라한다.

// Example 3. Apply
var greetInformally = greet.Apply("Hey");
names.Map(greetInformally).ForEach(WriteLine);

// Example 4.
Some(9.0).Map(Math.Sqrt).ForEach(WriteLine);
PersonalizedGreeting GreetMethod(Greeting gr, Name name) => $"{gr}, {name}";

/*Func<Name, PersonalizedGreeting> GreetWith(Greeting greeting) 
    => GreetMethod.Apply(greeting);*/

Func<Name, PersonalizedGreeting> GreetWith1(Greeting greeting) 
    => FuncExt.Apply<Greeting, Name, PersonalizedGreeting>(GreetMethod, greeting);

Func<Name, PersonalizedGreeting> GreetWith2(Greeting greeting) 
    => new Func<Greeting, Name, PersonalizedGreeting>(GreetMethod).Apply(greeting);

// Example5. Currying
greetWith("Hello")("World");
var greetWith2 = greet.Curry();
var greetNostalgically = greetWith2("안녕");
names.Map(greetNostalgically).ForEach(WriteLine);


// Example6. Dapper Partial-application-friendly-api

IEnumerable<T> Query<T>(this IDbConnection conn, string sqlQuery, object param = null,
    SqlTransaction tran = null, bool buffered = true)
{
}

// original
const string sql = "SELECT 1";
string connString = "connString";
using (var conn = new SqlConnection(connString))
{
    conn.Open();
    var result = conn.Query(sql);
}

// StartUp.cs
// ConnectionString connString = configuration.GetSection("ConnecionString").Value;


public record ConnectionString(string Value)
{
    public static implicit operator string(ConnectionString c) => c.Value;
    public static implicit operator ConnectionString(string s) => new(s);
}


public static class FuncExt
{
    public static Func<T2, R> Apply<T1, T2, R>(this Func<T1, T2, R> f, T1 t1)
        => t2 => f(t1, t2);

    public static Func<T2, T3, R> Apply<T1, T2, T3, R>(this Func<T1, T2, T3, R> f, T1 t1)
        => (t2, t3) => f(t1, t2, t3);
    
    public static Func<T1, Func<T2, R>> Curry<T1, T2, R>(this Func<T1, T2, R> f)
        => t1 => t2 => f(t1, t2);
    public static Func<T1, Func<T2, Func<T3, R>>> Curry<T1, T2, T3, R> (this Func<T1, T2, T3, R> f)
        => t1 => t2 => t3 => f(t1, t2, t3);
}