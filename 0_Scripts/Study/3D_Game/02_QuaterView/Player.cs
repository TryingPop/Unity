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

    private bool fDown;     // ���� ��ư 

    private bool isJump;    // ���� ���� ���� üũ
    private bool isDodge;   // ���� ȸ�� ���� üũ

    private bool isSwap;    // ���� ���� ��?

    private bool isFireReady = true;   // ���� ���� ����?

    private Vector3 moveVec;    // �̵��� ����3
    private Vector3 dodgeVec;   // ������ ����3
    private Animator anim;
    public Rigidbody rigid;

    private GameObject nearObject;
    private Weapon equipWeapon;
    int equipWeaponIndex = -1;

    public GameObject[] weapons;
    public bool[] hasWeapons;

    public GameObject[] grenades;   // ��ź ����
    public int hasGrenades;

    public int ammo;
    public int coin;
    public int health;
    

    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenades;

    private float fireDelay;    // ���� ������

    private void Awake()
    {

        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        GetInput();
        Move();
        Turn();
        Jump();
        Attack();
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

        if (isSwap || !isFireReady)
        {

            moveVec = Vector3.zero;
        }
        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    private void Turn()
    {

        // �ش� ��ǥ�� �ٶ󺻴�
        // �׷��� ���� ��ġ + moveVec�� ���ش�
        // �̵� ���̳� �̵� �ĳ� �ڵ�� �Ȱ���
        // ���� �Ʒ��� ���̺����� �Ҵ��� ���
        // Vector3 destination = transform.position + moveVec
        // LookAt�� �̵� ������ �����Ѵ�
        transform.LookAt(transform.position + moveVec);
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

    private void Attack()
    {

        if (equipWeapon == null) return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if (fDown && isFireReady && !isDodge && !isSwap)
        {

            equipWeapon.Use();
            anim.SetTrigger("doSwing");
            fireDelay = 0f;
        }
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
