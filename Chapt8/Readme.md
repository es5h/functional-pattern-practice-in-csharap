Option과 Either을 사용 하게 경우,

`Option`은 `None`, `Either`는 `Left` 를 Failure를 기술 할 때 사용 하게 될 것이다.
우리의 WorkFlow에선 `Map`, `Bind`, `Where`를 사용 하겠지만, app 외부와 소통하기 위해선 마지막엔 Match 등을 이용하여 추상 객체를 구체화 해야한다.


### OptionBased APi

```c# 
using Microsoft.AspNetCore.Mvc;

namespace Chapt8_1;

public class InstrumentsController : ControllerBase
{
    [HttpGet, Route("api/instruments/{ticker}/details")]
    public IActionResult GetInstrumentDetails(string ticker)
        => GetDetails(ticker)
            .Match<IActionResult>(
                () => NotFound(),
                result => Ok(result)
            );

    private Option<In strumentDetails> GetDetails(string ticker)
    {
        throw new NotImplementedException();
    }
}

internal record InstrumentDetails;
```

### EitherLikeInterfaceAPi
```c#
public class MakeTransferController : ControllerBase
{
    [HttpPost, Route("api/transfers/future")]
    public IActionResult MakeTrasnfer([FromBody] MakeTrasnfer transfer)
        => Handle(transfer).Match<IActionResult>
       (
        Left: BadRequst, //404
        Right: _ => Ok() //200
       );
       
   Either<Error, Unit> Handle(MakeTransfer transfer)
}
```

### ResultDto
```C#
    public record ResultDto<T>
    {
        public bool Succeded { get; }
        public bool Failed => !Succeedded;
        
        public T Data { get; }
        public Error Error { get; }
        
        internal ResultDto(T data) => (Succeded, Data) = (true, data);
        internal ResultDto(Error error) => Error = error;
    }
    
    public static ResultDto<T> ToResult<T>(this Either<Error, T> eitehr
        => either.Match
        (
            Left : error => new ResultDto<T>(error),
            Right: data => new ResultDto<T>(data)
        );
        
    public class MakeTransferController : ControllerBase
    {
        [HttpPost, Route("api/tranfers/future")]
        public ResultDto<Unit> MakeTrasnfer([FromBody] MakeTrasnfer transfer)
            => makeTrasnfer(transfer).ToResult();
    }    
```

저자는 Either Class를 변형 클래스도 만듬
```
Validation<T> =	Invalid(IEnumerable<Error>) | Valid(T)
```
```
Exceptional<T> = Exception | Success(T)
```
```
Either <L, R> =	Left | Right 
```

`Validation`은 비지니스 룰의 위반을 의미.
`Exceptional`은 기술적인 에러를 의미한다. 

```c#
public class makeTrasnsferController : ControllerBase
{
    Validation<Exceptional<Unit>> Handle(MakeTransfer transfer)
        => Validate(transfer)
            .Map(Save);
            
    Validation<MakeTrasnfer> Validate(Maketransfer transfer)
        => ValidateBic(transfer)
            .Bind(ValidateDate);
            
    Validation<MakeTransfer> ValidateBic(MakeTransfer transfer) // 로직
    Validation<MkaeTransfer> ValidateDate(MakeTransfer transfer) // 로직
   
    Exceptional<Unit> Save(MakeTransfer transfer)
}
```

`Validate`는 `Validation`을 반환하고, `Save`는 `Exceptional`을 반환하여 `Bind`를 이용하여  합칠순 없다. 대신 `Map`을 사용하여 `Validation<Exceptional<Unit>>`으로 중첩 타입을 사용하였다. 이는 `Validation`의 효과 (원하는 리턴 값 대신 유효성 에러), `Exception` 의 효과 (validation을 통과했더라도, 값대신 예외)를 합친것과 같다.

최종 End-to-End 코드는 아래와 같다. 결국 `Validation<Exceptional<Unit>>` 을 클라이언트 에게 반환해줘야한다.

```c#
    [HttpPost, Route("api/transfers/future")]
    public IActionResult MakeTrasnfer([FromBody] MakeTrasnfer transfer)
        => Handle(transfer).Match
       (
            Invalid: BadRequest,
            Valid: result => result.Match
            (  
                Exception: OnFaulted,
                Success: _ => Ok()
            )
       );
   
   public IActionResult OnFaulted(Exception ex)
   {
        logger.LogError(ex.Message);
        return StatucCode(600, Errors.UnexpectedError);
   }
   
   Validation<Exceptional<Unit>> Handle(MakeTransfer transfer)
```

위 코드를 보면, 비지니스 규칙과 기술적인 이슈에 의한 failure를 구분하게 할 수 있다.