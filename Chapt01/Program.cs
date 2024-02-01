using static System.Console;
using static System.Linq.Enumerable;

// 일급객체로서의 함수
void Example1()
{
  Func<int, int> mod2 = (int x) => x % 2;

  Range(1, 10).Select(mod2).AggregatePrint(); // 1, 0, 1, 0, ...
}

// State Mutation . Sort
void Example2()
{
  int[] nums = [3, 6, 2, 1];

  nums.OrderBy(x => x).AggregatePrint(); // 1, 2, 3, 6
  nums.AggregatePrint();; // 3, 6, 2, 1

  Array.Sort(nums);
  nums.AggregatePrint();; // 1, 2, 3, 6
}

// Example1();
// Example2();

public static class Ext
{
  private static void Print(this string c)
  {
    WriteLine(c);
  }
  
  public static void AggregatePrint<T>(this IEnumerable<T> source)
  {
    source.Aggregate(string.Empty, (acc, x) => $"{acc} {x}").Print();
  }
}
