Total Function : 모든 정의역에 정의된 Mapping

Partial Function : 정의역의 일부에만 정의된 Mapping


Parsing Functon 이 대표적인 Partail Function, Option 타입을 이용해서 side effect 최소화

- string => int
- "33" => 33
- "-10' => -10
- "abc" => none


NRT 에도 한계가 있음. Option 타입의 필요. 한계는 아래와 같음

1. #nullable 처리를하고
2. Project 단에서 처리해도, #nullable disable 처리로 overrideing 가능함
3. 선언구역 참조 유역 모두 nrt 지원되는 context에서만 지원되서 아니녻에선 따로 추론하는 것을 어렵게 만든다.


## NullRefrenceException 을 막을려면

1. C# 8.0 이상을 쓴다면, NRT 를 킨다. optional 값에 T 대신 Option<T> 사용
2. 코드의 경계선 (라이브러리 등으로 공유/publish 하려는 코드 (1) / Web APi (2) / 메시지 큐의 메시지 수신기 (3)) 에서 NULL 값이 스며들지 않게 해야한다.
    - For Required Value
      - (1) Throw Argument Exception
      - (2) 400 리턴
      - (3) 메시지 거절
    - For Optional Value
        null 을 option 으로 변경
    - 
