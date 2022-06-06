using System.Diagnostics;

namespace Chapt3;

public class MakeTransferOOTest
{
    private static readonly DateTime PresentDate = new(2021, 3, 12);

    private class FakeDateTimeService : IDateTimeService
    {
        public DateTime UtcNow => PresentDate;
    }
        
    // [Test]
    public static void WhenTransferDateIsFuture2_ThenValidatePasses()
    {
        var svc = new FakeDateTimeService();
        
        var sut = new DateNotPastValidator2(svc);
        
        var transfer = MakeTransfer.Dummy with
        {
            Date = PresentDate.AddDays(-1)
        };

        var actual = sut.IsValid(transfer);
        Console.WriteLine(actual == false);
    }
}