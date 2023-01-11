#RoboRobo - v1.0.1

## Unity 버전
### - Unity 2021.3.11.f1

## 수정 내역
### 코드 정리 진행 중
#### Enemy HFSM으로 수정 중

### 애니메이션 stop으로 멈추면 초기 값으로 돌아가는게 아닌 현재 상태에서 멈춘다 
애니메이션 해결 방법 찾아 보는 중

### 씬 불러오기에서 missing reference exception 문제 해결
event에 연결하지 않고 gameManager에 controller 로 참고하고 있어서<br/>제한자를 public으로 바꾸고 불러오는 형식으로 했다

### stats 구분하기
싱글 게임이라 1인용으로 Enemy의 경우 static으로 awake에서 플레이어의 stat가져와 모두가 함께 참고하게 하고 플레이어는 공격할 때 참고하게 하기<br/>비슷하게 atkWaitTime도 Enemy는 공유해보기

### 내일 할 꺼
영상 찍어서 시작화면에 영상을 누르면 화면 넘어가게 전환하기!
