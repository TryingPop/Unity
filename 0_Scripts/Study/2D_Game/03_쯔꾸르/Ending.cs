using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{

    public GameObject go;   // panel 캔버스
                            // 여기에는 아래에서 위로 올라가는 애니메이션과
                            // 엔딩 내용을 담고 있는 Text가 있다
                            // 움직임도 막아야하는데 간단하게 만들었다

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (Input.GetKeyDown(KeyCode.Z))
        {

            go.SetActive(true);
        }
    }
}
