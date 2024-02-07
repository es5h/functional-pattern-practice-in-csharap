using LanguageExt;
using static System.Console;
using Name = System.String;
using Greeting = System.String;
using PersonalizedGreeting = System.String;

// (Greeting, Name) => PersonalizedGreeting
var greet = (Greeting greet, Name name) => $"{greet}, {name}";
Name[] names = ["Alice", "Bob", "Charlie"];

names.Map(x => greet("Hello", x)).ToList().ForEach(WriteLine);

// Greeting => Name => PersonalizedGreeting
var greetCurried = (Greeting greet) => (Name name) => $"{greet}, {name}";
var greetHello = greetCurried("Hello");
names.Map(greetHello).ToList().ForEach(WriteLine);

// Curry with Apply
Func<Name, PersonalizedGreeting> greetHi = "Hi".Apply(greetCurried);
names.Map(greetHi).ToList().ForEach(WriteLine);

