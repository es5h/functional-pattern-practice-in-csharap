using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using static Functional.F1.F;
using Unit = System.ValueTuple;
using Functional;
using static System.Console;
using static System.Math;
using Boc.Domains;
using Chapt2.DbLoggerExample.AfterPattern;
using Dapper;
using Functional.F1;

//Example 1.
Either<string, double> Calc(double x, double y)
{
    if (y == 0) return "y cannot be 0";
    if (x != 0 && Sign(x) != Sign(y)) return "x / y cannot be native";

    return Sqrt(x / y);
}
WriteLine(Calc(3,0));
bool Sign(double x) => x >= 0 ? true : false;

//Example2. Option-based
Func<Candidate, bool> IsEligible;
Func<Candidate, Option<Candidate>> TechTest;
Func<Candidate, Option<Candidate>> Interview;

Option<Candidate> Recruit(Candidate c)
    => Some(c)
        .Where(IsEligible)
        .Bind(TechTest)
        .Bind(Interview);

//Example 2-1. EitherBased
Func<Candidate, bool> IsEligible2;
Func<Candidate, Either<Rejection, Candidate>> TechTest2;
Func<Candidate, Either<Rejection, Candidate>> Interview2;

Either<Rejection, Candidate> CheckEligibility(Candidate c)
{
    if (IsEligible2(c)) return c;
    else return new Rejection("Not Eligible");
}

Either<Rejection, Candidate> Recruit2(Candidate c)
    => Right(c)
        .Bind(CheckEligibility)
        .Bind(TechTest2)
        .Bind(Interview2);

//Example3. Either-returning with Bind
Func<Either<Reason, Unit>> WakeUpEarly = null;
Func<Either<Reason, Ingredients>> ShopForIngredients = null;
Func<Ingredients, Either<Reason, Food>> CookRecipe = null;

Action<Food> EnjoyTogether = null;
Action<Reason> ComplainAbout = null;
Action OrderPizza = null;

WakeUpEarly()
    .Bind(_ => ShopForIngredients())
    .Bind(CookRecipe)
    .Match(
        Right: dish => EnjoyTogether(dish),
        Left: reason =>
        {
            ComplainAbout(reason);
            OrderPizza();
        }
    );

//Exmaple 4. Work Flow Example
//public class MakeTransferController: ControllerBaase
//{
    //[HttpPost, Route("transfers/book")]
void MakeTrasnfer( /*[FromBody]*/ MakeTransfer request)
    => Handle(request);

Either<Error, Unit> Handle(MakeTransfer transfer)
    => Right(transfer)
        .Bind(ValidateBic)
        .Bind(ValidateDate)
        .Bind(Save2);

Regex bicRegex = new Regex("[A-z]{11}");

Either<Error, MakeTransfer> ValidateBic(MakeTransfer transfer)
    => bicRegex.IsMatch(transfer.Bic)
        ? transfer
        : Errors.InvalidBic;

Either<Error, MakeTransfer> ValidateDate(MakeTransfer transfer)
    => transfer.Date.Date > DateTime.Now
        ? transfer
        : Errors.TransferDateIPast;

Either<Error, Unit> Save2(MakeTransfer cmd)
    => throw new NotImplementedException();
//}
// Handle 은 High-Level Work flow를 정의한다. 유효성 검사 후 Persist.
// Validate 와 Save는 둘다 Either을 리턴한다. Operation May Fail 과 함께

// Example 5.
Either<Error, int> ToIntIfWhole(double d) => (int)d;

Either<Error, int> Run(double x, double y)
    => Calc(x, y)
        .Map(
            Left: msg => new Error(msg),
            Right: d => d
        )
        .Bind(ToIntIfWhole);

// Example6. Validation
Validation<MakeTransfer> ValidateDate2(MakeTransfer transfer)
    => transfer.Date.Date > DateTime.Now
        ? transfer
        : Errors.TransferDateIPast;

// Example7. Exception-Based
string connString;

Exceptional<Unit> Save(MakeTransfer transfer)
{
    try
    {
        ConnectionHelper.Connect(connString, c => c.Execute("INSERT ..", transfer));
    }
    catch (Exception ex)
    {
        return ex;
    }

    return Unit();
}

record MakeTransfer(string Bic, DateTime Date);

namespace Boc.Domains
{
    public sealed record InvalidBicError() : Error("Bic code is invalid");

    public sealed record TransferDateIsPastError() : Error("Transfer date cannot be in the past");

    public static class Errors
    {
        public static Error InvalidBic => new InvalidBicError();
        public static Error TransferDateIPast => new TransferDateIsPastError();
    }
}



public record Error(string Msg);
public record Reason;
public record Ingredients;
public record Food;
public record Candidate(string Name);
public record Rejection(string Msg);
