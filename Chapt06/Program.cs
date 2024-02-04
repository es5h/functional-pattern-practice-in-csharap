using LanguageExt;
using static System.Console;
using static LanguageExt.Option<Age>;

void Example1()
{
  WriteLine(ReadAge());

  static Age ReadAge() =>
    Int.Parse(Prompt("Enter Age: "))
      .Bind(Age.Create)
      .Match(
        None: () =>
        {
          WriteLine("Invalid age");
          return ReadAge();
        },
        Some: (age) => age
      );

  static string Prompt(string prompt)
  {
    WriteLine(prompt);
    return ReadLine();
  }
}

void Example2()
{
  IEnumerable<Subject> population = new List<Subject>
  {
    new(Age.Create(42)),
    new(None),
    new(Age.Create(66)),
    new(Age.Create(121)),
  };

  population.Map(x => x.Age).Iter(x => WriteLine(x.ToString())); // Some(42), None, Some(66), None
  population.Bind(x => x.Age).Iter(x => WriteLine(x.ToString())); // 42, 66
  WriteLine(population.Bind(x => x.Age).Map(x => x.Value).Average()); // 54
}

void Exercise1()
{
  Dictionary<string, int> dict = new()
  {
    ["one"] = 1,
    ["two"] = 2,
    ["three"] = 3,
  };
  
  dict.Map(x => x + 1).Iter(x => WriteLine($"{x.Key} : {x.Value}")); // one : 2, two : 3, three : 4
}

void Exercise2()
{
  WriteLine(Option<int>.Some(42).Map2(x => x + 1)); // Some(43)
  new List<int> { 1, 2, 3, 4, 5 }.Map2(x => x + 1).Iter(x => WriteLine(x.ToString())); // 2, 3, 4, 5, 6
}

// Exercise1();

// Ex1. ISet, IDctionarary의 Map
public static class DictionaryAndSetExt 
{
  public static IDictionary<K, R> Map<K, T, R>(this IDictionary<K, T> dict, Func<T, R> f) where K : notnull =>
    dict.Select(kv => (kv.Key, f(kv.Value)))
      .ToDictionary(kv => kv.Item1, kv => kv.Item2);
  
  public static ISet<R> Map<T, R>(this ISet<T> set, Func<T, R> f) =>
    set.Select(f).ToHashSet();
}


// Ex2. Bind와 Return 으로 구현한 Map
public static class EnumerableExt
{
  public static IEnumerable<R> Return<R>(R value) => new[] { value };
    
  public static IEnumerable<R> Map2<T, R>(this IEnumerable<T> ts, Func<T, R> f) =>
    ts.Bind(t => Return(f(t)));
}

public static class OptionExt
{
  public static Option<T> Return<T> (T t) => Option<T>.Some(t);
  public static Option<R> Map2<T, R>(this Option<T> opt, Func<T, R> f) =>
    opt.Bind(t => Return(f(t)));
}

record Subject(Option<Age> Age);

public struct Age
{
  public int Value { get; }

  public static Option<Age> Create(int value) =>
    value >= 0 && value <= 120
      ? Some(new Age(value))
      : None;

  private Age(int value) => Value = value;

  public static implicit operator int(Age age) => age.Value;

  private static bool IsValid(int age) => age is >= 0 and <= 120;

  public override string ToString() => Value.ToString();
}

public static class Int
{
  public static Option<int> Parse(string value) =>
    int.TryParse(value, out int result)
      ? Option<int>.Some(result)
      : Option<int>.None;
}
