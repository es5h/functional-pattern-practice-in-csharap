using System.IO.Compression;
using System.Linq;
using static System.Console;
using static System.Linq.Enumerable;
namespace SimpleNoteBook;

static class StringExt
{
    public static string ToSentenceCase(this string s) =>
        s == string.Empty ? string.Empty : char.ToUpperInvariant(s[0]) + s.ToLower()[1..];
}
public static class Program
{
    private static int counter;
    private static string PrependCounter(string s) => $"{++counter}, {s}";

    private static List<string> StateMutatingFormat(this List<string> list)
        => list.AsParallel().Select(StringExt.ToSentenceCase).Select(PrependCounter).ToList();

    private static List<string> Format(this List<string> list) 
        => list.Select(StringExt.ToSentenceCase).Zip(Range(1, list.Count), (l, r) => $"{r} {l}").ToList();
    
    public static void Main(string[] args)
    {
        var itemList1 = Range(1, 10).Select(x => $"item{x}").ToList();
        // ar itemList2 = Range(1, 10).Select(x => $"item{x}").ToList();
        //itemList1.Format().ForEach(WriteLine);
        //WriteLine(itemList2.StateMutatingFormat().ToStringAndJoin()); // Order 엉망
    }
}