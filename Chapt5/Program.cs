// Example 1

string Greet(Option<string> greetee)
    => greetee switch
    {
        None<string> => "Sorry Whoo?",
        Some<string>(var name) => $"Hello {name}",
    };

// util Minimal OPtion
interface Option<T>
{
}

record None<T> : Option<T>;

record Some<T>(T value) : Option<T>;
