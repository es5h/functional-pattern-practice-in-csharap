using NUnit.Framework;

namespace Chapt03;

// 테스트 코드
public class DateNotPastValidatorTest2
{
  [TestCase(1, ExpectedResult = true)]
  [TestCase(-1, ExpectedResult = false)]
  [TestCase(366, ExpectedResult = true)]
  public bool Test(int offset)
  {
    DateTime testNow = new(2024, 2, 1); // 테스트용 현재 시간
    DataValidator2 validator = new(() => testNow);
    Data2 data = Data2.Dummy with { WriteDate = testNow.AddDays(offset) };

    return validator.IsValid(data);
  }
}

// 데이터 모델
public record Data2(string data, DateTime WriteDate = default)
{
  internal static Data2 Dummy => new("dummy");
}

// dateTimeProvider 를 주입받아서 현재 시간을 기준으로 유효성을 검사하는 DataValidator
public class DataValidator2(Func<DateTime> Clock) : IValidator<Data2>
{
  public bool IsValid(Data2 data) => data.WriteDate.Date > Clock().Date;
}
