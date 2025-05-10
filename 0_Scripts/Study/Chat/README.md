# 채팅 
네트워크 연습용 코드


# 주요 클래스
ChatUIs, ChatServer, ChatClnt<br/>


## ChatServer
채팅 서버 클래스이다.<br/>
ServerClient 클래스 정의도 포함되어 있는데, 해당 클래스는 단순히 닉네임과, socket을 보유 중이다.<br/>


소켓 연결은 비동기로 진행한다.<br/>
그리고 매 Update마다 소켓이 연결되었는지 확인하고 전송된 메세지가 있는지 확인한다.<br/>


연결된 클라이언트 소켓을 HashSet에 보관한다.<br/>
네트워크 송신간 string을 이용한다.<br/>
\&NAME의 문자열을 받은 경우 연결을 뜻한다.<br/>


## ChatClnt
채팅 클라이언트 클래스다.<br/>
강제 종료 되었을 시에 소켓을 종료한다.<br/>
닉네임과 메시지를 구분하는 것으로 \|를 이용한다.<br/>
처음 연결되었을 때 \"\&NAME\|닉네임\" 문자열을 전송해 닉네임을 설정한다.<br/>


## ChatUIs
네트워크로 주고 받은 메세지를 UI에 띄어준다.<br/>
싱글 톤 객체이다.<br/>
메시지를 입력받으면 \\n을 추가해 메세지를 구분한다.<br/>


## 참고 사이트
https://www.youtube.com/watch?v=y3FU6d_BpjI