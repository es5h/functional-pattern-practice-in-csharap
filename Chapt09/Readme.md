## Currying?

## 1. Curry

다변수 함수를 단변수 함수들의 연속으로 변환하는 것을 말한다.
예를 들어 아래와 같은 이변수 함수가 있다고 하자.

```math
f(x, y) = f_{x}(y)
```

위는
```math
f : X \times Y \rightarrow Z
```
를 
```math
f: X \rightarrow (Y \rightarrow Z)
```
로 변환 하는 과정이다.

커링을 사용하면 부분적용 (Partial Application) 을 사용 할 수 있어, 코드의 재사용성과 모듈성을 높일 수 있다.
arrow notation 은 right-associative 이므로, 아래와 같이 표현해도 무방하다.

```math
f: X \rightarrow Y \rightarrow Z
```
