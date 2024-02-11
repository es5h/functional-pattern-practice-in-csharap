## Currying?

### 1. Curry

다변수 함수를 단변수 함수들의 연속으로 변환하는 것을 말한다.
예를 들어 아래와 같은 이변수 함수가 있다고 하자.

```math
f(x, y) = f_{x}(y)
```

아래의 함수를
```math
f : X \times Y \rightarrow Z
```
아래와 같이 
```math
f_{curried}: X \rightarrow (Y \rightarrow Z)
```
로 변환 하는 기법이다.

커링을 사용하면 부분적용 (Partial Application) 을 사용 할 수 있어, 코드의 재사용성과 모듈성을 높일 수 있다.
arrow notation 은 right-associative 이므로, 아래와 같이 표현해도 무방하다.

```math
f_{curried}: X \rightarrow Y \rightarrow Z
```

좀 더 일반적dm로, n변수 함수는 아래와 같이 커리 할 수 있다.

```math
(X1, X2, ..., Xn) \rightarrow Y
```
위의 signature를 가진 함수는 아래와 같은 커리 폼을 가질 수 있다.

```math
X1 \rightarrow (X2 \rightarrow (X3 \rightarrow ... (Xn \rightarrow Y) ...))
```

### 2. 부분 적용 (Partial Application)
부분 적용은 함수의 일부 인수를 고정한 새로운 함수를 생성하는 것을 말한다.
예를 들어 아래와 같은 $f(x, y, z)$ 함수가 있을 때, $x$와 $y$ 를 미리 적용(부분 적용) 하여,
새로운 함수 $g(z) = f(x_0, y_0, z)$ 를 생성하는 것을 말한다.

### 3. 수동으로 부분적용을 하는 경우와 Apply 함수를 사용하는 경우
- CurriedAndPartialApplication.cs 코드 참고

### 4. argument 의 순서
부분 적용을 적용하기 위해 인수의 순서를 잘 고려해야한다.
- 작업의 영향을 받는 데이터는 뒤에 위치시킨다.
- 함수의 작동 방식을 결정하는 옵션이나, 종속성 있는 데이터는 앞에 위치시킨다.

### 5. 커링과 부분 적용의 차이
커링은 다변수 함수를 단일 변수 함수의 Chain 으로 변환하는 것이 목적이지만, 부분 적용은 새로운 함수를 생성 하는것이 목적이다.
주로 커링은 F에서 함수의 재사용성과 조합성을 높일 때 사용하고, 부분 적용은 조금 더 구체적인 함수를 생성할 때 사용한다.

위에서 알 수 있는 것처럼 커링 자체는 부분 적용을 하기 위해 함수를 최적화 하는 것이 목적이다.
물론, 코드의 예시처럼, 일반 함수와 `Apply` 함수를 사용하면, 커링을 사용하지 않아도 부분 적용을 할 수 있지만, 커링을 사용하면, 부분적용을 쉽게 할 수 있다.

### 6. 부분 적용 친화적인 API 만들기
- `getMember: Guid -> Option<Member>`
- 'getMembersByCategory: string -> IEnumerable<Member>`

기존 Dapper Query는
```csharp
IEnumerable<T> Query<T>(this IDbConnection connection, string sql, object param)
```
를 갖는다. (일부 인수 생략) 
[참고] 해당 Api 에서의 인수 순서는 부분 적용하기 위해 적합한 순서처럼 보이지만 실제로는 그렇지 않다.
실제로 connection 은 disposable 이다. (쿼리가 적용 될때마다 connection이 생성되고, dispose 되어야 한다.)
저자는 connection 을 덜 일반적이라고 보고 두번째 인수로 두었다.

이하 코드 참고 (DapperPartialApplication.cs)

### 7. FP 모듈화
- OOP의 Dependency Inverson Principle (DIP)는 상위 수준이 하위 수준을 직접 의존하지 않고, 추상화에 의존해야 한다는 원칙이다.
    - Validator와 Repository 가 그 예시이고, 그래서 interface를 사용한다.
- 이는 아래의 장점을 일으킨다.
  - Decoupling: 하위 수준 모듈의 변경 예를들면 DB에서 MQ로 변경되어도 상위 수준 모듈에 영향을 주지 않는다. (단지, 연결만 변경하면 된다.)
  - Testability: 하위 수준 모듈을 Mocking 하여 상위 수준 모듈을 테스트 할 수 있다. (예를들면 Database에 직접 접근하지 않아도 된다.)
- 다만, 이는 엄청난 수의 interface 와 boilerplate 코드를 만들어야 한다.
  - Testability를 위해, fake 구현을 만드는것도 복잡하다.

- 코드 참고
### 8. Fold
- `Aggregate: IEnumerable<T> -> Acc -> (Acc -> T -> Acc) -> Acc'

### 8. Validation 
- Fail Fast
- Collect Errors
