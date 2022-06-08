namespace Chapt4;

using Unit = System.ValueTuple;

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
}
