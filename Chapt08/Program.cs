using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using static System.Console;

void Example1()
{
  Either<string, double> DivideAndSqrt(double x, double y)
    => y == 0
      ? "Divide by zero"
      : x * y < 0
        ? "Negative number"
        : Math.Sqrt(x / y);

  WriteLine(DivideAndSqrt(4, 2)); // Right(1.414..)
  WriteLine(DivideAndSqrt(4, 0)); // Left(Divide by zero)
  WriteLine(DivideAndSqrt(4, -2)); // Left(Negative number)
}

public class MakeTransferController(ILogger loger, IValidator<MakeTransfer> validator, IRepository<MakeTransfer> req, ISwiftService swiftService)
{
  public ResultDto<Unit> MakeTransfer1([FromBody] MakeTransfer transfer)
    => Handle1(transfer)
      .ToResult1();
  public IActionResult Ok() => new OkResult();
  public IActionResult BadRequest(Error error) => new BadRequestObjectResult(error.Message);

  public IActionResult OnFailure(Error error)
  {
    loger.LogError(error.Message);
    return new StatusCodeResult(500);
  }

  public IActionResult MakeTransfer2([FromBody] MakeTransfer transfer)
    => Handle1(transfer)
      .Match<IActionResult>(
        Fail: x => BadRequest(x[0]),
        Succ: t => t.Match(
          Succ: _ => Ok(),
          Fail: e => OnFailure(e)));
  
  Validation<Error, Try<Unit>> Handle1(MakeTransfer transfer) =>
    Validate(transfer)
      .Map(Save);


  Validation<Error, MakeTransfer> Validate(MakeTransfer transfer) => Validate1(transfer).Bind(Validate2);
  Validation<Error, MakeTransfer> Validate1(MakeTransfer transfer) => transfer;
  Validation<Error, MakeTransfer> Validate2(MakeTransfer transfer) => transfer;
  Try<Unit> Save(MakeTransfer account) => req.Save(account);
}

public static class Ext
{
  public static ResultDto<T> ToResult1<T>(this Validation<Error, Try<T>> valid)
    => valid.Match(
      Succ: t => t.Match(
        Succ: data => new ResultDto<T>(data),
        Fail: e => new ResultDto<T>(e)),
      Fail: e => new ResultDto<T>(e[0]));
}

public record ResultDto<T>
{
  public bool Success { get;  }
  public bool Fail => !Success;
  
  public T Data { get; }
  public Error Error { get; }
  
  public ResultDto(T data) => (Success, Data) = (true, data);
  public ResultDto(Error error) => (Success, Error) = (false, error);
}

public interface ISwiftService
{
  void Send(MakeTransfer transfer);
}

public record AccountState(decimal Balance);

public record MakeTransfer(Guid From, Guid To, decimal Amount);

public interface IValidator<T>
{
  bool IsValid<T>(T t);
}

public interface IRepository<T>
{
  Option<T> Get(Guid id);
  Try<Unit> Save(T t);
}
