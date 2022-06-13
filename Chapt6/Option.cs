using static F;

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