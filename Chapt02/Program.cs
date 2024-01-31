using static System.Linq.Enumerable;
using static System.Console;

// 다양한 함수의 형태 
void Example1()
{
  // Lambda Expression
  Func<int, bool> isFactorOf6LE = x => 6 % x == 0;
  
  // Local Function
  bool IsFactorOf6LF(int x) => 6 % x == 0;

  Range(1, 6).Filter(isFactorOf6LE).ToList().ForEach(WriteLine); // 1, 2, 3, 6
  Range(1, 6).Filter(IsFactorOf6LF).ToList().ForEach(WriteLine); // 1, 2, 3, 6
}

// 고차함수 반환 형태
void Example2()
{
  var firstMinusSecond = (int x, int y) => x - y;
  
  WriteLine(firstMinusSecond(2, 1)); // 1

  WriteLine(firstMinusSecond.SwapArgs()(2, 1)); // -1
  // or
  var secondMinusFirst = firstMinusSecond.SwapArgs();
  WriteLine(secondMinusFirst(2, 1)); // -1
}

void Excercise()
{
  List<int> nums = [3, 5, 1, 4, 2, 8, 9];

  nums.QuickSort((x, y) => x - y).ToList().ForEach(WriteLine); // 1, 2, 3, 4, 5, 8, 9
  nums.QuickSort((x, y) => y - x).ToList().ForEach(WriteLine); // 9, 8, 5, 4, 3, 2, 1
}

Excercise();

public static class Ext
{
  public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> predicate)
  {
    foreach (T item in source)
    {
      if (predicate(item))
      {
        yield return item;
      }
    }
  }
  
  public static Func<T1, T2, T3> SwapArgs<T1, T2, T3>(this Func<T2, T1, T3> f)
    => (t1, t2) => f(t2, t1);
  
  public static IEnumerable<T> QuickSort<T>(this IEnumerable<T> list, Comparison<T> comparer)
  {
    List<T> quickSort = list.ToList();

    if (quickSort.ToList().Count <= 1)
    {
      return quickSort;
    }
    
    T pivot = quickSort[0];
    IEnumerable<T> rest = quickSort.Skip(1);

    IEnumerable<T> left = rest.Filter(x => comparer(x, pivot) <= 0);
    IEnumerable<T> right = rest.Filter(x => comparer(x, pivot) > 0);
    
    return left.QuickSort(comparer).Concat(new List<T> { pivot }).Concat(right.QuickSort(comparer)).ToList();
  }
}
