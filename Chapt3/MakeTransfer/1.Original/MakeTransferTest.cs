using System.Diagnostics;

namespace Chapt3;

public static class MakeTransferTest
{
    // [Test]
    public static void WhenTransferDateIsFuture_ThenValidatePasses()
    {
        var sut = new DateNotPastValidator();
        var transfer = MakeTransfer.Dummy with
        {
            Date = new DateTime(2021, 3, 12),
        };

        var actual = sut.IsValid(transfer);
        Console.WriteLine(actual == true);
    }
}