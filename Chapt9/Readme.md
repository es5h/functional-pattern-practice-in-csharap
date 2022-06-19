## Currying
Haskell Curry 이름에서 땀

`(T1, T2, .. , Tn) -> R `, 커리하면
`T1 -> T2 -> .. -> Tn -> R `

## Modularizing
``` c#
public class MakeTrasnferController : ControllerBase
{
    DateTIme now;
    static readonly Regex regex = new Regex("^[A-Z]{6}[A-Z1-9]{5}$");
    string connString;
    ILogger<MakeTrasnferController> logger;
    
    public IActionResult MakeTransfer([FromBody] MakeTransfer transfer)
    
    IActionResult OnFaluted(Exception ex)
    
    Validation<Exception<Unit>> Handle(MakeTransfer request)
    Validation<MakeTransfer> Validate(MakeTransfer cmd)
    Validation<MakeTransfer> ValidateBic(MakeTransfer cmd)
    Validation<MakeTransfer> ValidateDate(MakeTransfer cmd)
    
    Exception<Unit> Save(MakeTransfer transfer)
}
```
End-to-End 의 예제의 멤버 요소를 보면 위와 같았다. 모든 로직이 Controller에 있으니 Module화 필요
OOP와 Functional 의 관점 차이를 아래의 예제를 통해 확인하자.

###  OOP
```c#
public interface IValidator<T>
{
    Validation<T> Validate(T request);
}

public interface IRepository<T>
{
    Option<T> Lookup(Guid id);
    Exceptioal<Unit> Save(T entity);
}
```

컨트롤러는 `Interface`에만 의존하고 low-level 에 있는 구현체를 사용하듯이 한다. (의존성 역전)
OOP 는 아래와 같은 장점이 있다.
- Decoupling => 구현체만 바꾸면된다.
- Testability => fake Repository를 주입하는 방식으로 Db에 직접 연관 없이 테스트 가능
아래와 같은 단점이 있따.
- Interface 가 는 다 => BoilierPlate 가 는다, 코드 navigate가 어려워진다.
- fake 구현체 만드는게 어렵다.

아래는 Functional 조금에 OOP 모듈링을 적용 한 예시

```c#
public class MakeTransferController : ControllerBase
{
    IValidator<MakeTransfer> _validator; 
    IRepository<MakeTransfer> _repository; 
    public MakeTransferController(IValidator<MakeTransfer> validator, IRepository<MakeTransfer> repository)
    {
        _validator = validator; 
        _repository = repository; 
    }
    
    [HttpPost, Route("api/transfers/book")]
    public IActionResult TransferOn([FromBody] MakeTransfer transfer)
        => validator.Validate(transfer)
            .Map(repository.Save)
            .Match
            (
                Invalid: BadRequest,
                Valid: result => result.Match<IActionResult>
                (
                    Exception: _ => StatusCode(500, Errors.UnexpectedError),
                    Success: _ => Ok()
                )
            );
}
```

## FP
OOP의 기본 유닛은 Object이고 FP의 기본 유닛은 Function 이다.

아래는 Injection 할 Dependency
```c#
public record DateNotPastValidator(Func<DateTIme> Clock) : IValidator<MakeTrasnfe>
{
    public Validation<MakeTransfer> Validate(MakeTrasnfer transfer)
        => transfer.Date.Date < Clock().Date
            ? Erros.TrasnferDateIsPast
            : Valid(transfer);
}
```
아래는 Controller가 IValidator라는 Object에 의존하지 않고 Validator라는 FUnction에 의존할 경우

```C#
public static Validator<MakeTransfer> DateNotPast(Func<DateTime> clock)
    => trasnfer => transfer.Date.Date< < clock().Date
        ? Error.TransferDateIsPast
        : Valid(transfer);
```
- app을 사용하는 시점에 
`Validator<MakeTransfer> val = DateNotPast(() => DateTime.UtcNow());
- 특정 시점을 테스트 하는 용도로
`Validator<MakeTransfer> uut = DateNotPast(() => new DateTime(2020, 20, 10));

  `DateNotPast`는 Clock 과 MakeTrasnfer 가 둘다 필요한 Binary 함수 이다.

```c#
public class MakeTransferController : ControllerBase
{
    Validotor<MakeTransfer> validate;
    Func<MakeTransfer, Exceptional<Unit>>save;
    
    [HttpPost, Route("api/transfers/book")]
    public IActionResult MakeTrasnfer([FromBody] MakeTransfer cmd)
        => validate(cmd).Map(save).Match( //..   
}
```


## Top-Level Fp
```c#
using static Microsoft.AspNetCore.Http.Results;

static Func<MakeTransfer, IResult> HandleSaveTransfer
(
    Validator<MakeTrasnfer> validate,
    Func<MakeTransfer, Exceptional<Unit>>save
)
    => transfer => validate(transfer).Map(save).Match
        (
            Invalid : err => BadRequeset(err)
            Valid: result => result.Match
                (
                    Exception : _ => StatusCode(//500..
                    Success :  => Ok()
                )
         )
```

# Mapping (Functions - Api EndPOints)
```c#
  using Microsoft.AspNetCore.Builder;
  
  var app = WebApplication.Create(); // create application
  app.MapGet("/", () => "Hello, World!"); // configure endpoints
  
  app.MapPost("/todos", async ([FromServices] TodoDbContext db, Todo todo) => {
    await db.Todos.AddAsync(todo);
    await db.SaveChangesAsync();
    
    return new StatusCodeResult(204);
  });
  await app.RunAsync(); // starts listening for requests
```