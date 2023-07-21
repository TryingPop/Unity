using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformMove : MonoBehaviour
{

    [SerializeField] private float walkSpeed;       // �ȴ� �ӵ�
    [SerializeField] private float runSpeed;        // �޸��� �ӵ�
    private float applySpeed;                       // ���� ���� �ӵ�

    private float hAxis;                            // h �� �Է� ����
    private float vAxis;                            // v �� �Է� ����

    private Vector3 moveDir;                        // �̵� ����

    private Animator playerAnimator;                // �÷��̾� �ִϸ�����
    private float moveAnim;                         // �ȴ� �ִϸ��̼� �ӵ�

    [SerializeField] private bool runAnim;          // �޸��� �ӵ� or �ȴ� �ӵ� ���� ����

    private void Awake()
    {

        playerAnimator = GetComponentInChildren<Animator>();
    }

    
    private void FixedUpdate()
    {

        // InputManager�� �ִ� Ű ���ÿ� ��ϵ� �����̴�
        // Ű���� ����Ű�� WASD�� ������ �۵��Ѵ�
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        // ����ȭ�ؼ� ũ�� ����
        moveDir = new Vector3(hAxis, 0, vAxis).normalized;

        // �̵�
        applySpeed = runAnim ? runSpeed : walkSpeed;

        transform.position += moveDir * applySpeed * Time.deltaTime;

        // ���⸦ �ִϸ��̼ǿ��� ó���ϰ� �ʹ�
        // ���� �˾ƺ��� ���̴�
        if (moveDir.magnitude > 0)
        {

            moveAnim += Time.deltaTime;     
        }
        else
        {

            moveAnim -= 3 * Time.deltaTime;
        }

        moveAnim = Mathf.Clamp(moveAnim, 0, 2f);
        playerAnimator.SetFloat("Move", moveAnim);

        // �̵��ϴ� �������� �ٶ󺸱�
        transform.LookAt(transform.position + moveDir);
    }

    /// <summary>
    /// �ִϸ��̼ǿ� �̺�Ʈ, 0 : �ȴ� �ӵ�, 1 : �޸��� �ӵ�
    /// </summary>
    /// <param name="num"></param>
    public void OnAnimationChkRun(int num)
    {

        runAnim = num == 0 ? false : true;
    }
}
