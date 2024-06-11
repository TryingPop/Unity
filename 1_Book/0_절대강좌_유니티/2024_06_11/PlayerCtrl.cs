using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{

    // �̸� ������ ��Ƶΰ� �ش� ������ �����ϴ� ����� �̼������� ������
    // ĳ��ó���� ��ũ��Ʈ���� �����ؾ� �� ������Ʈ�� awake �Լ��� Start �Լ�����
    // �̸� ������ �Ҵ��� �Ŀ� �� ������ ���� �����ϴ� ���� ���Ѵ�
    private Transform tr;
    private Animation anim;


    public float moveSpeed = 10.0f;     // �̵� �ӵ�
    public float turnSpeed = 80.0f;     // ȸ�� �ӵ��� ����


    private void Start()
    {

        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();

        // Idle �̸��� �ִϸ��̼� ����
        anim.Play("Idle");
    }


    void Update()
    {

        float h = Input.GetAxis("Horizontal");      // -1.0f ~ 1.0f
        float v = Input.GetAxis("Vertical");        // -1.0f ~ 1.0f
        float r = Input.GetAxis("Mouse X");

        // Debug.Log("h = " + h);
        // Debug.Log("v = " + v);

        // Transform ������Ʈ�� ��ġ������
        // transform.position += new Vector3(0, 0, 1);

        // ����ȭ ���͸� ����� �ڵ�
        // transform.position += Vector3.forward * 1;
        // tr.Translate(Vector3.forward * 1.0f);

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        tr.Translate(moveDir.normalized * Time.deltaTime * moveSpeed);

        tr.Rotate(Vector3.up * turnSpeed * Time.deltaTime * r);

        // ���ΰ� ĳ������ �ִϸ��̼� ����
        PlayerAnim(h, v);
    }

    void PlayerAnim(float _h, float _v)
    {

        // 0.25�� ���� �ִϸ��̼��� ����Ǵ� �ڵ��
        // �ִϸ��̼��� Ű �����̸� ������ �ε巴�� ���� ��Ų��
        // ���� �ִϸ��̼� ����
        if (_v >= 0.1f) anim.CrossFade("RunF", 0.25f);
        // ���� �ִϸ��̼� ����
        else if (_v <= -0.1f) anim.CrossFade("RunB", 0.25f);
        // ������ �̵� �ִϸ��̼� ����
        else if (_h >= 0.1f) anim.CrossFade("RunR", 0.25f);
        // ���� �̵� �ִϸ��̼� ����
        else if (_h <= -0.1f) anim.CrossFade("RunL", 0.25f);
        // ���� �� Idle �ִϸ��̼� ����
        else anim.CrossFade("Idle", 0.25f);
    }
}
