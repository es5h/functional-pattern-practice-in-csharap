using System.Diagnostics;
using Chapt4;
using static System.Console;
using Unit = System.ValueTuple;

// Exmample 1.

// Runtime Error
Risk CalculateRiskProfile(dynamic age)
    => (age < 60) ? Risk.Low : Risk.Medium;

WriteLine(CalculateRiskProfile(20));
// WriteLine(CalculateRiskProfile("Hi"));

// Compile time Error
Risk CalculateRiskProfile3(int age)
    => (age < 60) ? Risk.Low : Risk.Medium;

WriteLine(CalculateRiskProfile3(20));
// WriteLine(CalculateRiskProfile3("Hi")); => Compiletime error


// Example 1-1. add validation : 밸리데이션 로직이 필요하다면? 아래와 같이 로직을 추가해야한다.
Risk CalculateRiskProfile2(int age)
{
    if (age < 0 || 120 < age)
        throw new ArgumentException($"{age} is not a valid age");
    return (age < 60) ? Risk.Low : Risk.Medium;
}
// 저자는 두 포인트를 지적
// 1) validation fails 를 위한 테스트 코드 추가작성이 필요
// 2) Risk를 계산해야하는데 validation 로직이 추가되서 srp 위반
// validation 부분은 코드가 중복될 가능성 높음 => 보통 중복이 되면 srp 위반됐을 가능성 시사

// Exampe 1-2 custom type / struct type 하기

Risk CalculateRiskProfile4(Age age)
    => (age.Value < 60) ? Risk.Low : Risk.Medium;

WriteLine(CalculateRiskProfile3(30));

// 유효한 값만 들어간다 / 런타임에러 사라진다 / Age TYpe 을 쓰면 중복 제거 가능
// Age 내부 structure를 클라이언트 코드가 알게 된다. 비교가 int 비교이므로 컨셉이 명확하지 않다. 

// Example 1-3 custom type / struct type 하기
Risk CalculateRiskProfile5(Age2 age)
    => (age < 600) ? Risk.Low : Risk.Medium;

// Example 2. Why void isn't ideal;
Instrumentation.Time("hi", () => WriteLine("hi"));

// 위 코드 보면 Action, Func<T> 중복 코드 사용됨.
// Func<Void>로 될가? 안됨. Void 가 compiler에 의해 특별하게 다뤄지는 타입(System.Void 타입)
// empty tuple = Unit 을 써보자
// *Void 는 empty set 을 상징함.

// Example 2-1, Action을 Func<Unit> 으로 바궈사용
Instrumentation2.Time("hi", () => WriteLine("hi"));

internal static class Instrumentation2
{
    public static T Time<T>(string op, Func<T> f)
    {
        var sw = new Stopwatch();
        sw.Start();
        T t = f();
        sw.Stop();
        WriteLine($"{op} took {sw.ElapsedMilliseconds}ms");
        return t;
    }

    public static void Time(string op, Action g)
        => Time<Unit>(op, g.ToFunc());
}

internal static class Instrumentation
{
    public static T Time<T>(string op, Func<T> f)
    {
        var sw = new Stopwatch();
        sw.Start();
        T t = f();
        sw.Stop();
        WriteLine($"{op} took {sw.ElapsedMilliseconds}ms");
        return t;
    }
    
    public static void Time(string op, Action g)
    {
        var sw = new Stopwatch();
        sw.Start();
        g();
        sw.Stop();
        WriteLine($"{op} took {sw.ElapsedMilliseconds}ms");
    }
}

// Util
internal enum Risk {Low, Medium, High}

internal struct Age
{
    public int Value { get; }

    public Age(int value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("");
        }

        Value = value;
    }

    private static bool IsValid(int age)
        => 0 <= age && age <= 120;
}

internal class Age2
{
    public Age2(int value)
    {
        Value = value;
    }

    private int Value { get; }

    public static bool operator <(Age2 l, Age2 r)
        => l.Value < r.Value;
        
    public static bool operator >(Age2 l, Age2 r)
        => l.Value < r.Value;
    
    public static bool operator <(Age2 l, int r)
        => l < new Age2(r);
        
    public static bool operator >(Age2 l, int r)
        => l > new Age2(r);
}
