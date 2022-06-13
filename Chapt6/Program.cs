using System.Collections.Immutable;
using System.Runtime.InteropServices;
using Unit = System.ValueTuple;
using static System.Console;
using static System.Linq.Enumerable;
using static Chapt6.Util;
using static CustomExt;
using static F;
using Pet = System.String;

// Ex 1. Map : (C<T>, (T -> R)) -> C(R) , where C<T> is Functor
// (T->R)이라는 작용을 당해주는 객체라서 Functor 임
Range(1, 10).Map(i => 3 * i).ToList().ForEach(WriteLine);
Range(1, 10).ForEach(WriteLine);

// Ex 2,
Option<string> name = Some("Enrico");
name.Map(x => x.ToUpper()).ForEach(WriteLine);
IEnumerable<string> names = new[] { "Constance", "Albert" };
names.Map(x => x.ToUpper()).ForEach(WriteLine);

// Ex3. Bind
string input = "3";
Option<int> optI = Int.Parse(input);
Option<Option<Age>> ageOpt = optI.Map(x => Age.Create(x)); // not good nested Option

Func<string, Option<Age>> parseAge = s => Int.Parse(s).Bind(Age.Create);

WriteLine($"only {ReadAge()}!");


// Bind : (Option<T>, T -> Option<R>) -> Option<R> as same with Option => IEnumrable
// when type of binding map exists, c<T> is called Monad. 
// Ex3-1

var neighbors = new Neighbor[]
{
    new(Name: "abc", Pets: new Pet[] { "abc1", "abc2" }),
    new(Name: "def", Pets: new Pet[] { }),
    new(Name: "ghi", Pets: new Pet[] { "ghi1" }),
};
var nested = neighbors.Map(x => x.Pets).ForEach(WriteLine);
var flat = neighbors.Bind(x => x.Pets).ForEach(WriteLine);
    
// Ex 4. Return Function
var empty = List<string>();
var single = List("abc");
var many = List("abc", "def");

// Return T -> M<T>
// recall bind, Bind : (M<T>, (T -> M<R>) -> M<R>;
// Bind와 Return의 결과물인 M<T> 가 모나드로 간주되기 위한 몇가지 법칙들이 있다. (군, 체, 위상 등의 예시를 말하는듯)

// Map : (C<T>, f: (T -> R)) -> C<R>
// Bind : (C<T>, g: (T -> C<R>) -> C<R>
// Return h: T -> C<T>
// 단순히 위에 세개의 형태만 볼경우 BInd 의 function 과 Return 을 조합하면 Map를 구성할 수 있을 것처럼 보인다.
// g = f h^(-1) 로 둔다면? 이는 suboptimal(아직은 무슨말인지 와닿지않음) 자체 구현된 Map은 Bind에 의존하지 않는다 => 이는 모든 monad 가 functor 임을 의미

// Ex 5. Where For Optional
bool isNatural(int i) => i > 0;
Option<int> ToNatural(string s) => Int.Parse(s).Where(isNatural);
ToNatural("2").ForEach(WriteLine);
ToNatural("-2").ForEach(WriteLine);;
ToNatural("abc").ForEach(WriteLine);;

// Ex6. 

IEnumerable<Subject> population = new []
{
    new Subject(Age.Create(33)),
    new Subject(None),
    new Subject(Age.Create(123))
};

var optionalAge = population.Map(p => p.Age);
optionalAge.Map(x => x.ToString()).ForEach(WriteLine);
var statedAge = population.Bind(p => p.Age);
statedAge.Map(x => x.ToString()).ForEach(WriteLine);
WriteLine(statedAge.Map(age => age.Value).Average());

public record Subject(Option<Age> Age);


public record Neighbor(string Name, IEnumerable<Pet> Pets);

public static class CustomExt
{
    public static Option<Age> ParseAge(string s)
        => Int.Parse(s).Bind(Age.Create);

    public static Age ReadAge()
        => ParseAge(Prompt("input number")).Match(() => ReadAge(), age => age);
    public static string Prompt(string msg)
    {
        WriteLine("input number");
        return ReadLine();
    }
    public static IEnumerable<R> Map<T, R>(this IEnumerable<T> ts, Func<T, R> f)
    {
        foreach (var t in ts)
            yield return f(t);
    }

    public static Func<Unit> ToFunc(this Action action)
        => () =>
        {
            action();
            return default;
        };

    public static Func<T, Unit> ToFunc<T>(this Action<T> action)
        => t =>
        {
            action(t);
            return default;
        };

    public static IEnumerable<Unit> ForEach<T>
        (this IEnumerable<T> ts, Action<T> action)
        => ts.Map(action.ToFunc()).ToImmutableList();

    public static Option<R> Map<T, R>(this Option<T> optT, Func<T, R> f)
        => optT.Match
        (
            () => None,
            (t) => Some(f(t))
        );

    public static Option<Unit> ForEach<T>
        (this Option<T> opt, Action<T> action)
        => Map(opt, action.ToFunc());

    public static Option<R> Bind<T, R>(this Option<T> optT, Func<T, Option<R>> f)
        => optT.Match(() => None, f);

    public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> ts, Func<T, IEnumerable<R>> f)
    {
        foreach (T t in ts)
            foreach (R r in f(t))
                yield return r;
    }
    
    public static IEnumerable<T> List<T>(params T[] items)
        => items.ToImmutableList();

    public static Option<T> Where<T>(this Option<T> optT, Func<T, bool> pred)
        => optT.Match(() => None, t => pred(t) ? optT : None);

    public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> ts, Func<T, Option<R>> f)
        => ts.Bind(t => f(t).AsEnumerable());
    
    public static IEnumerable<R> Bind<T, R>(this Option<T> optT, Func<T, IEnumerable<R>> f)
        => optT.AsEnumerable().Bind(f);
}
