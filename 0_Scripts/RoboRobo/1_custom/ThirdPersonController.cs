using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ThirdPersonController : Stats
{
    #region Component or Object
    
    [Header("������Ʈ �� ������Ʈ")]

    [Tooltip("ĳ���� ��ü ������Ʈ")] [SerializeField]
    private Transform chrBody; 

    [Tooltip("ī�޶� ����")] [SerializeField]
    private Transform cameraBox;
    
    [Tooltip("ĳ���� �ִϸ�����")]
    public Animator chrAnimator;

    [Tooltip("���� ���� �ݶ��̴�")] [SerializeField]
    private BoxCollider atkCollider;

    [Tooltip("����")] [SerializeField]
    private SkinnedMeshRenderer chrMesh;

    [Tooltip("Hammer")]
    public GameObject hammerObj;
    #endregion Component or Object


    #region Convertible Variable

    [Header("����")]
    [Tooltip("ĳ���� �̵� �ӵ�")] [SerializeField]
    private float moveSpeed;

    [Tooltip("ĳ���� �޸��� �ӵ�")] [SerializeField]
    private float runSpeed;

    [Tooltip("ĳ���� ���� �Ŀ�")] [SerializeField]
    private float jumpForce; 

    [Tooltip("ī�޶� �ΰ���")] [SerializeField]
    private float lookSensitivity;

    [Tooltip("���׹̳�(���� ��)")] [SerializeField]
    private float maxStamina;
    #endregion Convertible Variable

    // �ٴ� ����
    private bool runBool;

    // ���鿡 ��Ҵ��� Ȯ���ϴ� ����
    private bool groundBool;

    // ������ ��?
    private bool activeBool;

    // ��ư ���� Ȯ��
    private bool pushBool;

    // ���׹̳� ��� ������ ���� Ȯ��
    private bool staminaBool;


    // ���� �ӵ�
    private float applySpeed;

    // ���� ���׹̳�
    private float nowStamina;

    // ��ü
    private Rigidbody playerRigidbody;

    // �÷��̾� ĸ�� �ݶ��̴�
    private CapsuleCollider playerCollider;

    public float forcePow = 10f;

    private void Start()
    {

        // ������Ʈ ��������
        playerRigidbody = GetComponent<Rigidbody>(); 
        playerCollider = GetComponent<CapsuleCollider>(); 

        // �ǰ� �� ��ȯ ��ų ���׸���
        if (chrMesh == null)
        {
            chrMesh = GetComponentInChildren<SkinnedMeshRenderer>(); 
        }

        // hp ����
        SetHp();

        // �ʱ� ���׹̳� ����
        nowStamina = maxStamina;

        // ui �ʱ�ȭ
        StatsUI.instance.SetHp(nowHp);
        StatsUI.instance.SetStamina(nowStamina);
        StatsUI.instance.SetAtk(atk);

        // �ʱ� �ִϸ��̼� �ӵ� ����
        chrAnimator.speed = 2.0f;

        // �ʱ� �̵��ӵ� ����
        applySpeed = moveSpeed;

        // ���� �ʱ�ȭ
        hidden = Hidden.None;
        
    }

    void Update()
    {
        // ���� ���� ���¿����� ���� ����
        if (!deadBool 
            // && !GameManager.instance.uiBool
            ) 
        {
            // Ű�Է� ���� üũ
            IsGround(); // ���� üũ
            RunChk(); // �޸��� üũ

            // �̵� ����
            Move(); // �̵�
            Jump(); // ����

            // �þ� ����
            LookAround(); // �þ� Ȯ��

            // ����
            Attack(); // ����

            // �ൿ �� ���� üũ 
            StaminaChk(); // ���׹̳� ���� üũ

            if (Input.GetKeyDown(KeyCode.G))
            {
                StartSquat();
            }
        }
    }

    void StartSquat()
    {
        chrAnimator.SetTrigger("GG");
    }


    void IsGround() // ���� üũ 
    {
        groundBool = Physics.Raycast(transform.position, Vector3.down, playerCollider.bounds.extents.y + 0.1f);
        // ����� �����ɽ�Ʈ�� ���� �÷��̾� ���� �ؿ� �ݶ��̴��� ũ�� ���� + 0.1 ��ŭ �Ÿ� üũ
    }

    void RunChk() // �޸��� üũ
    {
        if (Input.GetKey(KeyCode.LeftShift) && nowStamina > 0) // ���� ����Ʈ�� �޸��� �ӵ� ����
        {
            runBool = true; // ���׹̳� ���� �� �� �뵵

            if (hidden != Hidden.HealthMan)
            {
                applySpeed = runSpeed; // �޸��� �ӵ��� ����
            }
            else
            {
                applySpeed = runSpeed * 2f; // �޸��� �ӵ� 2��
            }
            staminaBool = true; // ���׹̳� ��� ����
        }
        else
        {
            runBool = false; // �޸��� ���� X
            applySpeed = moveSpeed; // �ȴ� �ӵ� ����
            staminaBool = false; // ���׹̳� ������ X
        }
    }

    void Move() // �̵�
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // �����¿� Ű �Է°� ����
        pushBool = (moveInput != Vector2.zero); // Ű�Է� ������ �ȴ����� Ȯ���� �� ���� �뵵

        if (pushBool) // �̵�
        {
            // ���� ���� ���� ���
            #region animator // �ִϸ����� ��ȯ�� �� ���� ����
            if (!activeBool) // �ִϸ��̼� �� ���� ����
            {
                activeBool = true;
                chrAnimator.SetBool("runChk", true); // �ȴ� ���
            }

            if (runBool)
            {
                chrAnimator.speed = 3.0f; // �ִϸ��̼� �ӵ� ������ 
            }
            else
            {
                chrAnimator.speed = 2.0f; // �ִϸ��̼� �ӵ� ������
            }

            if (hidden == Hidden.TimeConqueror)
            {
                chrAnimator.speed *= GameManager.instance.accTime;
            }
            
            #endregion animator


            Vector3 lookForward = new Vector3(cameraBox.forward.x, 0f, cameraBox.forward.z).normalized; // ����
            Vector3 lookRight = new Vector3(cameraBox.right.x, 0f, cameraBox.right.z).normalized; // ����
            Vector3 moveDir = (lookForward * moveInput.y + lookRight * moveInput.x).normalized; // �̵� ���� 

            // chrBody.forward = lookForward; // ��ó�� ������ �ȴ°�
            chrBody.forward = moveDir; // ĳ���Ͱ� �̵��ϴ� �������� �ٶ󺸱�
            // transform.position += moveDir * Time.deltaTime * moveSpeed;

            if (hidden != Hidden.TimeConqueror)
            {
                playerRigidbody.MovePosition(transform.position + moveDir * applySpeed * Time.deltaTime); // �̵�
            }
            else
            {
                playerRigidbody.MovePosition(transform.position + moveDir * applySpeed * 2 * GameManager.instance.accTime * Time.deltaTime); // �̵�
            }

        }
        else 
        {
            if (activeBool) // Ȱ�� ���ε� Ű �Է��� ������ ��Ȱ��ȭ ������� ����
            {
                activeBool = false; // �ߺ����� �����°� ����
                chrAnimator.SetBool("runChk", false); // idle ��� Ȱ��ȭ
            }
        }
    }

    void Jump() // ����
    {
        if (groundBool && Input.GetKeyDown(KeyCode.Space)) // ���鿡 �ݰ� �����̽��ٸ� ���� ���
        {

            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // ���� ����
        }
    }

    void LookAround() // ī�޶� �̵�
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * lookSensitivity; // ���� x, y �� ȸ�� ��
        Vector3 camAngle = cameraBox.rotation.eulerAngles; // ���� ī�޶� ȸ�� �ޱ�
        if (hidden != Hidden.TimeConqueror)
        {
            mouseDelta *= Time.deltaTime * 100f;
        }
        else
        {
            mouseDelta *= Time.unscaledDeltaTime * 100f;
        }
        float x = camAngle.x - mouseDelta.x; // �� ������ �α����� ������ �޾ƿ�

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 90f); // �� ���� 90�� ����
            // -1f �ؾ��� ������ �̵� ����
        }
        else
        {
            x = Mathf.Clamp(x, 330f, 361f); // �Ʒ����� 90�� ����
            // 361f �ؾ��� �������� �Ѿ�� ����
        }
        
        cameraBox.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.y, camAngle.z);// ī�޶� ȸ��
    }

    void Attack() // ����
    {
        if (Input.GetMouseButtonDown(0) && !chrAnimator.GetCurrentAnimatorStateInfo(0).IsName("1_attack")) // ���� �ִϸ��̼� ��Ȱ��ȭ & ���� ��ư ���� ���
        {

            chrAnimator.SetBool("attackChk", true); // ���� ���
            dmgCol.enabled = true;
        }
    }

    void StaminaChk() // ���׹̳� üũ
    {
        if (staminaBool && pushBool) // ���׹̳� ��� ������ ���°� ��ư ������ ��,
        {
            if (hidden != Hidden.HealthMan) // HealthMan �ɷ� ���� ������ ���� ���׹̳�!
            {
                nowStamina -= Time.deltaTime; // ���׹̳� ����
                
                if (nowStamina < 0) // ���׹̳��� ������ 0����
                {
                    nowStamina = 0; // ���׹̳� 0
                }

                StatsUI.instance.SetStamina(nowStamina);
            }
            
        }

        else if (!pushBool) // �̵� Ű �Է��� ���� ��
        {
            if (nowStamina < maxStamina) // ���׹̳��� Ǯ�� �ƴ� ���
            {
                nowStamina += Time.deltaTime; // ���׹̳� ���

                if (nowStamina > maxStamina) // �ִ밪 �Ѿ�� �ִ밪���� ����
                {
                    nowStamina = maxStamina;
                }

                StatsUI.instance.SetStamina(nowStamina);
            }
        }

    }

    /// <summary>
    /// �ǰ� �޼ҵ�
    /// </summary>
    /// <param name="_damage">������</param>
    public override void Damaged(int _damage)
    {
        chrAnimator.SetBool("damageChk", true); // ������ ���� ǥ��
        base.Damaged(_damage); // ������ �ִ� �Լ� �ּҰ� 1 ����
                               // �� ��� Ȯ��
        StatsUI.instance.SetHp(nowHp);
    }

    public void ChangeColor(Color color)
    {
        chrMesh.material.color = color;
    }

    protected override void Dead()
    {
        base.Dead();

        GameManager.instance.GameOver(false);
    }

    /// <summary>
    /// Ư�� X
    /// </summary>
    public void SetNone()
    {
        hidden = Hidden.None;
        StatsUI.instance.SetAtk(atk);
    }

    /// <summary>
    /// ���� �ʴ� Ư��
    /// </summary>
    public void SetImmortality()
    {
        hidden = Hidden.Immortality;
        StatsUI.instance.SetAtk(atk);
    }

    /// <summary>
    /// ���׹̳� ����
    /// </summary>
    public void SetHealthMan()
    {
        hidden = Hidden.HealthMan;
        StatsUI.instance.SetAtk(atk);
    }

    /// <summary>
    /// ��û ������ Ư��
    /// </summary>
    public void SetTimeConqueror()
    {
        hidden = Hidden.TimeConqueror;
        StatsUI.instance.SetAtk(atk);
    }

    /// <summary>
    /// ���� ����Ŀ
    /// </summary>
    public void SetNuclearAttacker()
    {
        hidden = Hidden.NuclearAttacker;
        StatsUI.instance.SetAtk(nuclearAtk);
    }

    /// <summary>
    /// ���� ����Ŀ
    /// </summary>
    public void SetContinuousAttacker()
    {
        hidden = Hidden.ContinuousAttacker;
        StatsUI.instance.SetAtk(atk);
    }

    /// <summary>
    /// �˹�
    /// </summary>
    public void SetHomeRun()
    {
        hidden = Hidden.HomeRun;
        StatsUI.instance.SetAtk(atk);
    }
}