## Function Composition?

### 1. C# 일반적인 함수의 합성
- 예시

```csharp
Func<int, int> addOne = x => x + 1;
Func<int, int> multiplyByTwo = x => x * 2;
Func<int, int> addOneAndMultiplyByTwo = x => multiplyByTwo(addOne(x));
```

Method Chaning (with Extension Method)

```csharp
public static class IntExtensions
{
    public static int AddOne(this int x) => x + 1;
    public static int MultiplyByTwo(this int x) => x * 2;
}

var result = 2.AddOne().MultiplyByTwo();
```

### 2. 고차 세계의 함수 합성
- 예시
```csharp
Option<int> someOne = Some(1);
Option<int> someNumber = someOne.Map(addOne).Map(multiplyByTwo); // Some(4)
```
- Functor Laws
  - 1st law: for all Elements `ct` in `C<T>`, `ct.Map(x => x) == ct`
  - 2nd law: for all Elements `ct` in `C<T>`, `ct.Map(f).Map(g) == ct.Map(x => g(f(x)))`
  - [참고] 위의 두 법칙을 만족하는 것이 당연하게 들릴 수 있지만, Map 함수가 위의 두 법칙을 만족하지 않는다면, 그것은 Functor가 아니다.

### 3. 합성이 잘되도록 만들기
- **순수**하게: Function이 Side Effect가 없어야 한다. (Side Effect가 있다면 재사용되지 않는다.)
- **Chaining**이 되도록: extension method를 사용하여 Method Chaining을 할 수 있도록 만들어야 한다.
- **일반적**이게: 함수가 구체적일 수록, 재사용성이 떨어진다.
- **형태를 유지**하도록: 함수의 형태를 유지하도록 만들어야 한다. Option은 Option으로, List는 List로, 등등.
