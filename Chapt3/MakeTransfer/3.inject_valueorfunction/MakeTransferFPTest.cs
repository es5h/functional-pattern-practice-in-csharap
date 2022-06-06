using System.Diagnostics;

namespace Chapt3;

public class MakeTransferFPTest
{
    private static readonly DateTime PresentDate = new(2021, 3, 12);

    // [Test]
    public static void WhenTransferDateIsFuture2_ThenValidatePasses()
    {
        var sut = new DateNotPastValidatorFP2(() => PresentDate);
        
        var transfer = MakeTransfer.Dummy with
        {
            Date = PresentDate.AddDays(-1)
        };

        var actual = sut.IsValid(transfer);
        Console.WriteLine(actual == false);
    }
}