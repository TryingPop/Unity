using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.name == "Tank_Shell")
        {

            Debug.Log(">>>>>>>>>>>>>>>>> Hit!");
            // transform.localScale += new Vector3(1.0f, 1.0f, 1.0f);   // 클래스 변수로 크기가 1인 벡터 3가 정의 되어져 있으므로
                                                                        // 생성하기 보다는 있는걸 쓴다
            transform.localScale += Vector3.one;
            // rigidbody2D.AddForce(new Vector2(1000.0f, -1000.0f));    
            GetComponent<Rigidbody2D>().AddForce(new Vector2(1000.0f, -1000.0f));   // 앞에서는 GetComponent를 매 프레임마다 불러와서 성능 저하 문제로 rigidbody2D 변수를 지정해서 썼는데
                                                                                    // 매 프레임마다 체크 하는게 아닌 1번만 체크해서 GetComponent 이용
        }
    }
}
