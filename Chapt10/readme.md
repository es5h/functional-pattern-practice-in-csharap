a Monad M 은 다음과 같은 함수에 의해 정의됨
- Return => t $ \in $ regular T를 m $ \in $M<T>로 Lifting 하는 함수
- Bind => m 과 world-crossing f

there exists t such that m = M(t) and m(M) == bind 

Return 과 Bind는 
Right Identity / Left Identity / Associativaty 를 만족해야한다.


## Right Identity
```c#
m == m.Bind(Return)
```

