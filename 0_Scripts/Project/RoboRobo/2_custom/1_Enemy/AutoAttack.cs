using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AutoAttack : Unit
{

    private static Transform targetTrans;           // �÷��̾� transform
    private static PlayerController controller;     // ������ �� �÷��̾� controller

    private Vector3 dir;                            // �ٶ� ����

    private Animation myAnim;                       // �ִϸ��̼�

    public IObjectPool<AutoAttack> poolToReturn;    // ������Ʈ Ǯ
    

    private void Awake()
    {

        // Ÿ���� �ִ��� Ȯ�� ���ٸ� �߰�
        if (targetTrans == null)
        {

            targetTrans = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // ���� �÷��̾� ��Ʈ�ѷ� Ȯ��
        if (controller == null)
        {

            controller = targetTrans.GetComponent<PlayerController>();
        }

        // �ʱ� ������Ʈ ��´�
        // ������ٵ�, �Ҹ� ������Ʈ, ���� ��Ʈ�ѷ�, ���� Ÿ�̸� ����
        GetComp();

        // �ִϸ��̼� ������Ʈ ȹ��
        myAnim = GetComponent<Animation>();

        // ���̾� ����
        myAnim["0_idle"].layer = 0;
        myAnim["1_walk"].layer = 1;
        myAnim["2_attack"].layer = 2;
        myAnim["3_attacked"].layer = 3;

        // ���� �� �̺�Ʈ ����
        myWC.Attack += Attack;
    }

    private void OnEnable()
    {
        
        // ��Ȱ��ȭ �̹Ƿ� ü�� �ʱ�ȭ �� �ִϸ��̼� �ٽ� ���� �׸��� ���� �ڷ�ƾ ����
        Reset();
    }

    private void Update()
    {
        
        // �÷��̾� �������� ������ �ϸ� �̵�
        Move();
    }

    /// <summary>
    /// ü�� ȸ�� �� �� ��Ȱ �� �����ϴ� �ൿ��
    /// </summary>
    public void Reset()
    {

        // ü�� �ʱ�ȭ
        Init();

        // dmgCol.enabled = true;
        myAnim.CrossFade("0_idle", 0.2f);
        myAnim.CrossFade("1_walk", 0.1f);
        myAnim.CrossFade("2_attack", 0.1f);

        // ���� ���ݸ��� ���� �ݶ��̴� Ȱ��ȭ ��Ű�� �ڷ�ƾ ����
        StartCoroutine(Attack());
    }

    /// <summary>
    /// �÷��̾ ���� �̵�
    /// </summary>
    private void Move()
    {

        // ���� ����
        // y ���� ���� y��ǥ�� �����Ѵ�
        dir = targetTrans.position - transform.position;
        dir.y = 0;
        dir = dir.normalized;

        // �������� �̵� �� �ٶ󺸴� ���� ����
        myRd.MovePosition(transform.position + dir * status.MoveSpd * Time.deltaTime);
        transform.LookAt(transform.position + dir);
    }

    /// <summary>
    /// �ǰ� �޼ҵ� �ڽ��� hp ��´�
    /// </summary>
    /// <param name="atk">���ݷ�</param>
    public override void OnDamaged(int atk)
    {

        // �������� �ش�
        base.OnDamaged(atk);

        // hp % ��ġ ���� �� UI�� ǥ��
        float hp = (float)nowHp / status.Hp;
        StatsUI.instance.SetEnemyHp(hp);

        // ��� Ȯ��
        ChkDead();
    }

    /// <summary>
    /// ������ Ÿ�̸Ӹ��� ���� �ݶ��̴� Ȱ��ȭ
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Attack()
    {
       
        // ���� ���̰� ���� �ø� �۵� ����
        while (!deadBool && GameManager.instance.state == GameManager.GAMESTATE.Play)
        {
            
            myWC.AtkColActive(true);

            yield return atkWaitTime;
        }
    }

    /// <summary>
    /// �ݶ��̴� �浹 �� ������ �޼ҵ�
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="other"></param>
    protected override void Attack(object sender, Collider other)
    {

        // ����� �÷��̾� ���̹Ƿ� �÷��̾����ؼ� �������� �ش�
        controller.OnDamaged(status.Atk);

        base.Attack(sender, other);
    }

    /// <summary>
    /// ��� �� �����ϴ� �޼ҵ�
    /// </summary>
    protected override void Dead()
    {

        // ���� ����
        base.Dead();

        // �ڷ�ƾ ���� GameObject�� disable�� ��ҵǱ� ������
        // Ȥ�� ���� ����
        StopAllCoroutines();

        // �¸� ���� �޼ҵ�
        GameManager.instance.ChkWin();

        // pool���� ������ release ���� ��, ��Ȱ��ȭ
        poolToReturn.Release(element: this);
    }
}
