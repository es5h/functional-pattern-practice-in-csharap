using System.ComponentModel.DataAnnotations;
using LanguageExt;
using LanguageExt.Common;

namespace MyNamespace;

public class AggregateValidation
{
  public static Validator<T> FailFast<T>(IEnumerable<Validator<T>> validators)
    => t
      => validators.Fold(Valid(t),
        (acc, validator) => acc.Bind(_ => validator(t)));
  
  public static Validator<T> HarvestErrors<T> (IEnumerable<Validator<T>> validators)
  => t =>
  {
    var errors = validators.Map(validate => validate(t))
      .Bind(v => v.Match(
          Succ: _ => Option<Error>.None,
          Fail: e => Option<Error>.Some(e[0])
        )
      ).ToSeq();
    
    return errors.Count == 0 ? Valid(t) : Validation<Error, T>.Fail(errors);
    
  }
  // Valid(t).Bind(validators[0]).Bind(validators[1]).Bind(validators[2])...

  private static Validation<Error, T> Valid<T>(T t)
  {
    throw new NotImplementedException();
  }
}
