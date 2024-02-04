## Either?

### 1. Either
- Symbolic 정의
  - `Either<L, R> = Left<L> | Right<R>

### 2. Chain

```
- Start - Step1 - Step2 - Step3 - - Match Right
        └ ----- ┴ ----- ┴ ----- ┴ - Match Left
```

### 3. 여러 타입의 Either
- `Either<L, R> = Left<L> | Right<R>`
- `Valiation<T> = Invalid(IEnumerable<Error>) | Valid<T>`
- `Exception<T> = Exception | Success<T>`
