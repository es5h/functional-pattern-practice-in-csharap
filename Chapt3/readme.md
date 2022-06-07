수학적 함수를 pure function이라고 말해보자.. 모든 app이 pure function 으로만 모든걸 구현 할 수 있는건 아니다.

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


함수가 pure 하다면, Parallelization, Lazy Evaluation, Memoization 이 가능


Linq 를 사용할때 AsParrell 키워드를 사용하면 거의 free 하게 병렬화 계산을 할 수 있다. 왜 그냥 free가 아니고 '거의' 일까? 런타임 땐, 적용한 function 이 pure 인지 아닌지 알 법이 없어서 직접 AsParellel 을 알아서 판단해야 해줘서 그냥 free가 아니다.

## Concurrency
  - Asynchrony
  - Parallelism
  - Multithreading


Method가 input 에만 의존하면 static 으로 정의할 수 있다. 하지만 static 을 과도하게 사용하는데 불안감을 느낄 수도 있다. 아래와 같은 상황에서 문제를 일으키기 때문이다.
- static field 를 변화 시킬때 : static class 가 붙어있는 static field 들은 언제나 변할 수 있는데, 예측 불가능한 coupling 으로부터 예측 불가능한 동작이 발생할 수도 있다.
- I/O 를 할경우 : testability 가 안좋아진다. Method A가 Method B의 IO에 의존한다면, A의 단위테스트는 가능하지 않다.
위의 두가진 method가 impure 함을 의미한다.

함수가 pure 하면 static method 를 안전하게 만들 수 있는데, 다음과 같은 지침을 따르도록 하자.
- pure function 은 static으로 만들자.
- 변할 수 있는 static field는 피하자.
- I/O 를 하는 정적 메서드를 직접 부르지말자.

함수형으로 코딩할수록, 함수들은 더 pure 해질 것이고, 코드 대부분이 static class 에 있게 될것이다.


## isolating i/o effects
io를 수행하는 함수는 pure 할수가 없다.

- url 을 입력하면 리소스를 보여 주는 메서드 : 연결이 끊기면 error 을 throw, remote resource가 바뀌면 return 이 달라짐
- file path를 입력하면 컨텐츠를 입력하는 메서드 : 쓰기 권한이 없거나 directory 등이 없으면 error throw
- 특정 시점에 system clock 을 return 하는 메서드

io 영역과 로직을 잘 분리하자.

DateTimeValidator 예제 참고.

### 객체 지향 패턴에 DI 방식으로 테스트를 분리 할 수 있다.
1. IO를 추상화하는 interface 정의, 구현체에 impure 매서드를 배치한다.
2. 테스트를 할 클래스에 interface를 요구한다음 필드에 넣는다. (DI)
3. 테스트를 위해 가짜 클래스를 구현한후 주입한다.
4. 테스트 클래스가 돌때 impure 구현이 돌도록 몇가지 작업을 더한다.
=> net core 같은 경우 service와 의존성을 등록하는 행위

위 방식은 정석이고 훌륭하지만, 저자는 anti pattern으로도 볼수 있는 관점을 줌. bolierplate 가 많다.
