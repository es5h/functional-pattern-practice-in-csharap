using Chapt3;

public interface IDateTimeService
{
    DateTime UtcNow { get; }
}

public class DefaultDateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
}

public class DateNotPastValidator2 : IValidator<MakeTransfer>
{
    private readonly IDateTimeService _dateTimeService;
    
    public DateNotPastValidator2(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    public bool IsValid(MakeTransfer transfer)
        => _dateTimeService.UtcNow <= transfer.Date.Date; 
    // 이젠 IDateTImeService 가 어떻게 구현되는지에 따라 pure function 이 된다.
}

// 위에 class 를 좀더 refactor

public record DateNotPastValidator3 (IDateTimeService DateTimeService) : IValidator<MakeTransfer>
{
    private IDateTimeService DateTimeService { get; } = DateTimeService;

    public bool IsValid(MakeTransfer transfer)
        => DateTimeService.UtcNow <= transfer.Date.Date;
}