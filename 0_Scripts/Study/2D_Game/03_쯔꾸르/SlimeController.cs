using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MovingObject
{

    // public int atk;                     // 슬라임 공격력 다른 EnemyStat의 스텟에 있다
    public float attackDelay;           // 공격 유예

    public float inter_MoveWaitTime;    // 대기 시간

    private float current_interMWT;

    public string atkSound;

    private Vector2 playerPos;          // 플레이어 좌표 값

    private int random_int;             // 방향 설정
    private string direction;

    private EnemyStat theStat;
    public GameObject healthBar;

    private void Start()
    {

        queue = new Queue<string>();
        current_interMWT = inter_MoveWaitTime;
    }

    private void Update()
    {

        current_interMWT -= Time.deltaTime;

        if (current_interMWT <= 0)
        {

            current_interMWT = inter_MoveWaitTime;

            if (NearPlayer())
            {

                Flip();
                return;
            }

            RandomDirection();

            if (base.CheckCollision())
            {

                return;
            }

            base.Move(direction);
        }
    }

    private void RandomDirection()
    {

        vector.Set(0, 0, vector.z);

        random_int = Random.Range(0, 4);

        switch(random_int)
        {

            case 0:
                vector.y = 1f;
                direction = "UP";
                break;

            case 1:
                vector.y = -1f;
                direction = "DOWN";
                break;

            case 2:
                vector.x = 1f;
                direction = "RIGHT";
                break;

            case 4:
                vector.x = -1f;
                direction = "LEFT";
                break;
        }
    }

    private bool NearPlayer()
    {

        playerPos = PlayerManager.instance.transform.position;

        if (Mathf.Abs(playerPos.x - this.transform.position.x) <= applySpeed * walkCount * 1.01f)
        {

            if (Mathf.Abs(playerPos.y - this.transform.position.y) <= applySpeed * walkCount * 0.5f)
            {

                return true;
            }
        }

        if (Mathf.Abs(playerPos.y - this.transform.position.y) <= applySpeed * walkCount * 1.01f)
        {

            if (Mathf.Abs(playerPos.x - this.transform.position.x) <= applySpeed * walkCount * 0.5f)
            {

                return true;
            }
        }

        return false;
    }

    private void Flip()
    {

        Vector3 flip = transform.localScale;
        
        if (playerPos.x > transform.position.x)
        {

            flip.x = -1f;
        }
        else
        {
            flip.x = 1f;
        }

        this.transform.localScale = flip;
        healthBar.transform.localScale = flip;
        animator.SetTrigger("Attack");
    }

    IEnumerator WaitCoroutine()
    {

        yield return new WaitForSeconds(attackDelay);
        AudioManager.instance.Play(atkSound);

        if (NearPlayer())
        {

            // PlayerStat.instance.Hit(atk);
            PlayerStat.instance.Hit(theStat.atk);
        }
    }
}
