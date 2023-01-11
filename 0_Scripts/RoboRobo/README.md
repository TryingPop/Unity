#RoboRobo - v1.0.1

## Unity 버전
### - Unity 2021.3.11.f1

## 수정 내역
### 코드 정리 진행 중
#### Enemy HFSM으로 수정 중

### 애니메이션 stop으로 멈추면 초기 값으로 돌아가는게 아닌 현재 상태에서 멈춘다 
애니메이션 해결 방법 찾아 보는 중

### 씬 불러오기에서 missing reference exception 문제 발생
원인이 씬 불러오면서 새롭게 생성되는 캐릭터가 game manager reset이벤트에 연결되어
reset이벤트를 발동할 때마다 미싱 예외가 뜨는 것으로 보임