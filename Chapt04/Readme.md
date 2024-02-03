## Function Signature?

### 1. 함수 시그니처
- `int -> string`
```
Func<int, string> f = (int x) => x.ToString();
```
- `() -> string`
```
Func<string> f = () => "Hello, World!";
```
- `(int) -> ()`
```
Action<int> f = (int x) => WriteLine(x);
```
- `() -> ()`
```
Action f = () => WriteLine("Hello, World!");
```
- `(int, int) -> int`
```
Func<int, int, int> f = (int x, int y) => x + y;
```

- Combination `(IEnumerable<T>, (T -> bool)) -> IEnumerable<T>`
```
IEnumerable<T> Where<T>(IEnumerable<T> source, Func<T, bool> predicate);

Func<IEnumerable<T>, Func<T, bool>, IEnumerable<T>> Where = (IEnumerable<T> source, Func<T, bool> predicate) => source.Where(predicate);
```

### 2. Honest Function

- 인수(정의역)의 타입과 반환 값(공역)의 타입이 정확하게 명시되면, 함수는 공역 내의 값으로 매핑되면, Honest Function이다.
- DisHonest Function은 이러한 조건을 만족하지 않는다.
  - `int -> int` 의 함수가 주어질 경우, Null을 반환하거나 Exception을 던지는 경우 등이 있다.
- 

