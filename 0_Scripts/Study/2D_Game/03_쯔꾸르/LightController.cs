using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{

    private PlayerManager thePlayer;        // 플레이어가 바라보고 있는 방향
    private Vector2 vector;

    private Quaternion rotation;            // 회전(각도)을 담당하는 Vector4 x, y, z, w

    private void Start()
    {
        
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    private void LateUpdate()
    {
        
        this.transform.position = thePlayer.transform.position;
        vector.Set(thePlayer.animator.GetFloat("DirX"), thePlayer.animator.GetFloat("DirY"));

        if (vector.x == 1f)
        {

            rotation = Quaternion.Euler(0, 0, 90);
            this.transform.rotation = rotation;
            // this.transform.rotation = new Vector4(0, 0, 90, 0);  // 오류가 난다
        }
        else if (vector.x == -1f)
        {

            rotation = Quaternion.Euler(0, 0, -90);
            this.transform.rotation = rotation;
        }
        else if (vector.y == 1f)
        {

            rotation = Quaternion.Euler(0, 0, 180);
            this.transform.rotation = rotation;
        }
        else if (vector.y == -1f)
        {

            rotation = Quaternion.Euler(0, 0, 0);
            this.transform.rotation = rotation;
        }
    }
}
