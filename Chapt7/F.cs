using System.Collections.Immutable;
using Unit = System.ValueTuple;
using static F;
using static F2;

public static class F
{
    public static Option<T> Some<T>(T value) => new Option<T>(value);
    public static NoneType None => default;
}


public static class F2
{
    public static IEnumerable<R> Map<T, R>(this IEnumerable<T> ts, Func<T, R> f)
    {
        foreach (var t in ts)
            yield return f(t);
    }

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

    public static IEnumerable<Unit> ForEach<T>
        (this IEnumerable<T> ts, Action<T> action)
        => ts.Map(action.ToFunc()).ToImmutableList();

    public static Option<R> Map<T, R>(this Option<T> optT, Func<T, R> f)
        => optT.Match
        (
            () => None,
            (t) => Some(f(t))
        );

    public static Option<Unit> ForEach<T>
        (this Option<T> opt, Action<T> action)
        => Map(opt, action.ToFunc());

    public static Option<R> Bind<T, R>(this Option<T> optT, Func<T, Option<R>> f)
        => optT.Match(() => None, f);

    public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> ts, Func<T, IEnumerable<R>> f)
    {
        foreach (T t in ts)
        foreach (R r in f(t))
            yield return r;
    }
    
    public static IEnumerable<T> List<T>(params T[] items)
        => items.ToImmutableList();

    public static Option<T> Where<T>(this Option<T> optT, Func<T, bool> pred)
        => optT.Match(() => None, t => pred(t) ? optT : None);

    public static IEnumerable<R> Bind<T, R>(this IEnumerable<T> ts, Func<T, Option<R>> f)
        => ts.Bind(t => f(t).AsEnumerable());
    
    public static IEnumerable<R> Bind<T, R>(this Option<T> optT, Func<T, IEnumerable<R>> f)
        => optT.AsEnumerable().Bind(f);
}

public struct NoneType
{
}


public struct Option<T>
{
    private readonly T? _value;
    private readonly bool _isSome;
    
    public Option(T value)
    {
        _value = value ?? throw new ArgumentNullException();
        _isSome = true;
    }

    public override string ToString() => _isSome ? $"Some({_value})" : "None";
    
    public static implicit operator Option<T>(NoneType _)
        => default;

    public static implicit operator Option<T>(T value)
        => value is null ? None : Some(value);  
    public R Match<R>(Func<R> None, Func<T, R> Some)
        => _isSome ? Some(_value!) : None();

    public IEnumerable<T> AsEnumerable()
    {
        if (_isSome) yield return _value!;
    }
}