using System.Text.RegularExpressions;
using LanguageExt;
using static LanguageExt.Option<string>;
using static LanguageExt.Option<int>;

void Example1()
{
  Option<string> nothing = Option<string>.None;
  Option<string> man = Some("man");

  Console.WriteLine(Greet(nothing));
  Console.WriteLine(Greet(man));

  string Greet(Option<string> greetee) => greetee.Match(
    None: () => "Hello, Stranger!",
    Some: (g) => $"Hello, {g}!"
  );
}

void Example2()
{
  Console.WriteLine(Int.Parse("123")); // Some(123)
  Console.WriteLine(Int.Parse("abc")); // None
}

void Exercise1()
{
  Console.WriteLine(Enum.Parse<DayOfWeek>("Monday")); // Some(Monday)
  Console.WriteLine(Enum.Parse<DayOfWeek>("Moonday")); // None
}

void Exercise2()
{
  new List<int> { 1, 2, 3, 4, 5 }.Lookup(i => i % 2 == 0).Match(
    None: () => Console.WriteLine("No even numbers found"),
    Some: (i) => Console.WriteLine($"First even number found: {i}")
  );
}

void Exercise3()
{
  Console.WriteLine(Email.Create("es5h@github.com")); // Some(es5h@github.com)
  Console.WriteLine(Email.Create("es5h#github.com")); // None
}

Exercise3();

public class Email
{
  static readonly Regex emailRegex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

  private string Value { get; }

  private Email(string value) => Value = value;

  public static Option<Email> Create(string value) =>
    emailRegex.IsMatch(value) 
      ? Option<Email>.Some(new Email(value)) 
      : Option<Email>.None;

  public static implicit operator string(Email email) => email.Value;

  public override string ToString() => Value;
}


public static class ListExt
{
  public static Option<T> Lookup<T>(this IEnumerable<T> list, Func<T, bool> pred)
  {
    foreach (T item in list)
      if (pred(item))
      {
        return Option<T>.Some(item);
      }

    return Option<T>.None;
  }
}

public static class Enum
{
  public static Option<T> Parse<T>(string input) where T : struct =>
    System.Enum.TryParse<T>(input, out T result) ? Option<T>.Some(result) : Option<T>.None;
}

public enum DayOfWeek { Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday }

public static class Int {
  public static Option<int> Parse(string input) =>
    int.TryParse(input, out int i) ? Some(i) : Option<int>.None;
}

