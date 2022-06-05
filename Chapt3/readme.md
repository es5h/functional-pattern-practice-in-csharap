수학적 함수를 pure function과 유사하게 보자. 특정 app은 pure function 으로만 모든걸 구현 할 수없다.

impure function 같은 기능도 필요하다. 함수를 표현 하기 위한 구조들 (인스턴스 메서드는 인스턴스 필도를 참조, 람다는 주변 변수에 의존 하는등) 은 문맥에 접근한다. 데이터 베이스나 시스템 시계, 외부 api 등에 의존 하는 경우도 있다.

pure/ impure function 의 구분이 필요하다.

- pure function 
  - output은 입력 argument 에만 의존 
  - side effect 를 일으키지 않는다.
- impure function 
  - 입력 argument 외의 요소가 output에 영향을 끼친다
  - side effect 를 일으킨다
- side effect
  - global 상태를 mutate 한다
  - input arguments 를 mutate 한다.
  - exception 을 던진다
    - 예외를 발생시키면 호출의 결과가 context 따라 달라질 수 있다.
  - IO operation 을 한다
    - file io/ console io/ 앱 외부와의 교환등
    
추가로, side effect 가 없는 함수도 impure 할 수는 있다. output이 global mutate state 에 접근할 경우(의존할 경우) input 에만 의존하지 않을 수도 있다.
output이 input 에만 의존한다고 해도 global state 를 mutate 한다면 impure 할 수도 있다.


함수가 pure 하다면, Parallelization, Lazy Evaluation, 