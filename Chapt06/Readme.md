## FP Pattern?

### 1. Map and Functor
- `C<T>` 는 Type T 를 가지는 Container 이다.
- `Map : (C<T>, (T -> U)) -> C<U>`

이러한 Map(Select, Lift, etc)이 정의된 타입 C를 Functor라고 한다.
IEnumerable 과 Option은 Functor의 예시이다.

### 2. Monad and Bind
- `Bind : (C<T>, (T -> C<U>)) -> C<U>`
- 예시
  - Int.Parse : `string -> Option<int>`
  - Age.Create : `int -> Option<Age>`

Bind(SelectMany, FlatMap, etc) 가 정의된 타입 C를 Monad라고 한다.

### 3. Return
- `Return : T -> C<T>`
- 예시 
  - Option.Some : `T -> Option<T>`
  - Enumerable.Single : `T -> IEnumerable<T>`

### 4. 정리
- 모든 Monad는 Functor 인가?
  - 맞다. Map은 Bind와 Return으로 구현 가능하다.
- 모든 Functor는 Monad 인가?
  - 아니다.

### 5. 다른 계층에서의 코딩
- 일반 값과 고차 값
  - 일반값 (T): `string`, `int`
  - 고차값 (A<T>): `Option<int>`, `IEnumerable<string>` 
- 추상화
  - 추상화는 일반 타입에 효과 적용을 가능하게 한다.
  - 예시
    - `Option`은 T의 값에 존재성(possibility)을 추가한다.
    - `IEnumerable`는 T의 값에 복수성(aggregation)을 추가한다.
    - `Task`는 T의 값에 비동기성(asynchrony)을 추가한다.
    - `Func`는 T의 값에 지연성(laziness)을 추가한다.
- 추상화 레벨의 변경 (world-crossing)
  - `T -> R` : `ToString: int i => i.ToString()`
  - `T -> A<R>`: `Int.Parse`
  - `A<T> -> A<R>`: `IEnumerable<int> nums = nus.OrderBy(x => x)`
  - `A<T> -> R`: Sum, Count, Average, **Match**, **Fold**, etc...
    - Return을 이용해서 Lift를 한다 했을 때, 위 매핑은 해당 함수의 역함수 처럼 존재하지만, 반드시 존재하는 것은 아니다.
- 옳은 레벨에서의 구현
  - 일반 값만을 활용한 구현은, for나, if, null check 등 low lever 구현으로 비효율적이고, 오류가 발생하기 쉽다.
  - 하지만, 추상하과 심해지면, `A<B<C<T>>>` 등 복잡한 구현이 되어, 이해하기 어려워진다. 이는 `Traverse`와 같은 함수를 사용해야한다.
