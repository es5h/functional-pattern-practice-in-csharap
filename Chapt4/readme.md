## Example of function singature 

int => string : `Func<int,string>` : `int i => i.ToString();`

() => string : `Func<string>` : `() => "hi"`

int => () : `Action<int>` : `(int i) => WriteLine($(i))`

() => () : `Action`

(int, int ) => (int) : `Func<int, int, int>` : `(int a, int b) => a + b`


ex. 
`R Connect<R>(string con, Func<IDbConnection, R> func)

`Connect : (string, (IDbCnonection => R)) => R`


## Honest Function
현 챕터 샘플 코드에서 사용하는 CalculateRiskProfile 함수를 보자.
`int => Risk` 이다.

하지만 유효하지 않은 int 가 들어 온다고 했을대, exception 발생하게 코드를 작성 했다면, signature 대로 가지 않는다.(dishonest)

function is honest if
1) returns value of the declared type 예시에선 risk
2) don't throw exceptions
3) never return null