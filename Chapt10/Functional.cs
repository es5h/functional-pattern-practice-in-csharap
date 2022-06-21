// Either
// Error
// Exceptional
// F
// EnumerableExt
// FunctionExt
// Option
// ActionExt
// TaskExt
using System;
using Unit = System.ValueTuple;
using System.Collections.Generic;
using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Linq;
using System.Threading.Tasks;

namespace LaYumba.Functional
{
   using static F;

   public static partial class F
   {
      public static Task<T> Async<T>(T t) => Task.FromResult(t);
   }

   public static class TaskExt
   {
      public static async Task<R> Apply<T, R>
         (this Task<Func<T, R>> f, Task<T> arg)
         //=> (await f)(await arg); // simple version, less efficient
         => (await f.ConfigureAwait(false))(await arg.ConfigureAwait(false)); // ConfigureAwait(false) more efficient, but not for UI-thread apps

      public static Task<Func<T2, R>> Apply<T1, T2, R>
         (this Task<Func<T1, T2, R>> f, Task<T1> arg)
         => Apply(f.Map(F.Curry), arg);

      public static Task<Func<T2, T3, R>> Apply<T1, T2, T3, R>
         (this Task<Func<T1, T2, T3, R>> @this, Task<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Task<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>
         (this Task<Func<T1, T2, T3, T4, R>> @this, Task<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Task<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>
         (this Task<Func<T1, T2, T3, T4, T5, R>> @this, Task<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Task<Func<T2, T3, T4, T5, T6, R>> Apply<T1, T2, T3, T4, T5, T6, R>
         (this Task<Func<T1, T2, T3, T4, T5, T6, R>> @this, Task<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Task<Func<T2, T3, T4, T5, T6, T7, R>> Apply<T1, T2, T3, T4, T5, T6, T7, R>
         (this Task<Func<T1, T2, T3, T4, T5, T6, T7, R>> @this, Task<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Task<Func<T2, T3, T4, T5, T6, T7, T8, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R>
         (this Task<Func<T1, T2, T3, T4, T5, T6, T7, T8, R>> @this, Task<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Task<Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
         (this Task<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> @this, Task<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static async Task<R> Map<T, R>
         (this Task<T> task, Func<T, R> f)
         //=> f(await task);
         => f(await task.ConfigureAwait(false));

      public static async Task<R> Map<R>
         (this Task task, Func<R> f)
      {
         await task;
         return f();
      }

      public static Task<Func<T2, R>> Map<T1, T2, R>
         (this Task<T1> @this, Func<T1, T2, R> func)
          => @this.Map(func.Curry());

      public static Task<Func<T2, T3, R>> Map<T1, T2, T3, R>
         (this Task<T1> @this, Func<T1, T2, T3, R> func)
          => @this.Map(func.CurryFirst());

      public static Task<Func<T2, T3, T4, R>> Map<T1, T2, T3, T4, R>
         (this Task<T1> @this, Func<T1, T2, T3, T4, R> func)
          => @this.Map(func.CurryFirst());

      public static Task<Func<T2, T3, T4, T5, R>> Map<T1, T2, T3, T4, T5, R>
         (this Task<T1> @this, Func<T1, T2, T3, T4, T5, R> func)
          => @this.Map(func.CurryFirst());

      public static Task<Func<T2, T3, T4, T5, T6, R>> Map<T1, T2, T3, T4, T5, T6, R>
         (this Task<T1> @this, Func<T1, T2, T3, T4, T5, T6, R> func)
          => @this.Map(func.CurryFirst());

      public static Task<Func<T2, T3, T4, T5, T6, T7, R>> Map<T1, T2, T3, T4, T5, T6, T7, R>
         (this Task<T1> @this, Func<T1, T2, T3, T4, T5, T6, T7, R> func)
          => @this.Map(func.CurryFirst());

      public static Task<Func<T2, T3, T4, T5, T6, T7, T8, R>> Map<T1, T2, T3, T4, T5, T6, T7, T8, R>
         (this Task<T1> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, R> func)
          => @this.Map(func.CurryFirst());

      public static Task<Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
         (this Task<T1> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> func)
          => @this.Map(func.CurryFirst());

      public static Task<R> Map<T, R>
         (this Task<T> task, Func<Exception, R> Faulted, Func<T, R> Completed)
         => task.ContinueWith(t =>
               t.Status == TaskStatus.Faulted
                  ? Faulted(t.Exception!)
                  : Completed(t.Result));

      public static Task<Unit> ForEach<T>(this Task<T> @this, Action<T> continuation)
          => @this.ContinueWith(t => continuation.ToFunc()(t.Result)
              , TaskContinuationOptions.OnlyOnRanToCompletion);

      public static async Task<R> Bind<T, R>
         (this Task<T> task, Func<T, Task<R>> f)
          //=> await f(await task);
          => await f(await task.ConfigureAwait(false)).ConfigureAwait(false);


      public static Task<T> OrElse<T>
         (this Task<T> task, Func<Task<T>> fallback)
         => task.ContinueWith(t =>
               t.Status == TaskStatus.Faulted
                  ? fallback()
                  : Task.FromResult(t.Result)
            )
            .Unwrap();


      public static Task<T> Recover<T>
         (this Task<T> task, Func<Exception, T> fallback)
         => task.ContinueWith(t =>
               t.Status == TaskStatus.Faulted
                  ? fallback(t.Exception!)
                  : t.Result);

      public static Task<T> RecoverWith<T>
         (this Task<T> task, Func<Exception, Task<T>> fallback)
         => task.ContinueWith(t =>
               t.Status == TaskStatus.Faulted
                  ? fallback(t.Exception!)
                  : Task.FromResult(t.Result)
         ).Unwrap();

      // LINQ

      public static async Task<RR> SelectMany<T, R, RR>
         (this Task<T> task, Func<T, Task<R>> bind, Func<T, R, RR> project)
      {
         T t = await task;
         R r = await bind(t);
         return project(t, r);
      }

      public static async Task<RR> SelectMany<T, R, RR>
         (this Task<T> task, Func<T, ValueTask<R>> bind, Func<T, R, RR> project)
      {
         T t = await task;
         R r = await bind(t);
         return project(t, r);
      }

      public static async Task<RR> SelectMany<R, RR>
         (this Task task, Func<Unit, Task<R>> bind, Func<Unit, R, RR> project)
      {
         await task;
         R r = await bind(Unit());
         return project(Unit(), r);
      }

      public static async Task<R> SelectMany<T, R>(this Task<T> task, Func<T, Task<R>> f)
         => await f(await task.ConfigureAwait(false)).ConfigureAwait(false);

      public static async Task<R> Select<T, R>(this Task<T> task, Func<T, R> f)
         => f(await task);

      public static async Task<T> Where<T>(this Task<T> source
          , Func<T, bool> predicate)
      {
         T t = await source;
         if (!predicate(t)) throw new OperationCanceledException();
         return t;
      }

      public static async Task<V> Join<T, U, K, V>(
          this Task<T> source, Task<U> inner,
          Func<T, K> outerKeySelector, Func<U, K> innerKeySelector,
          Func<T, U, V> resultSelector)
      {
         await Task.WhenAll(source, inner);
         if (!EqualityComparer<K>.Default.Equals(
             outerKeySelector(source.Result), innerKeySelector(inner.Result)))
            throw new OperationCanceledException();
         return resultSelector(source.Result, inner.Result);
      }

      public static async Task<V> GroupJoin<T, U, K, V>(
          this Task<T> source, Task<U> inner,
          Func<T, K> outerKeySelector, Func<U, K> innerKeySelector,
          Func<T, Task<U>, V> resultSelector)
      {
         T t = await source;
         return resultSelector(t,
             inner.Where(u => EqualityComparer<K>.Default.Equals(
                 outerKeySelector(t), innerKeySelector(u))));
      }
   }
}

namespace LaYumba.Functional
{
   using static F;

   public static class ActionExt
   {
      public static Func<Unit> ToFunc(this Action action)
         => () => { action(); return default; };

      public static Func<T, Unit> ToFunc<T>(this Action<T> action)
         => t => { action(t); return default; };

      public static Func<T1, T2, Unit> ToFunc<T1, T2>(this Action<T1, T2> action)
         => (T1 t1, T2 t2) => { action(t1, t2); return default; };

      public static Func<Task<Unit>> ToFunc(this Func<Task> f)
         => async () => { await f(); return Unit(); };

      public static Func<T, Task<Unit>> ToFunc<T>(this Func<T, Task> f)
         => async (t) => { await f(t); return Unit(); };

      public static Func<T1, T2, Task<Unit>> ToFunc<T1, T2>(this Func<T1, T2, Task> f)
         => async (t1, t2) => { await f(t1, t2); return Unit(); };
   }
}

namespace LaYumba.Functional
{
   using static F;

   public static partial class F
   {
      public static Validation<T> Valid<T>(T value)
         => new(value ?? throw new ArgumentNullException(nameof(value)));

      // create a Validation in the Invalid state
      public static Validation.Invalid Invalid(params Error[] errors) => new(errors);
      public static Validation<T> Invalid<T>(params Error[] errors) => new Validation.Invalid(errors);
      public static Validation.Invalid Invalid(IEnumerable<Error> errors) => new(errors);
      public static Validation<T> Invalid<T>(IEnumerable<Error> errors) => new Validation.Invalid(errors);
   }

   public struct Validation<T>
   {
      internal IEnumerable<Error> Errors { get; }
      internal T? Value { get; }

      public bool IsValid { get; }

      public static Validation<T> Fail(IEnumerable<Error> errors)
         => new(errors);

      public static Validation<T> Fail(params Error[] errors)
         => new(errors.AsEnumerable());

      private Validation(IEnumerable<Error> errors)
         => (IsValid, Errors, Value) = (false, errors, default);

      internal Validation(T t)
         => (IsValid, Errors, Value) = (true, Enumerable.Empty<Error>(), t);

      public static implicit operator Validation<T>(Error error)
         => new Validation<T>(new[] { error });
      public static implicit operator Validation<T>(Validation.Invalid left)
         => new Validation<T>(left.Errors);
      public static implicit operator Validation<T>(T right) => Valid(right);

      public R Match<R>(Func<IEnumerable<Error>, R> Invalid, Func<T, R> Valid)
         => IsValid ? Valid(this.Value!) : Invalid(this.Errors);

      public Unit Match(Action<IEnumerable<Error>> Invalid, Action<T> Valid)
         => Match(Invalid.ToFunc(), Valid.ToFunc());

      public IEnumerator<T> AsEnumerable()
      {
         if (IsValid) yield return Value!;
      }

      public override string ToString()
         => IsValid
            ? $"Valid({Value})"
            : $"Invalid([{string.Join(", ", Errors)}])";

      public override bool Equals(object? obj)
         => obj is Validation<T> other
            && this.IsValid == other.IsValid
            && (IsValid && this.Value!.Equals(other.Value)
               || this.ToString() == other.ToString());

      public override int GetHashCode() => Match
      (
         Invalid: errs => errs.GetHashCode(),
         Valid: t => t!.GetHashCode()
      );
   }

   public static class Validation
   {
      public struct Invalid
      {
         internal IEnumerable<Error> Errors;
         public Invalid(IEnumerable<Error> errors) { Errors = errors; }
      }

      public static T GetOrElse<T>(this Validation<T> opt, T defaultValue)
         => opt.Match(
            (errs) => defaultValue,
            (t) => t);

      public static T GetOrElse<T>(this Validation<T> opt, Func<T> fallback)
         => opt.Match(
            (errs) => fallback(),
            (t) => t);

      public static Validation<R> Apply<T, R>(this Validation<Func<T, R>> valF, Validation<T> valT)
         => valF.Match(
            Valid: (f) => valT.Match(
               Valid: (t) => Valid(f(t)),
               Invalid: (err) => Invalid(err)),
            Invalid: (errF) => valT.Match(
               Valid: (_) => Invalid(errF),
               Invalid: (errT) => Invalid(errF.Concat(errT))));


      public static Validation<Func<T2, R>> Apply<T1, T2, R>
         (this Validation<Func<T1, T2, R>> @this, Validation<T1> arg)
         => Apply(@this.Map(F.Curry), arg);

      public static Validation<Func<T2, T3, R>> Apply<T1, T2, T3, R>
         (this Validation<Func<T1, T2, T3, R>> @this, Validation<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Validation<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>
         (this Validation<Func<T1, T2, T3, T4, R>> @this, Validation<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Validation<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>
         (this Validation<Func<T1, T2, T3, T4, T5, R>> @this, Validation<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);
      
      public static Validation<Func<T2, T3, T4, T5, T6, R>> Apply<T1, T2, T3, T4, T5, T6, R>
         (this Validation<Func<T1, T2, T3, T4, T5, T6, R>> @this, Validation<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);
      
      public static Validation<Func<T2, T3, T4, T5, T6, T7, R>> Apply<T1, T2, T3, T4, T5, T6, T7, R>
         (this Validation<Func<T1, T2, T3, T4, T5, T6, T7, R>> @this, Validation<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);
      
      public static Validation<Func<T2, T3, T4, T5, T6, T7, T8, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R>
         (this Validation<Func<T1, T2, T3, T4, T5, T6, T7, T8, R>> @this, Validation<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Validation<Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
         (this Validation<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> @this, Validation<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Validation<R> Map<T, R>
         (this Validation<T> @this, Func<T, R> f)
         => @this.Match
         (
            Valid: t => Valid(f(t)),
            Invalid: errs => Invalid(errs)
         );

      public static Validation<Func<T2, R>> Map<T1, T2, R>(this Validation<T1> @this
         , Func<T1, T2, R> func)
          => @this.Map(func.Curry());

      public static Validation<Unit> ForEach<R>
         (this Validation<R> @this, Action<R> act)
         => Map(@this, act.ToFunc());

      public static Validation<T> Do<T>
         (this Validation<T> @this, Action<T> action)
      {
         @this.ForEach(action);
         return @this;
      }

      public static Validation<R> Bind<T, R>
         (this Validation<T> val, Func<T, Validation<R>> f)
          => val.Match(
             Invalid: (err) => Invalid(err),
             Valid: (r) => f(r));
      

      // LINQ

      public static Validation<R> Select<T, R>(this Validation<T> @this
         , Func<T, R> map) => @this.Map(map);

      public static Validation<RR> SelectMany<T, R, RR>(this Validation<T> @this
         , Func<T, Validation<R>> bind, Func<T, R, RR> project)
         => @this.Match(
            Invalid: (err) => Invalid(err),
            Valid: (t) => bind(t).Match(
               Invalid: (err) => Invalid(err),
               Valid: (r) => Valid(project(t, r))));
   }
}

namespace LaYumba.Functional
{
   using static F;

   public static partial class F
   {
      // wrap the given value into a Some
      public static Option<T> Some<T>([NotNull] T? value) // NotNull: `value` is guaranteed to never be null if the method returns without throwing an exception
         => new(value ?? throw new ArgumentNullException(nameof(value)));

      // the None value
      public static NoneType None => default;
   }

   // a NoneType can be implicitely converted to an Option<T> for any type T
   public struct NoneType {}

   public struct Option<T> : IEquatable<NoneType>, IEquatable<Option<T>>
   {
      readonly T? value;
      readonly bool isSome;
      bool isNone => !isSome;

      internal Option(T t) => (isSome, value) = (true, t);

      public static implicit operator Option<T>(NoneType _) => default;

      public static implicit operator Option<T>(T t)
         => t is null ? None : new Option<T>(t);

      public R Match<R>(Func<R> None, Func<T, R> Some)
         => isSome ? Some(value!) : None();

      public IEnumerable<T> AsEnumerable()
      {
         if (isSome) yield return value!;
      }

      public static bool operator true(Option<T> @this) => @this.isSome;
      public static bool operator false(Option<T> @this) => @this.isNone;
      public static Option<T> operator | (Option<T> l, Option<T> r) => l.isSome ? l : r;

      // equality operators

      public bool Equals(Option<T> other)
         => this.isSome == other.isSome
         && (this.isNone || this.value!.Equals(other.value));

      public bool Equals(NoneType _) => isNone;

      public static bool operator ==(Option<T> @this, Option<T> other) => @this.Equals(other);
      public static bool operator !=(Option<T> @this, Option<T> other) => !(@this == other);

      public override bool Equals(object? other)
         => other is Option<T> option && this.Equals(option);

      public override int GetHashCode()
         => isNone ? 0 : value!.GetHashCode();

      public override string ToString() => isSome ? $"Some({value})" : "None";
   }

   public static class OptionExt
   {
      public static Option<R> Apply<T, R>
         (this Option<Func<T, R>> @this, Option<T> arg)
         => @this.Match(
            () => None,
            (func) => arg.Match(
               () => None,
               (val) => Some(func(val))));

      public static Option<Func<T2, R>> Apply<T1, T2, R>
         (this Option<Func<T1, T2, R>> @this, Option<T1> arg)
         => Apply(@this.Map(F.Curry), arg);

      public static Option<Func<T2, T3, R>> Apply<T1, T2, T3, R>
         (this Option<Func<T1, T2, T3, R>> @this, Option<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Option<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>
         (this Option<Func<T1, T2, T3, T4, R>> @this, Option<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Option<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>
         (this Option<Func<T1, T2, T3, T4, T5, R>> @this, Option<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Option<Func<T2, T3, T4, T5, T6, R>> Apply<T1, T2, T3, T4, T5, T6, R>
         (this Option<Func<T1, T2, T3, T4, T5, T6, R>> @this, Option<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Option<Func<T2, T3, T4, T5, T6, T7, R>> Apply<T1, T2, T3, T4, T5, T6, T7, R>
         (this Option<Func<T1, T2, T3, T4, T5, T6, T7, R>> @this, Option<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Option<Func<T2, T3, T4, T5, T6, T7, T8, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R>
         (this Option<Func<T1, T2, T3, T4, T5, T6, T7, T8, R>> @this, Option<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Option<Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
         (this Option<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> @this, Option<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Option<R> Bind<T, R>
         (this Option<T> optT, Func<T, Option<R>> f)
          => optT.Match(
             () => None,
             (t) => f(t));

      public static IEnumerable<R> Bind<T, R>
         (this Option<T> @this, Func<T, IEnumerable<R>> func)
          => @this.AsEnumerable().Bind(func);

      public static Option<Unit> ForEach<T>(this Option<T> @this, Action<T> action)
         => Map(@this, action.ToFunc());

      public static Option<R> Map<T, R>
         (this NoneType _, Func<T, R> f)
         => None;

      public static Option<R> Map<T, R>
         (this Option<T> optT, Func<T, R> f)
         => optT.Match(
            () => None,
            (t) => Some(f(t)));

      public static Option<Func<T2, R>> Map<T1, T2, R>
         (this Option<T1> @this, Func<T1, T2, R> func)
          => @this.Map(func.Curry());

      public static Option<Func<T2, T3, R>> Map<T1, T2, T3, R>
         (this Option<T1> @this, Func<T1, T2, T3, R> func)
          => @this.Map(func.CurryFirst());

      public static IEnumerable<Option<R>> Traverse<T, R>(this Option<T> @this
         , Func<T, IEnumerable<R>> func)
         => @this.Match(
            () => List((Option<R>)None),
            (t) => func(t).Map(r => Some(r)));

      // utilities

      public static Unit Match<T>(this Option<T> @this, Action None, Action<T> Some)
          => @this.Match(None.ToFunc(), Some.ToFunc());

      internal static bool IsSome<T>(this Option<T> @this)
         => @this.Match(
            () => false,
            (_) => true);

      internal static T ValueUnsafe<T>(this Option<T> @this)
         => @this.Match(
            () => { throw new InvalidOperationException(); },
            (t) => t);

      public static T GetOrElse<T>(this Option<T> opt, T defaultValue)
         => opt.Match( 
            () => defaultValue,
            (t) => t);

      public static T GetOrElse<T>(this Option<T> opt, Func<T> fallback)
         => opt.Match(
            () => fallback(),
            (t) => t);

      public static Task<T> GetOrElse<T>(this Option<T> opt, Func<Task<T>> fallback)
         => opt.Match(
            () => fallback(),
            (t) => Async(t));

      public static Option<T> OrElse<T>(this Option<T> left, Option<T> right)
         => left.Match( 
            () => right,
            (_) => left);

      public static Option<T> OrElse<T>(this Option<T> left, Func<Option<T>> right)
         => left.Match(
            () => right(), 
            (_) => left);

      public static Validation<T> ToValidation<T>(this Option<T> opt, Error error)
         => opt.Match(
            () => Invalid(error),
            (t) => Valid(t));

      public static Validation<T> ToValidation<T>(this Option<T> opt, Func<Error> error)
         => opt.Match(
            () => Invalid(error()),
            (t) => Valid(t));

      // LINQ

      public static Option<R> Select<T, R>(this Option<T> @this, Func<T, R> func)
         => @this.Map(func);

      public static Option<T> Where<T>
         (this Option<T> optT, Func<T, bool> predicate)
         => optT.Match(
            () => None,
            (t) => predicate(t) ? optT : None);

      public static Option<RR> SelectMany<T, R, RR>
         (this Option<T> opt, Func<T, Option<R>> bind, Func<T, R, RR> project)
         => opt.Match(
            () => None,
            (t) => bind(t).Match(
               () => None,
               (r) => Some(project(t, r))));
   }
}

namespace LaYumba.Functional
{
   using static F;

   public static class FuncExt
   {
      public static Func<T> ToNullary<T>(this Func<Unit, T> f)
          => () => f(Unit());

      public static Func<T1, R> Compose<T1, T2, R>(this Func<T2, R> g, Func<T1, T2> f)
         => x => g(f(x));

      public static Func<T, bool> Negate<T>(this Func<T, bool> pred) => t => !pred(t);

      public static Func<T2, R> Apply<T1, T2, R>(this Func<T1, T2, R> func, T1 t1)
          => t2 => func(t1, t2);

      public static Func<T2, T3, R> Apply<T1, T2, T3, R>(this Func<T1, T2, T3, R> func, T1 t1)
          => (t2, t3) => func(t1, t2, t3);

      public static Func<T2, T3, T4, R> Apply<T1, T2, T3, T4, R>(this Func<T1, T2, T3, T4, R> func, T1 t1)
          => (t2, t3, t4) => func(t1, t2, t3, t4);

      public static Func<T2, T3, T4, T5, R> Apply<T1, T2, T3, T4, T5, R>(this Func<T1, T2, T3, T4, T5, R> func, T1 t1)
          => (t2, t3, t4, t5) => func(t1, t2, t3, t4, t5);

      public static Func<T2, T3, T4, T5, T6, R> Apply<T1, T2, T3, T4, T5, T6, R>(this Func<T1, T2, T3, T4, T5, T6, R> func, T1 t1)
          => (t2, t3, t4, t5, t6) => func(t1, t2, t3, t4, t5, t6);

      public static Func<T2, T3, T4, T5, T6, T7, R> Apply<T1, T2, T3, T4, T5, T6, T7, R>(this Func<T1, T2, T3, T4, T5, T6, T7, R> func, T1 t1)
          => (t2, t3, t4, t5, t6, t7) => func(t1, t2, t3, t4, t5, t6, t7);

      public static Func<T2, T3, T4, T5, T6, T7, T8, R> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, R> func, T1 t1)
          => (t2, t3, t4, t5, t6, t7, t8) => func(t1, t2, t3, t4, t5, t6, t7, t8);

      public static Func<T2, T3, T4, T5, T6, T7, T8, T9, R> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> func, T1 t1)
          => (t2, t3, t4, t5, t6, t7, t8, t9) => func(t1, t2, t3, t4, t5, t6, t7, t8, t9);

      public static Func<I1, I2, R> Map<I1, I2, T, R>(this Func<I1, I2, T> @this, Func<T, R> func)
         => (i1, i2) => func(@this(i1, i2));
   }


   // Env -> T (aka. Reader)
   public static class FuncTRExt
   {
      public static Func<Env, R> Map<Env, T, R>
         (this Func<Env, T> f, Func<T, R> g)
         => x => g(f(x));

      public static Func<Env, R> Bind<Env, T, R>
         (this Func<Env, T> f, Func<T, Func<Env, R>> g)
         => env => g(f(env))(env);

      // same as above, in uncurried form
      public static Func<Env, R> Bind<Env, T, R>
         (this Func<Env, T> f, Func<T, Env, R> g)
         => env => g(f(env), env);


      // LINQ

      public static Func<Env, R> Select<Env, T, R>(this Func<Env, T> f, Func<T, R> g) => f.Map(g);

      public static Func<Env, P> SelectMany<Env, T, R, P>
         (this Func<Env, T> f, Func<T, Func<Env, R>> bind, Func<T, R, P> project)
         => env =>
         {
            var t = f(env);
            var r = bind(t)(env);
            return project(t, r);
         };
   }
}

namespace LaYumba.Functional
{
   using static F;

   public static partial class F
   {
      public static IEnumerable<T> List<T>(params T[] items) => items.ToImmutableList();
   }

   public static class EnumerableExt
   {
      public static Func<T, IEnumerable<T>> Return<T>() => t => List(t);

      public static IEnumerable<T> Append<T>(this IEnumerable<T> source
         , params T[] ts) => source.Concat(ts);

      static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T val)
      {
         yield return val;
         foreach (T t in source) yield return t;
      }

      public static (IEnumerable<T> Passed, IEnumerable<T> Failed) Partition<T>
      (
         this IEnumerable<T> source,
         Func<T, bool> predicate
      )
      {
         var grouped = source.GroupBy(predicate);
         return
         (
            Passed: grouped.Where(g => g.Key).FirstOrDefault(Enumerable.Empty<T>()),
            Failed: grouped.Where(g => !g.Key).FirstOrDefault(Enumerable.Empty<T>())
         );
      }

      public static Option<T> Find<T>(this IEnumerable<T> source, Func<T, bool> predicate)
         => source.Where(predicate).Head();

      public static IEnumerable<Unit> ForEach<T>
         (this IEnumerable<T> ts, Action<T> action)
         => ts.Map(action.ToFunc()).ToImmutableList();

      public static IEnumerable<R> Map_InTermsOfFold<T, R>
         (this IEnumerable<T> ts, Func<T, R> f)
          => ts.Aggregate(List<R>()
             , (rs, t) => rs.Append(f(t)));

      static IEnumerable<T> Where_InTermsOfFold<T>
         (this IEnumerable<T> @this, Func<T, bool> predicate)
          => @this.Aggregate(List<T>()
             , (ts, t) => predicate(t) ? ts.Append(t) : ts);

      static IEnumerable<R> Bind_InTermsOfFold<T, R>
         (this IEnumerable<T> ts, Func<T, IEnumerable<R>> f)
         => ts.Aggregate(List<R>()
            , (rs, t) => rs.Concat(f(t)));

      public static IEnumerable<R> Map<T, R>
         (this IEnumerable<T> list, Func<T, R> func)
          => list.Select(func);

      public static R Match<T, R>(this IEnumerable<T> list
         , Func<R> Empty, Func<T, IEnumerable<T>, R> Otherwise) 
         => list.Head().Match(
            None: Empty,
            Some: head => Otherwise(head, list.Skip(1)));

      public static Option<T> Head<T>(this IEnumerable<T> list)
      {
         if (list == null) return None;
         var enumerator = list.GetEnumerator();
         return enumerator.MoveNext() ? Some(enumerator.Current) : None;
      }

      public static IEnumerable<Func<T2, R>> Map<T1, T2, R>(this IEnumerable<T1> list
         , Func<T1, T2, R> func)
         => list.Map(func.Curry());

      public static IEnumerable<Func<T2, Func<T3, R>>> 
      Map<T1, T2, T3, R>(this IEnumerable<T1> opt, Func<T1, T2, T3, R> func)
         => opt.Map(func.Curry());

      public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> list, Func<T, IEnumerable<R>> func)
          => list.SelectMany(func);

      public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> list)
          => list.SelectMany(x => x);

      public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> list, Func<T, Option<R>> func)
          => list.Bind(t => func(t).AsEnumerable());

      // LINQ

      public static IEnumerable<RR> SelectMany<T, R, RR>
         (this IEnumerable<T> source
         , Func<T, Option<R>> bind
         , Func<T, R, RR> project)
         => from t in source
            let opt = bind(t)
            where opt.IsSome()
            select project(t, opt.ValueUnsafe());

      static IEnumerable<T> TakeWhile<T>(this IEnumerable<T> @this, Func<T, bool> pred)
      {
         foreach (var item in @this)
         {
            if (pred(item)) yield return item;
            else yield break;
         }
      }

      static IEnumerable<T> DropWhile<T>(this IEnumerable<T> @this, Func<T, bool> pred)
      {
         bool clean = true;
         foreach (var item in @this)
         {
            if (!clean || !pred(item))
            {
               yield return item;
               clean = false;
            }
         }
      }
   }
}

namespace LaYumba.Functional
{
   public static partial class F
   {
      public static Unit Unit() => default;

      // function manipulation 

      public static Func<T1, Func<T2, R>> Curry<T1, T2, R>(this Func<T1, T2, R> func)
          => t1 => t2 => func(t1, t2);

      public static Func<T1, Func<T2, Func<T3, R>>> Curry<T1, T2, T3, R>(this Func<T1, T2, T3, R> func)
          => t1 => t2 => t3 => func(t1, t2, t3);

      public static Func<T1, Func<T2, T3, R>> CurryFirst<T1, T2, T3, R>
         (this Func<T1, T2, T3, R> @this) => t1 => (t2, t3) => @this(t1, t2, t3);

      public static Func<T1, Func<T2, T3, T4, R>> CurryFirst<T1, T2, T3, T4, R>
         (this Func<T1, T2, T3, T4, R> @this) => t1 => (t2, t3, t4) => @this(t1, t2, t3, t4);

      public static Func<T1, Func<T2, T3, T4, T5, R>> CurryFirst<T1, T2, T3, T4, T5, R>
         (this Func<T1, T2, T3, T4, T5, R> @this) => t1 => (t2, t3, t4, t5) => @this(t1, t2, t3, t4, t5);

      public static Func<T1, Func<T2, T3, T4, T5, T6, R>> CurryFirst<T1, T2, T3, T4, T5, T6, R>
         (this Func<T1, T2, T3, T4, T5, T6, R> @this) => t1 => (t2, t3, t4, t5, t6) => @this(t1, t2, t3, t4, t5, t6);

      public static Func<T1, Func<T2, T3, T4, T5, T6, T7, R>> CurryFirst<T1, T2, T3, T4, T5, T6, T7, R>
         (this Func<T1, T2, T3, T4, T5, T6, T7, R> @this) => t1 => (t2, t3, t4, t5, t6, t7) => @this(t1, t2, t3, t4, t5, t6, t7);

      public static Func<T1, Func<T2, T3, T4, T5, T6, T7, T8, R>> CurryFirst<T1, T2, T3, T4, T5, T6, T7, T8, R>
         (this Func<T1, T2, T3, T4, T5, T6, T7, T8, R> @this) => t1 => (t2, t3, t4, t5, t6, t7, t8) => @this(t1, t2, t3, t4, t5, t6, t7, t8);

      public static Func<T1, Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> CurryFirst<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
         (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> @this) => t1 => (t2, t3, t4, t5, t6, t7, t8, t9) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9);

      public static Func<T, T> Tap<T>(Action<T> act) 
         => x => { act(x); return x; };
      
      public static R Pipe<T, R>(this T @this, Func<T, R> func) => func(@this);
      
      /// <summary>
      /// Pipes the input value in the given Action, i.e. invokes the given Action on the given value.
      /// returning the input value. Not really a genuine implementation of pipe, since it combines pipe with Tap.
      /// </summary>
      public static T Pipe<T>(this T input, Action<T> func) => Tap(func)(input);

      // Using
      public static R Using<TDisp, R>(TDisp disposable
         , Func<TDisp, R> func) where TDisp : IDisposable
      {
         using (var disp = disposable) return func(disp);
      }

      public static Unit Using<TDisp>(TDisp disposable
         , Action<TDisp> act) where TDisp : IDisposable 
         => Using(disposable, act.ToFunc());
      
      public static R Using<TDisp, R>(Func<TDisp> createDisposable
         , Func<TDisp, R> func) where TDisp : IDisposable
      {
         using var disp = createDisposable();
         return func(disp);
      }

      public static Unit Using<TDisp>(Func<TDisp> createDisposable
         , Action<TDisp> action) where TDisp : IDisposable
         => Using(createDisposable, action.ToFunc());

      // Range
      public static IEnumerable<char> Range(char from, char to)
      {
         for (var i = from; i <= to; i++) yield return i;
      }

      public static IEnumerable<int> Range(int from, int to)
      {
         for (var i = from; i <= to; i++) yield return i;
      }
   }
}

namespace LaYumba.Functional
{
   public static partial class F
   {
      public static Exceptional<T> Exceptional<T>(T t) => new (t);
   }

   public struct Exceptional<T>
   {
      private Exception? Ex { get; }
      private T? Value { get; }
      
      private bool IsSuccess { get; }
      private bool IsException => !IsSuccess;

      internal Exceptional(Exception ex)
      {
         IsSuccess = false;
         Ex = ex ?? throw new ArgumentNullException(nameof(ex));
         Value = default;
      }

      internal Exceptional(T value)
      {
         IsSuccess = true;
         Value = value ?? throw new ArgumentNullException(nameof(value));
         Ex = default;
      }

      public static implicit operator Exceptional<T>(Exception ex) => new (ex);
      public static implicit operator Exceptional<T>(T t) => new (t);

      public TR Match<TR>(Func<Exception, TR> Exception, Func<T, TR> Success)
         => this.IsException ? Exception(Ex!) : Success(Value!);

      public Unit Match(Action<Exception> Exception, Action<T> Success)
         => Match(Exception.ToFunc(), Success.ToFunc());

      public override string ToString() 
         => Match(
            ex => $"Exception({ex.Message})",
            t => $"Success({t})");
   }

   public static class Exceptional
   {
      // creating a new Exceptional

      public static Func<T, Exceptional<T>> Return<T>()
         => t => t;

      public static Exceptional<R> Of<R>(Exception left)
         => new Exceptional<R>(left);

      public static Exceptional<R> Of<R>(R right)
         => new Exceptional<R>(right);

      // applicative

      public static Exceptional<R> Apply<T, R>
         (this Exceptional<Func<T, R>> @this, Exceptional<T> arg)
         => @this.Match(
            Exception: ex => ex,
            Success: func => arg.Match(
               Exception: ex => ex,
               Success: t => F.Exceptional(func(t))));

      public static Exceptional<Func<T2, R>> Apply<T1, T2, R>
         (this Exceptional<Func<T1, T2, R>> @this, Exceptional<T1> arg)
         => Apply(@this.Map(F.Curry), arg);

      public static Exceptional<Func<T2, T3, R>> Apply<T1, T2, T3, R>
         (this Exceptional<Func<T1, T2, T3, R>> @this, Exceptional<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Exceptional<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>
         (this Exceptional<Func<T1, T2, T3, T4, R>> @this, Exceptional<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Exceptional<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>
         (this Exceptional<Func<T1, T2, T3, T4, T5, R>> @this, Exceptional<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Exceptional<Func<T2, T3, T4, T5, T6, R>> Apply<T1, T2, T3, T4, T5, T6, R>
         (this Exceptional<Func<T1, T2, T3, T4, T5, T6, R>> @this, Exceptional<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Exceptional<Func<T2, T3, T4, T5, T6, T7, R>> Apply<T1, T2, T3, T4, T5, T6, T7, R>
         (this Exceptional<Func<T1, T2, T3, T4, T5, T6, T7, R>> @this, Exceptional<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Exceptional<Func<T2, T3, T4, T5, T6, T7, T8, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R>
         (this Exceptional<Func<T1, T2, T3, T4, T5, T6, T7, T8, R>> @this, Exceptional<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Exceptional<Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
         (this Exceptional<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> @this, Exceptional<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      // functor

      public static Exceptional<RR> Map<R, RR>
      (
         this Exceptional<R> @this,
         Func<R, RR> f
      )
      => @this.Match
      (
         Exception: ex => new Exceptional<RR>(ex),
         Success: r => f(r)
      );

      public static Exceptional<Unit> ForEach<R>(this Exceptional<R> @this, Action<R> act)
         => Map(@this, act.ToFunc());

      public static Exceptional<RR> Bind<R, RR>
      (
         this Exceptional<R> @this,
         Func<R, Exceptional<RR>> f
      )
      => @this.Match
      (
         Exception: ex => new Exceptional<RR>(ex),
         Success: r => f(r)
      );
      
      // LINQ

      public static Exceptional<R> Select<T, R>(this Exceptional<T> @this
         , Func<T, R> map) => @this.Map(map);

      public static Exceptional<RR> SelectMany<T, R, RR>
      (
         this Exceptional<T> @this,
         Func<T, Exceptional<R>> bind,
         Func<T, R, RR> project
      )
      => @this.Match
      (
         Exception: ex => new Exceptional<RR>(ex),
         Success: t => bind(t).Match
         (
            Exception: ex => new Exceptional<RR>(ex),
            Success: r => project(t, r)
         )
      );
   }
}

namespace LaYumba.Functional
{
   public static partial class F
   {
      public static Error Error(string message) => new Error(message);
   }

   public record Error(string Message)
   {
      public override string ToString() => Message;

      public static implicit operator Error(string m) => new(m);
   }
}

namespace LaYumba.Functional
{
   using static F;

   public static partial class F
   {
      public static Either.Left<L> Left<L>(L l) => new Either.Left<L>(l);
      public static Either.Right<R> Right<R>(R r) => new Either.Right<R>(r);
   }

   public struct Either<L, R>
   {
      private L? Left { get; }
      private R? Right { get; }

      private bool IsRight { get; }
      private bool IsLeft => !IsRight;

      internal Either(L left)
         => (IsRight, Left, Right)
         = (false, left ?? throw new ArgumentNullException(nameof(left)), default);

      internal Either(R right)
         => (IsRight, Left, Right)
         = (true, default, right ?? throw new ArgumentNullException(nameof(right)));

      public static implicit operator Either<L, R>(L left) => new Either<L, R>(left);
      public static implicit operator Either<L, R>(R right) => new Either<L, R>(right);

      public static implicit operator Either<L, R>(Either.Left<L> left) => new Either<L, R>(left.Value);
      public static implicit operator Either<L, R>(Either.Right<R> right) => new Either<L, R>(right.Value);

      public TR Match<TR>(Func<L, TR> Left, Func<R, TR> Right)
         => IsLeft ? Left(this.Left!) : Right(this.Right!);

      public Unit Match(Action<L> Left, Action<R> Right)
         => Match(Left.ToFunc(), Right.ToFunc());

      public IEnumerator<R> AsEnumerable()
      {
         if (IsRight) yield return Right!;
      }

      public override string ToString() => Match(l => $"Left({l})", r => $"Right({r})");
   }

   public static class Either
   {
      public struct Left<L>
      {
         internal L Value { get; }
         internal Left(L value) { Value = value; }

         public override string ToString() => $"Left({Value})";
      }

      public struct Right<R>
      {
         internal R Value { get; }
         internal Right(R value) { Value = value; }

         public override string ToString() => $"Right({Value})";

         public Right<RR> Map<L, RR>(Func<R, RR> f) => Right(f(Value));
         public Either<L, RR> Bind<L, RR>(Func<R, Either<L, RR>> f) => f(Value);
      }
   }

   public static class EitherExt
   {
      public static Either<L, RR> Map<L, R, RR>
         (this Either<L, R> @this, Func<R, RR> f)
         => @this.Match<Either<L, RR>>(
            l => Left(l),
            r => Right(f(r)));

      public static Either<LL, RR> Map<L, LL, R, RR>
         (this Either<L, R> @this, Func<L, LL> Left, Func<R, RR> Right)
         => @this.Match<Either<LL, RR>>
            (
               l => F.Left(Left(l)),
               r => F.Right(Right(r))
            );

      public static Either<L, Unit> ForEach<L, R>
         (this Either<L, R> @this, Action<R> act)
         => Map(@this, act.ToFunc());

      public static Either<L, RR> Bind<L, R, RR>
         (this Either<L, R> @this, Func<R, Either<L, RR>> f)
         => @this.Match(
            l => Left(l),
            r => f(r));

      // Applicative

      public static Either<L, RR> Apply<L, R, RR>
      (
         this Either<L, Func<R, RR>> @this,
         Either<L, R> valT
      )
      => @this.Match
      (
         Left: (errF) => Left(errF),
         Right: (f) => valT.Match<Either<L, RR>>
         (
            Right: (t) => Right(f(t)),
            Left: (err) => Left(err)
         )
      );

      public static Either<L, Func<T2, R>> Apply<L, T1, T2, R>
         (this Either<L, Func<T1, T2, R>> @this, Either<L, T1> arg)
         => Apply(@this.Map(F.Curry), arg);

      public static Either<L, Func<T2, T3, R>> Apply<L, T1, T2, T3, R>
         (this Either<L, Func<T1, T2, T3, R>> @this, Either<L, T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Either<L, Func<T2, T3, T4, R>> Apply<L, T1, T2, T3, T4, R>
         (this Either<L, Func<T1, T2, T3, T4, R>> @this, Either<L, T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Either<L, Func<T2, T3, T4, T5, R>> Apply<L, T1, T2, T3, T4, T5, R>
         (this Either<L, Func<T1, T2, T3, T4, T5, R>> @this, Either<L, T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Either<L, Func<T2, T3, T4, T5, T6, R>> Apply<L, T1, T2, T3, T4, T5, T6, R>
         (this Either<L, Func<T1, T2, T3, T4, T5, T6, R>> @this, Either<L, T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Either<L, Func<T2, T3, T4, T5, T6, T7, R>> Apply<L, T1, T2, T3, T4, T5, T6, T7, R>
         (this Either<L, Func<T1, T2, T3, T4, T5, T6, T7, R>> @this, Either<L, T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Either<L, Func<T2, T3, T4, T5, T6, T7, T8, R>> Apply<L, T1, T2, T3, T4, T5, T6, T7, T8, R>
         (this Either<L, Func<T1, T2, T3, T4, T5, T6, T7, T8, R>> @this, Either<L, T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Either<L, Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Apply<L, T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
         (this Either<L, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> @this, Either<L, T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);


      // LINQ query pattern

      public static Either<L, R> Select<L, T, R>
      (
         this Either<L, T> @this,
         Func<T, R> f
      )
      => @this.Map(f);

      public static Either<L, RR> SelectMany<L, T, R, RR>
      (
         this Either<L, T> @this,
         Func<T, Either<L, R>> bind,
         Func<T, R, RR> project
      )
      => @this.Match
      (
         Left: l => Left(l),
         Right: t => bind(t).Match<Either<L, RR>>
         (
            Left: l => Left(l),
            Right: r => project(t, r)
         )
      );
   }
}
