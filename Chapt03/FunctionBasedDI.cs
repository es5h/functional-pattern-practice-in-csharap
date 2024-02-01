using Xunit;

namespace Chapt03;

// 테스트 코드
public class DateNotPastValidatorTest2
{
  [Theory]
  [InlineData(1)]
  // [InlineData(-1)]
  [InlineData(365)]
  public void Test(int offset)
  {
    DateTime testNow = new(2024, 2, 1); // 테스트용 현재 시간
    DataValidator2 validator = new(() => testNow);
    Data2 data = Data2.Dummy with { WriteDate = testNow.AddDays(offset) };

    Assert.True(validator.IsValid(data));
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
