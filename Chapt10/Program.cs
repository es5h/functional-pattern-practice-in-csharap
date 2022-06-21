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