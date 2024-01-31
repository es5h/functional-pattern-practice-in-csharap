## 1. 사상(mapping)으로서의 함수
### 1. 함수
수학에서의 함수는 정의역과 공역으로 이루어진 두 집합 사이의 관계로 정의된다.

```csharp
Func<int, int> f = (int x) => x + 1;
```
위의 예시의 함수는 정의역 int, 공역 int 두 집합 사이의 관계로 정의된다.
(int -> int) 로 표현될 수도 있다.

C#에선 해당 함수를 Local Function, Delegate, Lambda Expression, Dictionary 등으로 표현 가능하다.

### 2. 고차 함수
함수를 인자로 받거나, 함수를 반환하는 함수를 고차 함수라고 한다.
범함수(Functional) 또는 Operator 라고도 한다.

일반역학에 익숙하다면 Lagrangian, Hamiltonian 등이 이에 해당한다는 것을 알 수 있다.

```csharp
public static class EnumerableExt
{
  public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
  {
    foreach (T item in source)
    {
      if (predicate(item))
      {
        yield return item;
      }
    }
  }
}
```
IEnumerable 의 Where 메서드는 고차 함수의 예시이다.
Where 메서드는 인자로 받은 predicate 함수를 인자로 받는다.
