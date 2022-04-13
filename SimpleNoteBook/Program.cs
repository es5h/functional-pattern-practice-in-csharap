using test1;
using static System.String;
namespace test1
{
    public static class TestSet1
    {
        public static string ToStringAndJoin(this IEnumerable<int> s) => Join(",", s.Select(x => x.ToString()));
        // this 연습
        public static (string, string) SplitAt(this string s, int at) => (s.Substring(0, at), s.Substring(at)); //??
        public static (string Base, string Quote) AsPair(this string ccyPair) => ccyPair.SplitAt(3);

        public static void Test1()
        {
            var (baseCcy, quoteCcy) = "abcdefg".SplitAt(3);
            var pair = "ghijkl".AsPair();
            Console.WriteLine(baseCcy);
            Console.WriteLine(pair.Quote);
        }

        // 책에 퀴즈처럼 있길래 작성해봄
        public static (IEnumerable<int>, IEnumerable<int>) Partition(this IEnumerable<int> s, Func<int, bool> fn) =>
            (s.Where(fn), s.Where(x => !fn(x)));
        

        public static void PartitionMaker()
        {
            var (even, odd) = Enumerable.Range(1, 10).Partition(x => x % 2 == 0);

            Console.WriteLine(even.ToStringAndJoin());
            Console.WriteLine(odd.ToStringAndJoin());
        }
    }
}

namespace Test2
{
    public class Program
    {
        private static Comparison<int> alphabetically = (left, right) => Compare(left.ToString(), right.ToString(), StringComparison.Ordinal);    
        
        public static void Main(string[] args)
        {
            var list = Enumerable.Range(1, 10).Select(x => x * 3).ToList();
            list.Sort(alphabetically);
            Console.WriteLine(list.ToStringAndJoin());
            
        }
    }
}
