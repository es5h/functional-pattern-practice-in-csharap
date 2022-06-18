using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Unit = System.ValueTuple;

namespace Functional.F1
{
   using static F;

   public static partial class F
   {
      public static Either.Left<L> Left<L>(L l) => new Either.Left<L>(l);
      public static Either.Right<R> Right<R>(R r) => new Either.Right<R>(r);
      public static Unit Unit() => default;

      public static Validation<T> Valid<T>(T value)
         => new(value ?? throw new ArgumentNullException(nameof(value)));

      // create a Validation in the Invalid state
      public static Validation.Invalid Invalid(params Error[] errors) => new(errors);
      public static Validation<T> Invalid<T>(params Error[] errors) => new Validation.Invalid(errors);
      public static Validation.Invalid Invalid(IEnumerable<Error> errors) => new(errors);
      public static Validation<T> Invalid<T>(IEnumerable<Error> errors) => new Validation.Invalid(errors);

      // wrap the given value into a Some
      public static Option<T>
         Some<T>([NotNull] T? value) // NotNull: `value` is guaranteed to never be null if the method returns without throwing an exception
         => new(value ?? throw new ArgumentNullException(nameof(value)));

      // the None value
      public static NoneType None => default;

      public static IEnumerable<T> List<T>(params T[] items) => items.ToImmutableList();
   }

   public struct NoneType
   {
   }

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
      public static Option<T> operator |(Option<T> l, Option<T> r) => l.isSome ? l : r;

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

      public static IEnumerable<Option<R>> Traverse<T, R>(this Option<T> @this
         , Func<T, IEnumerable<R>> func)
         => @this.Match(
            () => List((Option<R>) None),
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
         => new Validation<T>(new[] {error});

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

         public Invalid(IEnumerable<Error> errors)
         {
            Errors = errors;
         }
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

      public static Validation<R> Map<T, R>
         (this Validation<T> @this, Func<T, R> f)
         => @this.Match
         (
            Valid: t => Valid(f(t)),
            Invalid: errs => Invalid(errs)
         );

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

   public static class ActionExt
   {
      public static Func<Unit> ToFunc(this Action action)
         => () =>
         {
            action();
            return default;
         };

      public static Func<T, Unit> ToFunc<T>(this Action<T> action)
         => t =>
         {
            action(t);
            return default;
         };

      public static Func<T1, T2, Unit> ToFunc<T1, T2>(this Action<T1, T2> action)
         => (T1 t1, T2 t2) =>
         {
            action(t1, t2);
            return default;
         };

      public static Func<Task<Unit>> ToFunc(this Func<Task> f)
         => async () =>
         {
            await f();
            return Unit();
         };

      public static Func<T, Task<Unit>> ToFunc<T>(this Func<T, Task> f)
         => async (t) =>
         {
            await f(t);
            return Unit();
         };

      public static Func<T1, T2, Task<Unit>> ToFunc<T1, T2>(this Func<T1, T2, Task> f)
         => async (t1, t2) =>
         {
            await f(t1, t2);
            return Unit();
         };
   }

   public static class FuncTExt
   {
      public static Func<R> Map<T, R>
         (this Func<T> f, Func<T, R> g)
         => () => g(f());

      public static Func<R> Bind<T, R>
         (this Func<T> f, Func<T, Func<R>> g)
         => () => g(f())();

      // LINQ

      public static Func<R> Select<T, R>(this Func<T> @this
         , Func<T, R> func) => @this.Map(func);

      public static Func<P> SelectMany<T, R, P>(this Func<T> @this
         , Func<T, Func<R>> bind, Func<T, R, P> project)
         => () =>
         {
            T t = @this();
            R r = bind(t)();
            return project(t, r);
         };
   }

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

         internal Left(L value)
         {
            Value = value;
         }

         public override string ToString() => $"Left({Value})";
      }

      public struct Right<R>
      {
         internal R Value { get; }

         internal Right(R value)
         {
            Value = value;
         }

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