using System.IO.Compression;
using static System.Linq.ParallelEnumerable;

namespace Chapt3;

static class StringExt
{
    // Pure function
    public static string ToSentenceCase(this string s)
        => s == string.Empty ? string.Empty : char.ToUpperInvariant(s[0]) + s.ToLower()[1..];
}


public class ListFormatter
{
    private int _counter;
    
    // impure function
    public  string PrependCounter(string s) => $"{++_counter}. {s}";

    public List<string> Format(List<string> list)
        => list.Select(StringExt.ToSentenceCase).ToList();
    // => list.AsParallel().Select(StringExt.ToSentenceCase).Select(PrependCounter).ToList();
    // impure 때문에 AsParallel이 원하는 동작은 안하게 된다.
}

public static class ListFormatterPureWithzip
{
    public static List<string> Format(List<string> list)
    {
        var left = list.Select(StringExt.ToSentenceCase);
        var right = Enumerable.Range(1, list.Count);
        var zipped = Enumerable.Zip(left, right, (s, i) => $"{i}. {s}");
        return zipped.ToList();
    }
    
    // zip(left, right, selector) === left.zip(right, selector); And with Parallel
    public static List<string> FormatFluent(List<string> list)
        => list.AsParallel().Select(StringExt.ToSentenceCase)
            .Zip(ParallelEnumerable.Range(1, list.Count), (s, i) => $"{i}. {s}")
            .ToList();
}
