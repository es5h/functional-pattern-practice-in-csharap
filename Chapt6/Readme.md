- Regular value `T` // `string`, `int`, `DayofWeek`, `Neighbor`...
- Elevated value `A<T>` / `Option<int>`, `Enumerable<string>`, `Func<Neigbor>, ..`
- ?? 많이 추상화됨 : ..A<A<A<..<T>..>

Elevated value는 그의 대응되는 regular 타입의 추상화 이다. 
- Option 은 Optionality 를 부여해준다. T가 아닌 May be T
- IEnumerable 은 집합의 의미를 부여해준다. T 가 아닌 T의 sequence.
- Func은 laziness를 부여해준다. T 가 아닌 T를 얻기 위한 계산
- Task는 asynchrony 를 부여해준다. T 가 아닌 some point에서 T를 얻기 위한 promise

Return은 regular 값을 elevate 해준다. `Return : T -> A<T>`

4 종의 유형의 매핑을 나눠보자
- `T -> R` (regular mapping)
    - `int -> string` : `int i => i.ToString();`
- `A<T> -> A<R>`(mapping elevated value)
    - `IEnumerable<int> -> IEnumerable<int>` : `(IEnumerable<ints> ints) => ints.OrderBy(i => i)
    - `Map`, `Bind`, `Where`
- `T -> A<R>` (upward - crossing function)
  - `string -> Option<int>` : Int.Parse
  -  `Return`
- `A<T> -> R` (downcrossing function)
  - `Average, Sum, Count for IEnumerable`, `Match For Option`
  - Return의 역함수 : `C<T> -> T` Honest 함수 형태로 언제나 가능한 것은 아니다.
  - `Option<int> -> int` Return 이 onto functino 이 아니기 때문에..
  - 마찬가지로, `IEnumerable<Emplyee> -> Employee`. single Employee 의 경우 Employee로 매핑 되지 못함. Mathematica에서 []를 끝에서 없애지 못하는것에 대응 
  * Return 은 `trivial Mapping` 임


## Map vs Bind
- Map은 `f: T -> R` 은 추상화된 레벨에서 `Map(f) : A<T> -> A<R>`로 작용한다.
- Bind는 `f: T -> A<R>` 의 추상화 레벨에서 `Bind(f) : A<T> -> A<R>` 로 작용한다.
Map은 regular function 을 취하고, BInd 는 upward-crossing 을 취한다. 

만약 Map이 upward를 취한다면, `A<A<R>>` 도 가능해지는데, 이는 보통 원하는 것이 아니고, 그럴땐 대신 Bind를 선택한다.

## 올바른 추상화 레벨에서의 작업
다른 추상화 레벨에서 작업하는 것은 중요하다. regular value에서만 작업하게 된다면 for loop, null check 등의 low-level operation 을 하게된다.
하나이 추상화 위 단계에서 (A<T>)에서 일하는것은 매우편하다.

그러나 깊게 추상화 레벨에 들어가는건 주의하자 like `A<B<C<D<T>>>>`다루는 것도 어렵다.
