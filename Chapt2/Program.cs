using static System.Linq.Enumerable;
using static Chpat2.Util;

// Example1. Delegate
var list = Range(1, 10).Select(i => i * 3).ToList();
Print(list);

// delegate
Comparison<int> alphabetically = (l, r) => String.Compare(l.ToString(), (r.ToString()), StringComparison.Ordinal);
list.Sort(alphabetically);
Print(list);

// delegate Greeting Greeter(Person p); simply, use Func<Person, Greeting>

// Example 2. Where, Here is Filter we implement
var filtered = list.Filter(x => x % 6 == 0);

Print(filtered);

// Example 3. Adapter Function
var divide = (int x, int y) => x / y;
Print(divide(10, 2));

var divideBy = divide.SwapArgs();
Print(divideBy(2, 10));