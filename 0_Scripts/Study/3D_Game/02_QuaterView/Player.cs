using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 20f;
    public float jumpPower = 15;

    private float hAxis;    // ĳ���� �̵� �¿� ���� Ű
    private float vAxis;    // ĳ���� �̵� ���� ���� Ű

    private bool wDown;     // ĳ���� �ȱ� ���� ����Ʈ Ű �Է�
    private bool jDown;     // ĳ���� ���� �����̽��� Ű

    private bool iDown;     // ��� ���� �Է� Ű
    private bool sDown1;
    private bool sDown2;
    private bool sDown3;

    private bool fDown;     // ���� ��ư ���콺 ����

    private bool rDown;     // ���� ��ư r��ư

    private bool gDown;     // ��ź ��� ��ư ���⼭�� ���콺 ������

    private bool isJump;    // ���� ���� ���� üũ
    private bool isDodge;   // ���� ȸ�� ���� üũ

    private bool isSwap;    // ���� ���� ��?

    private bool isFireReady = true;   // ���� ���� ����?

    private bool isReload;      // ���� ��?

    private bool isBorder;      // �����ڸ��� ������?

    private bool isDamage;      // ���� Ÿ���� ���� ����

    private Vector3 moveVec;    // �̵��� ����3
    private Vector3 dodgeVec;   // ������ ����3
    private Animator anim;
    public Rigidbody rigid;

    private MeshRenderer[] meshs;  

    private GameObject nearObject;
    private Weapon equipWeapon;
    int equipWeaponIndex = -1;

    public GameObject[] weapons;
    public bool[] hasWeapons;

    public GameObject[] grenades;   // ��ź ����
    public int hasGrenades;

    public GameObject grenadeObj;

    public int ammo;
    public int coin;
    public int health;
    

    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenades;

    private float fireDelay;    // ���� ������

    public Camera followCamera;

    private void Awake()
    {

        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();

        meshs = GetComponentsInChildren<MeshRenderer>();
    }

    private void Update()
    {

        GetInput();
        Move();
        Turn();
        Jump();

        Grenade();

        Attack();
        Reload();
        Dodge();

        Swap();
        Interaction();
    }

    private void GetInput()
    {

        // GetAxis�� �ε巴�� �޾ƿ��� �ݸ�,   -1f ~ 1f
        // GetAxisRaw�� ��� �޾ƿ´�          -1, 0, 1�� �޾ƿ´�
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        // ProjectSettings�� InputManager���� Walk�� �߰�������Ѵ�
        wDown = Input.GetButton("Walk");        // ���⼭�� left shift
        jDown = Input.GetButtonDown("Jump");    // ���⼭�� �����̽� ��

        iDown = Input.GetButtonDown("Interaction"); // ���⼭�� e

        fDown = Input.GetButton("Fire1");

        rDown = Input.GetButtonDown("Reload");

        gDown = Input.GetButtonDown("Fire2");

        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    private void Move()
    {

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;      // ũ�� 1�� �����

        if (isDodge)
        {

            moveVec = dodgeVec;
        }

        if (isSwap || isReload || !isFireReady)
        {

            moveVec = Vector3.zero;
        }

        if (!isBorder)
        {

            transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;
        }

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    private void Turn()
    {

        // Ű���忡 ���� ȸ��
        // �ش� ��ǥ�� �ٶ󺻴�
        // �׷��� ���� ��ġ + moveVec�� ���ش�
        // �̵� ���̳� �̵� �ĳ� �ڵ�� �Ȱ���
        // ���� �Ʒ��� ���̺����� �Ҵ��� ���
        // Vector3 destination = transform.position + moveVec
        // LookAt�� �̵� ������ �����Ѵ�
        transform.LookAt(transform.position + moveVec);

        // ���콺�� ���� ȸ��

        if (fDown)
        {

            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);  // ��ũ������ ���̸� ��� �޼ҵ� �Ű������� �ش� ��ġ
            RaycastHit rayHit;  // �浹ü�� ��� ����

            // �Ÿ��� 100
            // LayerMask.GetMask(���̾� �̸�);��� ����� �ڵ�
            // ���⼭ 11�� Floor ���̾��̹Ƿ� 11�� ����
            // 1 << ?? ���� ?? �� �� ���ڴ� ���̾� ��ȣ 1�� ��Ʈ �������� 11ĭ �̵� ���Ѷ�� �ǹ��̴�
            // Ȥ�� 2^11 = 2048�� ���� �ص��ȴ�
            if (Physics.Raycast(ray, out rayHit, 100, 1 << 11))
            {


                // ���̰� ��Ҵ� ����
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
    }

    private void Jump()
    {

        if (jDown && !isJump && moveVec == Vector3.zero && !isDodge && !isSwap)
        {

            // �������� ���� �ִ� ����� Impulse
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    private void Grenade()
    {

        if (hasGrenades == 0)
        {

            return;
        }

        if (gDown && !isReload && !isSwap)
        {

            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100))
            {

                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 4f;

                GameObject instantGrenade = Instantiate(grenadeObj, transform.position + 2 * Vector3.up, transform.rotation);
                Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();

                rigidGrenade.AddForce(nextVec, ForceMode.Impulse);
                rigidGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);

                hasGrenades--;
                if (hasGrenades < 4)
                {

                    grenades[hasGrenades].SetActive(false);
                }
            }
        }
    }

    private void Attack()
    {

        if (equipWeapon == null) return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if (fDown && isFireReady && !isDodge && !isSwap)
        {

            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0f;
        }
    }

    private void Reload()
    {

        if (equipWeapon == null)
        {

            return;
        }

        if (equipWeapon.type != Weapon.Type.Ranged || ammo == 0)
        {

            return;
        }

        if (rDown && !isJump && !isDodge && !isSwap && isFireReady)
        {

            anim.SetTrigger("doReload");
            isReload = true;

            Invoke("ReloadOut", 0.4f);
        }
    }

    private void ReloadOut()
    {

        int reAmmo = ammo < equipWeapon.maxAmmo - equipWeapon.curAmmo ? ammo : equipWeapon.maxAmmo - equipWeapon.curAmmo;
        equipWeapon.curAmmo += reAmmo;
        ammo -= reAmmo;
        isReload = false;
    }

    private void Dodge()
    {

        if (jDown && !isJump && moveVec != Vector3.zero && !isDodge && !isSwap)
        {

            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            // �ش� �̸��� �޼ҵ带 0.4�� �ڿ� ����
            Invoke("DodgeOut", 0.4f);
        }
    }

    private void DodgeOut()
    {

        isDodge = false;
        speed *= 0.5f;
    }

    private void Swap()
    {

        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
        {

            return;
        }

        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
        {

            return;
        }

        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
        {

            return;
        }

        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        else if (sDown2) weaponIndex = 1;
        else if (sDown3) weaponIndex = 2;

        if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge)
        {

            if (equipWeapon != null)
            {

                equipWeapon.gameObject.SetActive(false);
            }

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[equipWeaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            anim.SetTrigger("doSwap");

            isSwap = true;

            Invoke("SwapOut", 0.3f);
        }
    }

    private void SwapOut()
    {

        isSwap = false;
    }

    private void Interaction()
    {

        if (iDown && nearObject != null && !isJump && !isDodge)
        {

            if (nearObject.tag == "Weapon")
            {

                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;   // value�� �ε��� ����

                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        } 
    }

    private void FixedUpdate()
    {

        FreezeRotation();
        StopToWall();
    }

    private void FreezeRotation()
    {

        rigid.angularVelocity = Vector3.zero;
    }

    private void StopToWall()
    {

        Debug.DrawRay(transform.position + Vector3.up * 2f, transform.forward * 5, Color.magenta);
        
        // ���̸� ���� �� ���̾�� �浹 �Ǹ� true, ������ false
        isBorder = Physics.Raycast(transform.position + Vector3.up * 2f, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    // ���� ����
    private void OnCollisionEnter(Collision collision)
    {
        
        // �±׷� ���� ����
        // Floor �±� ���� �� ���鿡 Floor �±� �߰�
        if (collision.gameObject.tag == "Floor")
        {

            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        // ������
        if (other.tag == "Item")
        {

            Item item = other.GetComponent<Item>();

            // Use��?
            switch (item.type)
            {

                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo) ammo = maxAmmo;
                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth) health = maxHealth;
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin) coin = maxCoin;
                    break;
                case Item.Type.Grenade:

                    hasGrenades += item.value;
                    if (hasGrenades > maxHasGrenades) hasGrenades = maxHasGrenades;
                    for (int i = 0; i < Mathf.Min(hasGrenades, 4); i++)
                    {

                        grenades[i].SetActive(true);
                    }
                    break;
            }

            Destroy(other.gameObject);
        }
        // ��
        else if (other.tag == "EnemyBullet")
        {

            if (!isDamage)
            {

                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;

                if (other.GetComponent<Rigidbody>() != null) Destroy(other.gameObject);
                StartCoroutine(OnDamage());
            }
        }
    }

    IEnumerator OnDamage()
    {

        isDamage = true;

        foreach(MeshRenderer mesh in meshs)
        {

            mesh.material.color = Color.yellow;
        }

        yield return new WaitForSeconds(1f);

        foreach(MeshRenderer mesh in meshs)
        {

            mesh.material.color = Color.white;
        }

        isDamage = false;
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other.tag == "Weapon")
        {

            nearObject = other.gameObject;
        }

        
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.tag == "Weapon")
        {

            nearObject = null;
        }
    }
}
