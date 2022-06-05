using static System.Console;

namespace Chpat2;

public static class Util
{
    public static void Print<T>(T scalar)
    {
        WriteLine(scalar);
    }

    public static void Print<T>(IEnumerable<T> seq)
    {
        foreach (var item in seq)
        {
            Write(item + " ");
        }
        WriteLine();
    }
    
    public static IEnumerable<T> Filter<T>(this IEnumerable<T> ts, Func<T, bool> predicate)
    {
        foreach (var t in ts)
        {
            if (predicate(t))
            {
                yield return t;
            }
        }
    }

    public static Func<T2, T1, R> SwapArgs<T1, T2, R>(this Func<T1, T2, R> f)
        => (t1, t2) => f(t2, t1);
}