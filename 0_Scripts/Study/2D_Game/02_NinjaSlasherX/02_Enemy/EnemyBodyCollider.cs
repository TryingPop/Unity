using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyCollider : MonoBehaviour
{

    EnemyController enemyCtrl;
    Animator playerAnim;
    int attackHash = 0;

    void Awake()
    {

        enemyCtrl = GetComponentInParent<EnemyController>();
        playerAnim = PlayerController.GetAnimator();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        // Debug.Log("Enemy OnTriggerEnter2D : " + collision.name);

        if (enemyCtrl.cameraRendered)
        {

            if (collision.tag == "PlayerArm")
            {

                AnimatorStateInfo stateInfo =
                    playerAnim.GetCurrentAnimatorStateInfo(0);
                if (attackHash != stateInfo.fullPathHash)
                {

                    attackHash = stateInfo.fullPathHash;
                    enemyCtrl.ActionDamage();
                    Camera.main.GetComponent<CameraFollow>().AddCameraSize(-0.01f, -0.3f);
                }
            }
            else if (collision.tag == "PlayerArmBullet")
            {

                Destroy(collision.gameObject);
                enemyCtrl.ActionDamage();
            }
        }
    }

    private void Update()
    {

        AnimatorStateInfo stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
        if (attackHash != 0 && stateInfo.fullPathHash == PlayerController.ANISTS_Idle)
        {

            attackHash = 0;
        }
    }
}
