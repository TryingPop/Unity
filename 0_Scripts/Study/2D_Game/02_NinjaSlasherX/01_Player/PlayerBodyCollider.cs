using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyCollider : MonoBehaviour
{

    PlayerController playerCtrl;

    private void Awake()
    {
        playerCtrl = transform.parent.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        // Debug.Log("Player OnTriggerEnter2D : " + collision.name);

        // 트리거 검사
        if (collision.tag == "EnemyArm")
        {

            EnemyController enemyCtrl = collision.GetComponentInParent<EnemyController>();
            // Debug.Log(string.Format("EnemyArm Hit {0}", enemyCtrl.attackEnable));
            if (enemyCtrl.attackEnabled)
            {

                enemyCtrl.attackEnabled = false;
                playerCtrl.dir =
                    (playerCtrl.transform.position.x < enemyCtrl.transform.position.x) ? +1 : -1;
                playerCtrl.AddForceAnimatorVx(-enemyCtrl.attackNockBackVector.x);
                playerCtrl.AddForceAnimatorVy(enemyCtrl.attackNockBackVector.y);
                playerCtrl.Actiondamage(enemyCtrl.attackDamage);
            }
        }
        else if (collision.tag == "EnemyArmBullet")
        {

            FireBullet fireBullet = collision.transform.GetComponent<FireBullet>();
            if (fireBullet.attackEnabled)
            {

                fireBullet.attackEnabled = false;
                playerCtrl.dir = (playerCtrl.transform.position.x <
                    fireBullet.transform.position.x) ? +1 : -1;
                playerCtrl.AddForceAnimatorVx(-fireBullet.attackNockBackVector.x);
                playerCtrl.AddForceAnimatorVy(-fireBullet.attackNockBackVector.y);
                playerCtrl.Actiondamage(fireBullet.attackDamage);
                Destroy(collision.gameObject);
            }
        }
        else if (collision.tag == "CameraTrigger")
        {

            Camera.main.GetComponent<CameraFollow>().SetCamera(
                collision.GetComponent<StageTrigger_Camera>().param);
        }
        else if (collision.tag == "EventTrigger")
        {

            collision.SendMessage("OnTriggerEnter2D_PlayerEvent", gameObject);
        }
        else if (collision.tag == "Item")
        {

            if (collision.name == "Item_koban")
            {

                PlayerController.score += 10;
            }
            else if(collision.name == "Item_Ohoban")
            {

                PlayerController.score += 100000;
            }
            else if (collision.name == "Item_Hyoutan")
            {

                playerCtrl.SetHp(playerCtrl.hp + playerCtrl.hpMax / 3, playerCtrl.hpMax);
            }
            else if (collision.name == "Item_Makimono")
            {

                // playerCtrl.superMode = true;
                playerCtrl.GetComponentInChildren<Stage_AfterImage>().afterImageEnabled = true;
                // playerCtrl.basScaleX = 2.0f;
                // playerCtrl.transform.localScale = new Vector3(
                //    playerCtrl.basScaleX, 2.0f, 1.0f);
                // Invoke("SuperModeEnd", 10.0f);
            }
            else if (collision.name == "Item_Key_A")
            {

                PlayerController.score += 10000;
                PlayerController.itemKeyA = true;
                GameObject.Find("Stage_Item_Key_A").
                    GetComponent<SpriteRenderer>().enabled = true;
            }
            else if (collision.name == "Item_Key_B")
            {

                PlayerController.score += 10000;
                PlayerController.itemKeyB = true;
                GameObject.Find("Stage_Item_Key_B").
                    GetComponent<SpriteRenderer>().enabled = true;
            }
            else if (collision.name == "Item_Key_C")
            {

                PlayerController.score += 10000;
                PlayerController.itemKeyC = true;
                GameObject.Find("Stage_Item_Key_C").
                    GetComponent<SpriteRenderer>().enabled = true;
            }


            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (!playerCtrl.jumped && 
            (collision.gameObject.tag == "Road" ||
            collision.gameObject.tag == "MoveObject" ||
            collision.gameObject.tag == "Enemy"))
        {

            playerCtrl.groundY = transform.parent.transform.position.y;
        }
    }

    void SuperModeEnd()
    {

        // playerCtrl.superMode = false;
        playerCtrl.GetComponent<Stage_AfterImage>().afterImageEnabled = false;
        playerCtrl.basScaleX = 1.0f;
        playerCtrl.transform.localScale =
            new Vector3(playerCtrl.basScaleX, 1.0f, 1.0f);
    }
}
