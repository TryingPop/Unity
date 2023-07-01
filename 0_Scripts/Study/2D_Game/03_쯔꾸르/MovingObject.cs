using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{

    public float walkSpeed;
    public float runSpeed;

    private float applySpeed;

    private bool applyRunFlag = false;

    private Vector3 vector;

    public int walkCount;
    private int currentWalkCount;

    private bool canMove = true;

    private void Start()
    {

        
    }

    private void Update()
    {

        // 좌 방향키 -1, 우 방향키 1 리턴
        // 상 방향키 1, 하 방향키 -1
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {

            if (canMove)
            {

                StartCoroutine(MoveCoroutine());
                canMove = false;
            }
        }
    }

    IEnumerator MoveCoroutine()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {

            applySpeed = runSpeed;
            applyRunFlag = true;
        }
        else
        {

            applySpeed = walkSpeed;
            applyRunFlag = false;
        }

        vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

        while (currentWalkCount < walkCount)
        {
            if (vector.x != 0)
            {

                // 현재 위치에서 해당 값만큼 이동
                transform.Translate(vector.x * applySpeed * walkCount, 0, 0);
            }
            else if (vector.y != 0)
            {

                transform.Translate(0, vector.y * applySpeed * walkCount, 0);
            }

            if (applyRunFlag) currentWalkCount++;

            currentWalkCount++;
            yield return new WaitForSeconds(0.01f);
        }

        currentWalkCount = 0;
        canMove = true;
    }
}
