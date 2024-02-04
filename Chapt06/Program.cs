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

  population.Map(x => x.Age).Iter(x => WriteLine(x.ToString())); // 42, None, 66, None
  population.Bind(x => x.Age).Iter(x => WriteLine(x.ToString())); // 42, 66
  WriteLine(population.Bind(x => x.Age).Map(x => x.Value).Average()); // 54
}

Example2();


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
