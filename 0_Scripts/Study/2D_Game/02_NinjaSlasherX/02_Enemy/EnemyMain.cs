using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ENEMYAISTS  // �� AI ����
{

    ACTIONSELECT,       // �׼� ���� (���)
    WAIT,               // ���� �ð� (���缭) ��ٸ���
    RUNTOPLAYER,        // �޷��� �÷��̾ ������ ����
    JUMPTOPLAYER,       // �����ؼ� �÷��̾ ������ ����
    ESCAPE,             // �÷��̾�Լ� ����ģ��
    ATTACKONSIGHT,      // �� �ڸ����� �̵����� �ʰ� �����Ѵ� (���Ÿ� ���ݿ�)
    FREEZ,              // �ൿ ���� (��, �̵�ó���� ����Ѵ�)
}

public class EnemyMain : MonoBehaviour
{

    // �ܺ� �Ķ����(Inspector ǥ��)
    public int debug_SelectRandomAIState = -1;

    // �ܺ� �Ķ����
    [HideInInspector] public ENEMYAISTS aiState = ENEMYAISTS.ACTIONSELECT;

    // ĳ��
    protected EnemyController enemyCtrl;
    protected GameObject player;
    protected PlayerController playerCtrl;

    // ���� �Ķ����
    protected float aiActionTimeLength = 0.0f;
    protected float aiActionTimeStart = 0.0f;
    protected float distanceToPlayer = 0.0f;
    protected float distanceToPlayerPrev = 0.0f;

    // �ڵ� (Monobehaviour �⺻ ��� ����)
    public virtual void Awake()
    {

        enemyCtrl = GetComponent<EnemyController>();
        player = PlayerController.GetGameObject();
        playerCtrl = player.GetComponent<PlayerController>();
    }

    // public virtual void Start() { }

    // public virtual void Update() { }

    public virtual void FixedUpdate()
    {

        if (BeginEnemyCommonWork())
        {

            FixedUpdateAI();
            EndEnemyCommonWork();
        }
    }

    public virtual void FixedUpdateAI() { }

    // �ڵ� (�⺻ AI ���� ó��)
    public bool BeginEnemyCommonWork()
    {

        // ����ִ��� Ȯ��
        if (enemyCtrl.hp <= 0)
        {

            return false;
        }

        enemyCtrl.animator.enabled = true;

        // ���� �˻�
        if (!CheckAction())
        {

            return false;
        }

        return true;
    }

    public void EndEnemyCommonWork()
    {

        // �׼� �Ѱ� �ð� �˻�
        float time = Time.fixedTime - aiActionTimeStart;
        if (time > aiActionTimeLength)
        {

            aiState = ENEMYAISTS.ACTIONSELECT;
        }
    }


    public bool CheckAction()
    {

        // ���� �˻�
        AnimatorStateInfo stateInfo = enemyCtrl.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.tagHash == EnemyController.ANITAG_ATTACK ||
            stateInfo.fullPathHash == EnemyController.ANISTS_DMG_A ||
            stateInfo.fullPathHash == EnemyController.ANISTS_DMG_B ||
            stateInfo.fullPathHash == EnemyController.ANISTS_Dead)
        {

            return false;
        }

        return true;
    }

    public int SelectRandomAIState()
    {

#if UNITY_EDITOR    // ��ó�� ��ɾ�
                    // UNITY_EDITOR������ ���� �ڵ尡 Ȱ��ȭ

        if (debug_SelectRandomAIState >= 0)
        {

            return debug_SelectRandomAIState;
        }
#endif

        return Random.Range(0, 100 + 1);
    }

    public void SetAIState(ENEMYAISTS sts, float t)
    {

        aiState = sts;
        aiActionTimeStart = Time.fixedTime;
        aiActionTimeLength = t;
    }

    public virtual void SetCombatAIState(ENEMYAISTS sts)
    {

        aiState = sts;
        aiActionTimeStart = Time.fixedTime;
        enemyCtrl.ActionMove(0.0f);
    }

    // �ڵ� (AI ��ũ��Ʈ ���� �Լ�)
    public float GetDistancePlayer()
    {

        distanceToPlayerPrev = distanceToPlayer;
        distanceToPlayer = Vector3.Distance(
            transform.position, playerCtrl.transform.position);
        return distanceToPlayer;
    }

    public bool IsChangeDistancePlayer(float l)
    {

        return (Mathf.Abs(distanceToPlayer - distanceToPlayerPrev) > l);
    }

    public float GetDistancePlayerX()
    {

        Vector3 posA = transform.position;
        Vector3 posB = playerCtrl.transform.position;
        posA.y = 0; posA.z = 0;
        posB.y = 0; posB.z = 0;
        return Vector3.Distance(posA, posB);
    }

    public float GetDistancePlayerY()
    {

        Vector3 posA = transform.position;
        Vector3 posB = playerCtrl.transform.position;
        posA.x = 0; posA.z = 0;
        posB.x = 0; posB.z = 0;
        return Vector3.Distance(posA, posB);
    }
}


