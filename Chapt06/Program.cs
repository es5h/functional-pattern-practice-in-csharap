// See https://aka.ms/new-console-template for more information

using LanguageExt;

public struct Age
{
  private int Value { get; }
  
  public static Option<Age> Create(int value) =>
    value >= 0 && value <= 120
      ? Option<Age>.Some(new Age(value))
      : Option<Age>.None;
  
  private Age(int value) => Value = value;
  
  public static implicit operator int(Age age) => age.Value;
  
  private static bool IsValid(int age) => age >= 0 && age <= 120;
}
