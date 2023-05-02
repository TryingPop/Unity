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
    RETURNTODOGPILE,    // ���� ���Ϸ� ���ƿ´�
    ATTACKONSIGHT,      // �� �ڸ����� �̵����� �ʰ� �����Ѵ� (���Ÿ� ���ݿ�)
    FREEZ,              // �ൿ ���� (��, �̵�ó���� ����Ѵ�)
}

public class EnemyMain : MonoBehaviour
{

    // �ܺ� �Ķ����(Inspector ǥ��)
    public bool cameraSwitch = true;
    public bool inActiveZoneSwitch = false;
    public bool combatAIOrder = true;
    public float dogPileReturnLength = 10.0f;

    public int debug_SelectRandomAIState = -1;

    // �ܺ� �Ķ����
    [HideInInspector] public bool cameraEnabled = false;
    [HideInInspector] public bool inActiveZone = false;
    [HideInInspector] public ENEMYAISTS aiState = ENEMYAISTS.ACTIONSELECT;
    [HideInInspector] public GameObject dogPile;

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

    public virtual void Start() 
    {

        // Dog Pile Set
        StageObject_DogPile[] dogPileList =
            GameObject.FindObjectsOfType<StageObject_DogPile>();

        foreach(StageObject_DogPile findDogPile in dogPileList)
        {

            foreach(GameObject go in findDogPile.enemyList)
            {

                if (gameObject == go)
                {

                    dogPile = findDogPile.gameObject;
                    break;
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        
        // ���� �˻�
        if (enemyCtrl.grounded && CheckAction())
        {

            //Debug.Log("Enemy OnTriggerStay2D: " + collision.name);
            if (collision.name == "EnemyJumpTrigger_L")
            {

                if (enemyCtrl.ActionJump())
                {

                    enemyCtrl.ActionMove(-1.0f);
                }
            }
            else if (collision.name == "EnemyJumpTrigger_R")
            {

                if (enemyCtrl.ActionJump())
                {

                    enemyCtrl.ActionMove(+1.0f);
                }
            }
            else if (collision.name == "EnemyJumpTrigger")
            {

                enemyCtrl.ActionJump();
            }
        }
    }

    public virtual void Update() 
    {

        cameraEnabled = false;
    }

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

        // Ȱ�� ������ ���� �ִ��� Ȯ��
        if (inActiveZoneSwitch)
        {

            inActiveZone = false;
            Vector3 vecA = player.transform.position +
                playerCtrl.enemyActiveZonePointA;
            Vector3 vecB = player.transform.position +
                playerCtrl.enemyActiveZonePointB;

            if (transform.position.x > vecA.x && transform.position.x < vecB.x &&
                transform.position.y > vecA.y && transform.position.y < vecB.y)
            {

                inActiveZone = true;
            }
        }

        // ���߿����� ���� ����(���� ��ġ ��, ������ ����)
        if (enemyCtrl.grounded)
        {

            // ī�޶� �ȿ� ���� �ִ��� Ȯ��
            if(cameraSwitch && !cameraEnabled && !inActiveZone)
            {

                // ī�޶� �������� ���� �ʴ�
                enemyCtrl.ActionMove(0.0f);
                enemyCtrl.cameraRendered = false;
                enemyCtrl.animator.enabled = false;
                enemyCtrl.rigidbody2D.Sleep();
                return false;
            }
        }

        enemyCtrl.animator.enabled = true;
        enemyCtrl.cameraRendered = true;

        // ���� �˻�
        if (!CheckAction())
        {

            return false;
        }

        // ���� ����
        if (dogPile != null)
        {

            if (GetDistanceDogPile() > dogPileReturnLength)
            {

                aiState = ENEMYAISTS.RETURNTODOGPILE;
            }
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

    public float GetDistanceDogPile()
    {

        return Vector3.Distance(transform.position, dogPile.transform.position);
    }
}