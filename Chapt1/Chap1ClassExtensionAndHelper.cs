using static System.Console;

namespace functional_pattern_practice_in_csharap;

public static class Chap1ClassExtensionAndHelper
{
    public static (string, string) SplitAt(this string s, int at)
        => (s.Substring(0, at), s.Substring(at));

    public static (IEnumerable<int> even, IEnumerable<int> odd ) Partition(this IEnumerable<int> seq,
        Func<int, bool> predicate)
        => (seq.Where(predicate), seq.Where(x => !predicate(x)));
    
    // Util
    public static void Print<T>(IEnumerable<T> seq)
    {
        foreach (var item in seq)
        {
            Write(item + " ");
        }
        WriteLine();
    }
}