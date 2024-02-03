using System.Diagnostics;
using LanguageExt;

Tools.Time("Hello, World!", () => Console.WriteLine("Hello, World!"));
var x = Tools.Time("Hello, x", () => "Hello, x");
Console.WriteLine(x);

public static class Tools
{
  public static T Time<T>(string op, Func<T> f)
  {
    var sw = new Stopwatch();
    sw.Start();
    
    T t = f();

    sw.Stop();
    Console.WriteLine($"{op} : {sw.ElapsedMilliseconds}ms");
    return t;
  }

  public static void Time(string op, Action action) => Time(op, action.ToFunc());
  public static T Time<T>(string op, Action<T> action) => Time<T>(op, _ => action.ToFunc());
  public static Func<Unit> ToFunc(this Action action) => () => { action(); return default; };
  public static Func<T, Unit> ToFunc<T>(this Action<T> action) => t => { action(t); return default; };
}
