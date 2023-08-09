using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _2_Reposition : MonoBehaviour
{

    public int tileSizeX = 20;
    public int tileSizeY = 20;

    Collider2D coll;

    private void Awake()
    {

        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (!collision.CompareTag("Area"))
        {

            return;
        }

        Vector3 playerPos = _3_GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        // ���� ���� �ڵ�
        // float diffX = Mathf.Abs(playerPos.x - myPos.x);
        // float diffY = Mathf.Abs(playerPos.y - myPos.y);

        // ���⼭ inputVec�� public ���� ������Ѵ�
        Vector3 playerDir = _3_GameManager.instance.player.inputVec;
        // float dirX = playerDir.x < 0 ? -1 : 1;
        // float dirY = playerDir.y < 0 ? -1 : 1;

        float dirX = playerPos.x - myPos.x;
        float dirY = playerPos.y - myPos.y;

        // float diffX = Mathf.Abs(dirX);
        // float diffY = Mathf.Abs(dirY);

        dirX = (int)dirX / tileSizeX;
        dirY = (int)dirY / tileSizeY;

        switch (transform.tag)
        {
            
            case "Ground":

                transform.Translate(Vector3.right * dirX * tileSizeX * 2);
                transform.Translate(Vector3.up * dirY * tileSizeY * 2);
                break;

            case "Enemy":

                // �ݶ��̴��� Ȱ��ȭ = ����ִ�
                if (coll.enabled)
                {

                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist * 2);
                }
                break;
        }
    }
}