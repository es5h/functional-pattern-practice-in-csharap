using static F;

namespace Chapt6;

public class Util
{
    public struct Age
    {
        private int Value { get; }
    
        public static Option<Age> Create(int age)
            => IsValid(age) ? Some(new Age(age)) : None;
    
        private Age(int value)
            => Value = value;
    
        private static bool IsValid(int age)
            => 0 <= age && age < 120;
    }
}