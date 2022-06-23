// Example1.

using System.Data;
using LaYumba.Functional;
using static System.Console;
using static LaYumba.Functional.F;
    
var multiply = (int x, int y) => x * y;

// Map the function , then Apply
Some(3).Map(multiply).Apply(Some(4)).ForEach(WriteLine);;
Some(3).Map(multiply).Apply(None).ForEach(WriteLine);

// Lift the function, then APply
Some(multiply).Apply(Some(3)).Apply(Some(4)).ForEach(WriteLine);

// Functor, Applicative, Monad
// Functor (F<T>, map : (T->R)) -> F<R> , F<T> -> (T -> R) -> F<R>
// Simillaru, Monad. Bind M<T> -> T -> M<R> -> M<R>

// Right Identity
Option<int> m = Some(3);
WriteLine(m.Bind(Some).Equals(m));

// Left Identity
Func<int, IEnumerable<int>> f = i => Range(0, i);
WriteLine(f(3).Equals(List( 3).Bind(f)));
List(3).Bind(f).ForEach(WriteLine);

// Associativity (m >== f) >== g = m >== ((x -> f(x) >== g)  
// Deinfe >>= such that m >== f is m.Bind(f)   
Func<string, Option<double>> f1 = Parse;

Option<double> Parse(string s)
{
    double result;
    return double.TryParse(s, out result)
        ? Some(result) : None;
}

Func<double, Option<double>> g = d => d < 0 ? None : Some(Math.Sqrt(d));
Option<string> m1 = Some("23");
WriteLine(m1.Bind(f1).Bind(g).Equals(m1.Bind(x => f1(x).Bind(g))));

// Example2. Arbitrary Monad

var chars = new[] { 'a', 'b', 'c'};
var ints = new[] { 1, 2 };

chars.Bind(c => ints.Map(i => (c, i).ToString())).ForEach(WriteLine);