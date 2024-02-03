## Option?

### 1. Option Type

- 심볼릭 정의
```
Option<T> = Some(T) | None
```
- `None`: 데이터의 부재를 표현, 내부 값이 없을 경우 Option은 None이다.
- `Some(T)`: T의 값을 래핑 한다. 내부 값이 있을 경우 Option은 Some이다.

### 2. Total Function and Partial Function

- Total Function 
  - 정의역 내의 모든 값에 대해 정의된 함수
- Partial Function
  - 정의역 내의 일부 값에 대해 정의된 함수

Partial Function 은 항상 결과값을 보장 할 수 없기 때문에 문제를 일으킨다.
공역의 Option Type을 사용하면, 결과값이 없을 경우 None을 반환하고, 결과값이 있을 경우 Some을 반환함으로써, Total Function으로 만들 수 있다.

**예시**
`int.Parse`(`string -> int`) 는 Partial Function 이다. Parese가 실패할 경우 Exception을 던져, int를 반환하지 않는다.

### 3. Null 을 안전하게 다루려면?
null 이 넘어가지 않는 다는 것을 보장하기 위해, null check과 ArgumentNullException을 던지지 않게 한다.
- NRT 활성화
- T? 대신 Option<T> 사용
코드 경계를 파악해서, 라이브러리용 public method, web api, message broker 등에 null 이 스며들지 않게한다.
- 필수 값의 경우, ArgunemtNullException을 던지거나, 400 Bad Request를 반환한다, messge를 거절한다.
