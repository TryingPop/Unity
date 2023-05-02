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
}
