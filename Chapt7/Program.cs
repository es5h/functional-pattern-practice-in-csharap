using static System.Console;
using static System.Linq.Enumerable;
using static Ext;
using static F2;
using static F;


// Example1.  Chaining Compostition

var person = new Person("abc", "def");

WriteLine(person.AbbreviateName().AppendDomain());

Func<Person, string> emailFor = p => AppendDomain2(AbbreviateName2(p));

var optP = Some(new Person("abc", "def"));
var a = optP.Map(emailFor);
var b = optP.Map(AbbreviateName2).Map(AppendDomain2);
WriteLine(a.Equals(b));

// Example 2. 
var population = Range(1, 8)
    .Select(x => new People(Earning: x * 10000))
    .ToList();

WriteLine(AverageEarningsOfRichestQuartile(population));
// Composable
// Pure, Chaiable, General, Shape-preserving

// AfterRefactor;
WriteLine(AverageEarningsOfRichestQuartile2(population));

// Example 3. WorkFlow


public record People(decimal Earning);

public static class Ext
{
    // after refactor;
    public static decimal AverageEarning(this IEnumerable<People> pop)
        => pop.Average(p => p.Earning);

    public static IEnumerable<People> RichestQuartile(this IEnumerable<People> pop)
        => pop.OrderByDescending(x => x.Earning)
            .Take(pop.ToList().Count / 4);

    public static decimal AverageEarningsOfRichestQuartile2(IEnumerable<People> population)
        => population
            .RichestQuartile()
            .AverageEarning();
    
    // before refactor;
    public static decimal AverageEarningsOfRichestQuartile(List<People> population)
        => population
            .OrderByDescending(x => x.Earning)
            .Take(population.Count / 4)
            .Select(p => p.Earning)
            .Average();
    
    public static string AppendDomain(this string localpart)
        => $"{localpart}@gmail.com";

    public static string AbbreviateName(this Person p)
        => Abbreviate(p.FirstName) + Abbreviate(p.SecondName);

    static string Abbreviate(string s)
        => s.Substring(0, Math.Min(2, s.Length)).ToLower();

    public static string AppendDomain2(string localpart)
        => $"{localpart}@gmail.com";
    
    public static string AbbreviateName2(Person p)
        => Abbreviate(p.FirstName) + Abbreviate(p.SecondName);
}

public record Person(string FirstName, string SecondName);