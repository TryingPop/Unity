using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingObject : MonoBehaviour
{

    public string characterName;

    public float walkSpeed;
    protected float applySpeed;

    public int walkCount;
    protected int currentWalkCount;

    protected Vector3 vector;

    public BoxCollider2D boxCollider;
    public LayerMask layerMask;
    public Animator animator;

    public Queue<string> queue;     // 선입선출(FIFO) 자료구조

    private bool notCoroutine = false;  // 코루틴 반복을 막기 위해 넣은 bool 변수

    public void Move(string _dir, int _frequency = 5)
    {

        queue.Enqueue(_dir);
        if (!notCoroutine)
        {

            notCoroutine = true;
            StartCoroutine(MoveCoroutine(_dir, _frequency));
        }
    }

    private IEnumerator MoveCoroutine(string _dir, int _frequency)
    {
        while (queue.Count > 0)
        {

            string direction = queue.Dequeue();
            vector.Set(0, 0, vector.z);

            switch (direction)
            {

                case "UP":
                    vector.y = 1f;
                    break;

                case "DOWN":
                    vector.y = -1f;
                    break;

                case "RIGHT":
                    vector.x = 1f;
                    break;

                case "LEFT":
                    vector.x = -1f;
                    break;
            }

            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            animator.SetBool("Walkinig", true);

            applySpeed = walkSpeed;

            while (currentWalkCount < walkCount)
            {

                transform.Translate(vector.x * applySpeed, vector.y * applySpeed, 0);

                currentWalkCount++;
                yield return new WaitForSeconds(0.01f);
            }

            currentWalkCount = 0;

            if (_frequency != 5)
            {

                animator.SetBool("Walking", false);
            }

            notCoroutine = false;
        }
    }

    protected bool CheckCollision()
    {

        RaycastHit2D hit;

        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(vector.x * applySpeed * walkCount, vector.y * vector.y * applySpeed * walkCount); ;

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, layerMask);
        boxCollider.enabled = false;

        if (hit.transform != null) return true;

        return false;
    }
}
