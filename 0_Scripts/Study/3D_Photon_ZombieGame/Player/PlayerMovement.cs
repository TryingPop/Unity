using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������� �Է¿� ���� �÷��̾� ĳ���͸� �����̴� ��ũ��Ʈ
public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;        // �յ� �������� �ӵ�
    public float rotateSpeed = 180f;    // �¿� ȸ�� �ӵ�

    private PlayerInput playerInput;    // �÷��̾� �Է��� �˷��ִ� ������Ʈ

    private Rigidbody playerRigidbody;  // �÷��̾� ĳ������ ������ٵ�
    private Animator playerAnimator;    // �÷��̾� ĳ������ �ִϸ�����

    private void Start()
    {
        
        // ����� ������Ʈ���� ���� ��������
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {

        // ���� ���� �ֱ⸶�� ������, ȸ��, �ִϸ��̼� ó�� ����
        // ȸ�� ����
        Rotate();

        // ������ ����
        Move();

        // �Է°��� ���� �ֈ��������� Move �Ķ���Ͱ� ����
        playerAnimator.SetFloat("Move", playerInput.move);
    }

    // �Է°��� ���� ĳ���͸� �յڷ� ������
    private void Move()
    {

        Vector3 moveDistance =
            playerInput.move * transform.forward * moveSpeed * Time.fixedDeltaTime;
        // ���翡���� Time.deltaTime�̶� �Ǿ��� �ִµ�, fixedUpdate �������� ����
        // ��ǻ�� ������ ������ �������� ���� �̵��ӵ��� ���� ��ǻ�� ������ �������� ���� �ӵ��� ���⿡ fixedDeltaTime���� ����

        // ������ٵ� �̿��� ���� ������Ʈ ��ġ ����
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    // �Է°��� ���� ĳ���͸� �¿�� ȸ��
    private void Rotate()
    {

        // ��������� ȸ���� ��ġ ���
        float turn = playerInput.rotate * rotateSpeed * Time.fixedDeltaTime;

        // ������ٵ� �̿��� ���� ������Ʈ ȸ�� ����
        playerRigidbody.rotation =
            playerRigidbody.rotation * Quaternion.Euler(0, turn, 0f);
        // ���� ȸ�� ���¿��� �� ȸ���ϰ� ���� ���� ���ʹϾ� ���� �ַ� �̿��Ѵ�!
    }
}