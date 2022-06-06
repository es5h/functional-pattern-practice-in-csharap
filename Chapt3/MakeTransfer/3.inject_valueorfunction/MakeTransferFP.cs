using Chapt3;

public record DateNotPastValidatorFP (DateTime Today) : IValidator<MakeTransfer>
{
    public bool IsValid(MakeTransfer transfer)
        =>  Today <= transfer.Date.Date;
}

/*
 public void ConfiguredServices(IServiceCollection services){
    services.AddTransient<DateNotPastValidatorFP>( _ => new DateNotPastValidatorFP(DateTime.UtdNow.Date)
 }
*/

public record DateNotPastValidatorFP2 (Func<DateTime> Clock) : IValidator<MakeTransfer>
{
    public bool IsValid(MakeTransfer transfer)
        =>  Clock().Date <= transfer.Date.Date;
}

/*
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<DateNotPastValidator>(_ => new DateNotPastValidator(() => DateTime.UtcNow.Date));
}
*/
