using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tank : MonoBehaviour
{

    GameObject goShell = null;  // 포탄
    bool action = false;        // 행동 확인용 변수

    // 현재 Component.rigidbody2D는 사용 안되므로 다음과 같이 추가
    // 콘솔에서 Component.rigidbody2D가 가려진다는 경고 메시지가 떠도 사용되지 않는 속성이라고
    // obsolete 어트리뷰트가 알려주기에 같은 변수 명 사용
    Rigidbody2D rigidbody2D;
    
    void Start()
    {

        // 해당 오브젝트의 Rigidbody2D 컴포넌트 가져오기
        rigidbody2D = GetComponent<Rigidbody2D>();

        // 포탄 게임 오브젝트를 가져오고, 포탄을 표시하지 않게 설정
        goShell = transform.Find("Tank_Shell").gameObject;
        goShell.SetActive(false);
    }


    void Update()
    {


        // 버튼을 눌렀는가?
        if (Input.GetMouseButton(0))
        {

            // 탱크를 클릭했는가?
            Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collition2d = Physics2D.OverlapPoint(tapPoint);

            if (collition2d)
            {

                if (collition2d.gameObject == gameObject)
                {

                    // 액션 유효화
                    action = true;
                }
            }

            // 버튼을 누른 상태인가?
            if (action)
            {

                // 탱크 이동
                rigidbody2D.AddForce(new Vector2(+30.0f, 0.0f));
            }
        }
        else
        {

            // 버튼을 놓았는가?
            if (Input.GetMouseButtonUp(0) && action)
            {

                // 포탄 발사
                if (goShell)
                {

                    goShell.SetActive(true);
                    goShell.GetComponent<Rigidbody2D>().AddForce(new Vector2(300.0f, 500.0f));
                    Destroy(goShell.gameObject, 3.0f);
                }
                action = false;
            }
        }
    }

    void OnGUI()
    {

        // 왼쪽 상단에 게임 설명 문구 추가
        GUI.TextField(new Rect(10, 10, 300, 60), "[Unity 2D Sample 2-1]\n" 
            + "탱크를 클릭하면 가속\n놓으면 발사!");

        if (GUI.Button(new Rect(10, 80, 100, 20), "다시 시작"))
        {

            // Application.LoadLevel(Application.loadedLevelName);  // 현재는 사용 안하는 코드
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

