using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace MyNamespace;

// Validator<T> : T -> Validation<Error, T>
public delegate Validation<Error, T> Validator<T>(T t);

public record MakeTransfer(DateTime Date);

public static class FpModuleExample
{
  public static Func<MakeTransfer, IResult> ConfigureSaveTransferHandler(IConfiguration config)
  {
    ConnectionString connString = config.GetSection("ConnString").Value;
    SqlTemplate sql = "INSERT INTO Transfer (Date) VALUES (@Date)";

    var save = connString.TryExecute<Unit>(sql);
    var validate = DateNotPast(() => DateTime.Now);

    return HandleSaveTransfer(validate, save);
  }

  public static Func<MakeTransfer, IResult> HandleSaveTransfer
  (
    Validator<MakeTransfer> validate,
    Func<MakeTransfer, TryOption<Unit>> save
  ) => transfer
    => validate(transfer).Map(save).Match
    (
      Fail: e => BadRequest(e[0]),
      Succ: res => res.Match(
        Some: _ => Ok(),
        None: () => StatusCode(StatusCodes.Status500InternalServerError),
        Fail: e => StatusCode(StatusCodes.Status500InternalServerError)
      )
    );


  static Validator<MakeTransfer> DateNotPast(Func<DateTime> Clock)
    => transfer => transfer.Date.Date < Clock().Date
      ? Error.New("Transfer date cannot be in the past")
      : transfer;


  static IResult Ok() => new OkResult() as IResult;
  static IResult BadRequest(Error error) => new BadRequestObjectResult(error.Message) as IResult;
  static IResult StatusCode(int status) => new StatusCodeResult(status) as IResult;
}

