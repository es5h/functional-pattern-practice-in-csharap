using System.ComponentModel;
using functional_pattern_practice_in_csharap;
using static System.Console;
using static System.Linq.Enumerable;
using static functional_pattern_practice_in_csharap.Chap1ClassExtensionAndHelper;

// Example1. Linq
var orderedEnumerable = Range(1, 3).OrderBy(x => -x);

// Print(orderedEnumerable);

// Example 2. Side effect of Mutation
var nums = Range(-10000, 20001).Reverse().ToArray();

var task1 = () => WriteLine(nums.Sum());
var task2 = () =>
{
    Array.Sort(nums);
    WriteLine(nums.Sum());
};

Parallel.Invoke(task1, task2); // 0, 5001

// Example 3. Shorthand syntax for coding functionally
Circle circle = new(3.0);
WriteLine(circle.Area);

// Example 4. Tuples and Class Extension
var (former, latter) = "abcdef".SplitAt(3);

var (even, odd) = Range(1, 5).Partition(i => i % 2 == 0);
Print(even);
Print(odd);

// Example 5. switch and record type
// static decimal Vat(Address address, Order order) => Vat(RateByCountry(address.Country), order);

static decimal RateByCountry(string country)
    => country switch
    {
        "it" => 0.22m,
        "jp" => 0.08m,
        _ => throw new ArgumentException($"Missing rate for {country}");
    };

static decimal Vat(decimal rate, Order order) => order.NetPrice * rate;

static decimal Vat(Address address, Order order)
    => address switch
    {
        ("de") _ => DeVat(order),
        (var country) _ => Vat(RateByCountry(country), order),
    };

static decimal DeVat(Order order) => order.NetPrice * (order.Product.IsFood ? 0.08m: 0.2m);
