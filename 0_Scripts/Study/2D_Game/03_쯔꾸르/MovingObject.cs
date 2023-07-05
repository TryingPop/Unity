using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float walkSpeed;

    public int walkCount;
    protected int currentWalkCount;

    protected Vector3 vector;

    protected BoxCollider2D boxCollider;
    public LayerMask layerMask;
    protected Animator animator;

    protected bool npcCanMove;

    protected void Move(string _dir, int _frequency)
    {
        
        StartCoroutine(MoveCoroutine(_dir, _frequency));
    }

    private IEnumerator MoveCoroutine(string _dir, int _frequency)
    {

        npcCanMove = false;
        vector.Set(0, 0, vector.z);

        switch (_dir)
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

        while (currentWalkCount < walkCount)
        {

            transform.Translate(vector.x * walkSpeed, vector.y * walkSpeed, 0);

            currentWalkCount++;
            yield return new WaitForSeconds(0.01f);
        }

        currentWalkCount = 0;

        if (_frequency != 5)
        {

            animator.SetBool("Walking", false);
        }

        npcCanMove = true;
    }
}
