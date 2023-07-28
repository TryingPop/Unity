using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _2_Reposition : MonoBehaviour
{

    public int tileSizeX = 20;
    public int tileSizeY = 20;

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (!collision.CompareTag("Area"))
        {

            return;
        }

        Vector3 playerPos = _3_GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        // 기존 강의 코드
        // float diffX = Mathf.Abs(playerPos.x - myPos.x);
        // float diffY = Mathf.Abs(playerPos.y - myPos.y);

        // 여기서 inputVec은 public 선언 해줘야한다
        // Vector3 playerDir = _3_GameManager.instance.player.inputVec;
        // float dirX = playerDir.x < 0 ? -1 : 1;
        // float dirY = playerDir.y < 0 ? -1 : 1;

        float dirX = playerPos.x - myPos.x;
        float dirY = playerPos.y - myPos.y;

        float diffX = Mathf.Abs(dirX);
        float diffY = Mathf.Abs(dirY);

        dirX = dirX > 0 ? 1 : -1;
        dirY = dirY > 0 ? 1 : -1;

        switch (transform.tag)
        {

            case "Ground":

                if (diffX > diffY)
                {

                    transform.Translate(Vector3.right * dirX * tileSizeX * 2);
                }
                else if (diffX < diffY)
                {

                    transform.Translate(Vector3.up * dirY * tileSizeY * 2);
                }
                break;

            case "Enemy":

                break;
        }
    }
}