## Functional Programming 이란?

### 0. 정리

두가지 기본 개념이 있다.
- Functions 는 일급 객체이다.
- State Mutation 이 없다.

### 1. 일급 객체로서의 함수
C# 에서 함수(정확히는 메서드)는 delegate 나 Lambda Expression 을 통해 일급 객체로 간주 할 수 있다.

```csharp
// delegate
public delegate int Add(int a, int b);

// Lambda Expression
Func<int, int> mod2 = (int x) => x % 2;
```

### 2. State Mutation 이 없다.
State Mutation 이란, 메모리의 변수가 실제 변경을 일으키는 것을 의미한다.

Sort 메서드의 예시를 들어보자.
```csharp
int[] nums = [3, 6, 2, 1];

  nums.OrderBy(x => x).ToList().ForEach(WriteLine); // 1, 2, 3, 6
  nums.ToList().ForEach(WriteLine); // 3, 6, 2, 1

  Array.Sort(nums);
  nums.ToList().ForEach(WriteLine); // 1, 2, 3, 6
```
실제로 기존 nums의 값이 변경되는 것을 볼 수 있다.
여러 스레드에서 동시에 nums에 접근하게 될 경우, 이는 예상치 못한 결과 (side effect) 를 초래하게 된다.

### 추가 내용
C# 은 언어 차원에서 FP를 유용하게 사용할 수 있는 다양한 지원이 있다.
- 튜플
  - FP는 작은 단위의 함수로 프로그램을 구성되게 된다. 이때 도메인 의미가 약한 값들을 묶어서 사용할 때 유용하다. 
- 패턴매칭
  - FP Context에서 Match 의 개념에 대응될 수 있다. 
- Records
  - Immutable 한 데이터를 표현하기 위한 기능 
- LINQ
등등
