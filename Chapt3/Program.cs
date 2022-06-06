// Example 1. Mutate

using Chapt3;
using static System.Linq.Enumerable;

decimal RecoumputeTotal(Order order, List<OrderLine> linesToDelete)
{
    var result = 0m;
    foreach (var line in order.OrderLines)
    {
        if (line.Quantity == 0) linesToDelete.Add(line);
        else result += line.Product.Price * line.Quantity;
    }

    return result;
}

// Example 1-1 refactored
(decimal NewTotal, IEnumerable<OrderLine> LinesToDelete) RecomputeTotal(Order order)
    => (order.OrderLines.Sum(l => l.Product.Price * l.Quantity), order.OrderLines.Where(l => l.Quantity == 0));

// Example 2. state mutation 을 피하고 병렬화 시키기

var shoppingList = new List<string>
{
    "BANANAS",
    "coffee beans",
};

new ListFormatter()
    .Format(shoppingList)
    .ForEach(Console.WriteLine);

shoppingList.AsParallel().Select(new ListFormatter().PrependCounter).ToList();
// 위의 함수가 impure 라서 예상치도 못한 결과 나올 예정, 갯수가 많을 수록 티가 더 난다.

var sampeleList = Range(1, 1000).Select(x => $"item {x}").ToList();

ListFormatterPureWithzip
    .FormatFluent(sampeleList)
    .ForEach(Console.WriteLine);

new ListFormatter()
    .Format(sampeleList)
    .ForEach(Console.WriteLine);

// exmaple 3. IO
MakeTransferTest.WhenTransferDateIsFuture_ThenValidatePasses();


// util for example 1.
internal record Order(List<OrderLine> OrderLines);

internal record OrderLine (decimal Quantity, Product Product);

internal record Product(decimal Price);
