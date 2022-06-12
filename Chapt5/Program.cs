using static F;

// Example 1

using System.Threading.Channels;


string Greet(Option<string> greetee)
    => greetee.Match
    (
        None: () => "who?",
        Some: (x) => $"Hello ,{x}"
    );

//Console.WriteLine(Greet(None));

// public static readonly noneType None = default;

// Examaple 2. use Option

Console.WriteLine(Int.Parse("30"));
Console.WriteLine(Int.Parse("-10"));
Console.WriteLine(Int.Parse("abc"));

// Example 4. smart constructor pattern 아래 Age Struct 참고

// Example 5.

string GreetingFor(Subscriber subscriber)
    => subscriber.Name.Match(
        () => "Dear Subscriber,",
        (name) => $"Dear {name.ToUpper()},"
        );

Subscriber subscriber1 = new Subscriber("abc", "abc@abc.com");
Subscriber subscriber2 = new Subscriber(string.Empty, "abc@abc.com");

Console.WriteLine(GreetingFor(subscriber1));
Console.WriteLine(GreetingFor(subscriber2));

// Example 5-1  Nullable?
#nullable enable
string GreetingFor2(Subscriber2 subscriber)
    => $"Dear {subscriber.Name.ToUpper()},";

public record Subscriber
(
    Option<string> Name,
    string Email
);

#nullable enable
public record Subscriber2
(
    string? Name,
    string Email
);

// util Minimal Option

public static class F
{
    public static Option<T> Some<T>(T value) => new Option<T>(value);
    public static NoneType None => default;
}

public struct NoneType
{
}


public struct Option<T>
{
    private readonly T? _value;
    private readonly bool _isSome;
    
    internal Option(T value)
    {
        _value = value ?? throw new ArgumentNullException();
        _isSome = true;
    }

    public static implicit operator Option<T>(NoneType _)
        => default;

    public static implicit operator Option<T>(T value)
        => value is null ? None : Some(value);
    public R Match<R>(Func<R> None, Func<T, R> Some)
        => _isSome ? Some(_value!) : None();
}   

public struct Age
{
    private int Value { get; }

    public static Option<Age> Create(int age)
        => IsValid(age) ? Some(new Age(age)) : None;

    private Age(int value)
        => Value = value;

    private static bool IsValid(int age)
        => 0 <= age && age < 120;
}