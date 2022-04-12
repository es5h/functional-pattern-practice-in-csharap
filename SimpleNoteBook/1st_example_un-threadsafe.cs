/*using System.Reflection;
using static System.Linq.Enumerable;
using static System.Console;

var nums = Range(-10000,20001).Reverse().ToArray();

var task1 = () => WriteLine(nums.Sum()); // unpredictable value
var task2 = () =>
{
    Array.Sort(nums);
    WriteLine(nums.Sum());
};
var task3 = () => WriteLine(nums.OrderBy(x => x).Sum());

Parallel.Invoke(task1, task2, task3);*/