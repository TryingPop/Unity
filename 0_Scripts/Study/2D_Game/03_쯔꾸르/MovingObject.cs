using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private Animator animator;
    private BoxCollider2D boxCollider;

    public LayerMask layerMask;             // ��� �Ұ����� ���̾� ����

    private void Start()
    {

        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }   

    private void Update()
    {

        // �� ����Ű -1, �� ����Ű 1 ����
        // �� ����Ű 1, �� ����Ű -1
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

        // �ڷ�ƾ ������ ���� ����� �Ҹ��ϱ⿡ �ڷ�ƾ ������ �ּ�ȭ
        while (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
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

            // �¿츦 �ֿ켱���� ����
            if (vector.x != 0)
            {

                vector.y = 0;
            }

            // �ִϸ����Ϳ� ����Ʈ���� DirX, DirY�� ���� ����
            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            RaycastHit2D hit;

            Vector2 start = boxCollider.bounds.center;  // ĳ������ ���� ��ġ �� 
                                                        // �ڽ��ݶ��̴� �����̹Ƿ� �ڽ��ݶ��̴� �߽��� �������� �ߴ�
            Vector2 end = start + new Vector2(vector.x * applySpeed * walkCount, vector.y * applySpeed * walkCount);    // ĳ���Ͱ� �̵��ϰ��� �ϴ� ��ġ ��

            boxCollider.enabled = false;
            hit = Physics2D.Linecast(start, end, layerMask);
            boxCollider.enabled = true;

            if (hit.transform != null)
            {

                break;
            }

            animator.SetBool("Walking", true);

            while (currentWalkCount < walkCount)
            {
                if (vector.x != 0)
                {

                    // ���� ��ġ���� �ش� ����ŭ �̵�
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
        }

        animator.SetBool("Walking", false);
        currentWalkCount = 0;
        canMove = true;
    }
}
