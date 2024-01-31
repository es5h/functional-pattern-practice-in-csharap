using static System.Console;
using static System.Linq.Enumerable;


// 일급객체로서의 함수
void Example1()
{
  Func<int, int> mod2 = (int x) => x % 2;

  Range(1, 10).Select(mod2).ToList().ForEach(WriteLine); // 1, 0, 1, 0, ...
}

// State Mutation . Sort
void Example2()
{
  int[] nums = [3, 6, 2, 1];

  nums.OrderBy(x => x).ToList().ForEach(WriteLine); // 1, 2, 3, 6
  nums.ToList().ForEach(WriteLine); // 3, 6, 2, 1

  Array.Sort(nums);
  nums.ToList().ForEach(WriteLine); // 1, 2, 3, 6
}


Example2();


