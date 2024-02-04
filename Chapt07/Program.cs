using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using static LanguageExt.Option<MakeTransfer>;
using static LanguageExt.Option<AccountState>;

public class MakeTransferController(IValidator<MakeTransfer> validator, IRepository<AccountState> accounts, ISwiftService swiftService)
{
  public void MakeTransfer([FromBody] MakeTransfer transfer)
    => Some(transfer)
      .Map(Normallize)
      .Filter(validator.IsValid)
      .Iter(Book);

  private MakeTransfer Normallize(MakeTransfer transfer) => transfer; // Temp Identity Map

  private void Book(MakeTransfer transfer)
  {
    accounts.Get(transfer.From)
      .Bind(from => from.Debit(transfer.Amount))
      .Iter(account =>
      {
        accounts.Save(transfer.From, account);
        swiftService.Send(transfer);
      });
  }
}

public interface ISwiftService
{
  void Send(MakeTransfer transfer);
}

public record AccountState(decimal Balance);

public static class Account
{
  public static Option<AccountState> Debit(this AccountState from, decimal amount)
    => from.Balance >= amount
      ? Some(new AccountState(from.Balance - amount))
      : Option<AccountState>.None;
}

public record MakeTransfer(Guid From, Guid To, decimal Amount);

public interface IValidator<T>
{
  bool IsValid<T>(T t);
}

public interface IRepository<T>
{
  Option<T> Get(Guid id);
  void Save(Guid id, T t);
}
