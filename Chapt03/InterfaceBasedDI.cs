using Xunit;

namespace Chapt03;

public interface IValidator<T>
{
  bool IsValid(T t);
}

// 테스트 코드
public class DateNotPastValidatorTest
{
  private class TestDateTimeProvider : IDateTimeProvider
  {
    public DateTime UtcNow { get; } = new(2024, 2, 1); // 테스트용 현재 시간
  }

  [Theory]
  [InlineData(1)]
  // [InlineData(-1)]
  [InlineData(365)]
  public void Test(int offset)
  {
    DataValidator validator = new(new TestDateTimeProvider());
    Data data = Data.Dummy with { WriteDate = new TestDateTimeProvider().UtcNow.AddDays(offset) };
    Assert.True(validator.IsValid(data));
  }
}

// 데이터 모델
public record Data(string data, DateTime WriteDate = default)
{
  internal static Data Dummy => new("dummy");
}

// dateTimeProvider 를 주입받아서 현재 시간을 기준으로 유효성을 검사하는 DataValidator
public class DataValidator(IDateTimeProvider dateTimeProvider) : IValidator<Data>
{
  public bool IsValid(Data data) => data.WriteDate.Date > dateTimeProvider.UtcNow.Date;
}

// DateTime Provider
public interface IDateTimeProvider
{
  DateTime UtcNow { get; }
}

public class DefaultDateTimeProvider : IDateTimeProvider
{
  public DateTime UtcNow => DateTime.UtcNow;
}
