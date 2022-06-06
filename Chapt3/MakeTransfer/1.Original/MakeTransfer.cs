using System.Text.RegularExpressions;

namespace Chapt3;

public abstract record Command(DateTime Timestamp);

public record MakeTransfer(
    Guid DebitedAccountId,
    
    string Beneficiary, // 받는 사람
    string Iban, // Like Bank Account
    string Bic, // Like Swift Code
    
    DateTime Date,
    decimal Amount,
    string Reference,
    DateTime Timestamp = default
) : Command(Timestamp)
{
    internal static MakeTransfer Dummy => new(default, default, default, default, default, default, default);
}

public interface IValidator<T>
{
    bool IsValid(T t);
}

public class BicFormatterValidator : IValidator<MakeTransfer>
{
    static readonly Regex Regex = new("^[A-Z]{6}[A-Z1-9]{5}$");

    // pure
    public bool IsValid(MakeTransfer transfer)
        => Regex.IsMatch(transfer.Bic);
}

public class DateNotPastValidator : IValidator<MakeTransfer>
{
    //impure
    public bool IsValid(MakeTransfer transfer)
        => DateTime.UtcNow.Date <= transfer.Date.Date;
}
