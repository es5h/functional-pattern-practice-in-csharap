using Functional.F1;
using static System.Console;

// Example 1.
using Name = System.String;
using Greeting = System.String;
using PersonalizedGreeting = System.String;

var greet = (Greeting greet, Name name) => $"{greet}, {name}";

Name[] names = {"Tristan", "ivan"};
names.Map(n => greet("Hello", n)).ForEach(WriteLine);

// greet : (Greeting, Name) -> PersonalizedGreeting

// Example2.

var greetWith = (Greeting gr) => (Name name) => $"{gr}, {name}";
var greetFormally = greetWith("goodEvening");
names.Map(greetFormally).ForEach(WriteLine);

// greetWith : Greeting -> (Name- > PersonalizedGreeting)
// arrow notaion 은 right associative 를 갖고 있어  Greeting -> Name -> PersonalizedGreeting 로 표기 가능하다.
// greet with 같은걸 curried form 이라한다.