using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject_BlockA : MonoBehaviour
{

    bool destroyed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!destroyed && collision.tag != null)
        {

            if (collision.tag == "PlayerArm" ||
                collision.tag == "PlayerArmBullet" ||
                collision.tag == "EnemyArm" ||
                collision.tag == "EnemyArmBullet")
            {

                destroyed = true;
                GetComponent<Animator>().enabled = true;
                GetComponent<Animator>().SetTrigger("Destroy");
                Destroy(gameObject, 0.5f);

                if (collision.tag == "EnemyArmBullet")
                {

                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
