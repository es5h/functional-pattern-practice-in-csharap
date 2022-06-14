work flow programming

work flow를 함수의 파이프라인이라 생각하자. if 사용을 자제하자

## 예제 1. 유효성 검사후 로직을 돌리는 코드 
```c#
// Orgin
public interface IValidator<T>
{
    bool IsValid(T t);
}

public class MakeTransferController : ControllerBase
{
    //MakeTransfer 은 DTO임. 거래를 위한 정보가 담겨있음
    private IValidator<MakeTransfer> _validator;
    
    public void MakeTransfer([FromBody] MakeTransfer transfer)
        if(_validator.IsValid(transfer)
            Book(transfer);
    
    void Book(MakeTransfer transfer)
        => // doing;
}

// After
public class MakeTransferController : ControllerBase
{
    private IValidator<MakeTransfer> _validator;
    
    public void MakeTransfer([FromBody] MakeTransfer transfer)
        => Some(transfer)
            // .Map(Normalize) // Compistion을 이용하면 flexibity 가 생겼다 말았다 한다.
            .Where(validator.IsValid)
            .ForEach(Book);
}
```

## 예제 2. 도메인 모델 함수형 구현
```c#
    // Origin
    public class Account
    {
        public decimal Balance { get; private set;}
        
        public Account(decimal balance) {
            Balance = balance;
        }    
        
        // 인출
        public void Debit(decimal amount){
            if(balance < amount)
                throw;
            
            Balance -= amount;
        }
    }
```
보다 시피 Debit 은 side effect 유발 시키기 좋은 함수// 유효성 검사가 실패 한다면 state가 오염됨.
```c#
    // After
    public record AccountState(decimal Balance);
    
    public class Account
    {
        public static Option<AccountState> Debit(this AccountState current, decimal account)
            => current.Balance < amount ? None : Some(new AccountState(current.Balance - amount));     
    }

```

## 예제 3. End-to-End 
```c#
interface IRepository<T>
{
    Option<T> Get(Guid id);
    void Save (GUid id, T t);
}

interface ISwiftService
{
    // 송금
    void Wire(MakeTransfer transfer, AccountState account);
}

public class MakeTransferController : ControllerBase
{
    private IValidator<MakeTransfer> _validator;
    private IRepository<AccountState> _account;
    private ISwiftService _swift;
    
    public void MakeTransfer([FromBody] MakeTransfer transfer)
        => Some(transfer)
            .Map(Normalize)
            .Where(validator.IsValid)
            .ForEach(Book);
            
    // 거래 기록 (아래 로직은 서비스 에 가도 되지만 편의상 여기둠)
    public void Book(MakeTransfer transfer) 
        => _accounts
            .Get(transfer.DebitedAccountId)
            .Bind(account => account.Debit(transfer.Account)) // accountstate => Option<accountstate> => Bind 필요
            .ForEach(account =>
                {
                    accounts.Save(transfer.DebittdAccountId, account); // DB 저장
                    _swift.Wire(transfer, account); // 송금
                }
            );   
}
    
    
```

## Expression 과 statement

명령형(imperative)은 Expression 에 의존하고, 함수형은 statement 에 의존한다.
- expression 은 값 그자체 `123`, `"abc"`, 또는 `변수`, 또는 `a || b` 같은 연산의 값
- statement 는 할당, `if`, loop` 등의 프로그램을 하는 지시문

expression 은 부작용을 일으키는 가능 성이 있지만,  statement는 부작용 그자체라 작성하지 않는다.

이 챕터의 방식으로 composing을 하는 프로그래밍 하게된다면 자연스럽게 `ForEach`같은 부작용을 일으키는 것들은 Pipeline에 끝에 몰리게 된다. 이는 sideeffect를 isolate 시킨다. (심지어 표현상에서도 그리된다.)
statement 없이 코드를 작성하는게 처음엔 어색 할 수 있어도, 가능하다. 단 위의 코드에서도 단 두개의 statement `ForEach`만 사용되었다.
