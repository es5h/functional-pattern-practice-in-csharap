## Pattern?

### 1. Fuctor
- `C<T>` 는 Type T 를 가지는 Container 이다.
- `Map : (C<T>, (T -> U)) -> C<U>`

이러한 Map이 정의된 타입 C를 Functor라고 한다.
IEnumerable 과 Option은 Functor의 예시이다.

### 2. Bind
- `Bind : (C<T>, (T -> C<U>)) -> C<U>`
- 예시
  - Int.Parse : `string -> Option<int>`
  - Age.Create : `int -> Option<Age>`

