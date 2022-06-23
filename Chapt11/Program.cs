using System.Collections.Immutable;
using static System.Console;

// Ex1.
var original = new AccountState(Currency: "EUR");
var activated = original.Activate();

WriteLine(original.Status);
WriteLine(original.Currency);

WriteLine(activated.Status);
WriteLine(activated.Currency);

// Ex2. mutable test
var mutableList = new List<Transaction>();

var account = new AccountState(Currency: "EUR", TransactionHistory: mutableList);

WriteLine(account.TransactionHistory.Count());

mutableList.Add(new(-1000, "CreateTrouble", DateTime.Now));

WriteLine(account.TransactionHistory.Count());

// Ex3.

public static class Ext
{
    public static AccountState Create(CurrencyCode currency) => new(currency); 
    public static AccountState Add(this AccountState account, Transaction transaction) => account with
    {
        TransactionHistory = account.TransactionHistory.Prepend(transaction)
    };
    
    public static AccountState Activate(this AccountState original) => original with { Status = AccountStatus.Active };

    public static AccountState RedFlag(this AccountState original) =>
        original with { Status = AccountStatus.Frozen, AllowedOverdraft = 0m };
    
    // Currency init 없앴으므로 컴파일 에러
    /*public static AccountState CurrencyUsd(this AccountState original) =>
        original with { Currency = "USD"};*/
}

public enum AccountStatus
{
    Requested,
    Active,
    Frozen,
    Dormant,
    Closed
}

public record AccountState(CurrencyCode Currency, AccountStatus Status = AccountStatus.Requested,
    decimal AllowedOverdraft = 0m, IEnumerable<Transaction> TransactionHistory = null)
{
    public CurrencyCode Currency { get; } = Currency;
    // init; 이 있으면 복사 할때 사용 가능, init; 없으면 불가능

    public IEnumerable<Transaction> TransactionHistory { get; init; } =
        ImmutableList.CreateRange(TransactionHistory ?? Enumerable.Empty<Transaction>());
    
    /* 이러면 안 mutable 해짐 mutableList 보고 봤으면 Count 1로 바뀌어잇을거임
    public IEnumerable<Transaction> TransactionHistory { get; init; } =
        TransactionHistory ?? Enumerable.Empty<Transaction>();*/
}

public record Transaction(decimal Amount, string Description, DateTime Date);

public record CurrencyCode(string Value)
{
    public static implicit operator string(CurrencyCode c) => c.Value;
    public static implicit operator CurrencyCode(string s) => new(s);
}
