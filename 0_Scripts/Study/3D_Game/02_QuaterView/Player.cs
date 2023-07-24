using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 20f;
    public float jumpPower = 15;

    private float hAxis;    // 캐릭터 이동 좌우 방향 키
    private float vAxis;    // 캐릭터 이동 상하 방향 키

    private bool wDown;     // 캐릭터 걷기 좌측 쉬프트 키 입력
    private bool jDown;     // 캐릭터 점프 스페이스바 키

    private bool iDown;     // 장비 장착 입력 키
    private bool sDown1;
    private bool sDown2;
    private bool sDown3;

    private bool fDown;     // 공격 버튼 마우스 왼쪽

    private bool rDown;     // 장전 버튼 r버튼

    private bool isJump;    // 현재 점프 상태 체크
    private bool isDodge;   // 현재 회피 상태 체크

    private bool isSwap;    // 무기 변경 중?

    private bool isFireReady = true;   // 공격 가능 상태?

    private bool isReload;      // 장전 중?

    private bool isBorder;

    private Vector3 moveVec;    // 이동용 벡터3
    private Vector3 dodgeVec;   // 닷지용 벡터3
    private Animator anim;
    public Rigidbody rigid;

    private GameObject nearObject;
    private Weapon equipWeapon;
    int equipWeaponIndex = -1;

    public GameObject[] weapons;
    public bool[] hasWeapons;

    public GameObject[] grenades;   // 폭탄 개수
    public int hasGrenades;

    public int ammo;
    public int coin;
    public int health;
    

    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenades;

    private float fireDelay;    // 공격 딜레이

    public Camera followCamera;

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
        Reload();
        Dodge();

        Swap();
        Interaction();
    }

    private void GetInput()
    {

        // GetAxis는 부드럽게 받아오는 반면,   -1f ~ 1f
        // GetAxisRaw는 즉시 받아온다          -1, 0, 1로 받아온다
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        // ProjectSettings의 InputManager에서 Walk를 추가해줘야한다
        wDown = Input.GetButton("Walk");        // 여기서는 left shift
        jDown = Input.GetButtonDown("Jump");    // 여기서는 스페이스 바

        iDown = Input.GetButtonDown("Interaction"); // 여기서는 e

        fDown = Input.GetButton("Fire1");

        rDown = Input.GetButtonDown("Reload");

        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    private void Move()
    {

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;      // 크기 1로 맞춘다

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

        // 키보드에 의한 회전
        // 해당 좌표로 바라본다
        // 그래서 현재 위치 + moveVec을 해준다
        // 이동 전이나 이동 후나 코드는 똑같다
        // 만약 아래와 같이변수를 할당할 경우
        // Vector3 destination = transform.position + moveVec
        // LookAt은 이동 전으로 가야한다
        transform.LookAt(transform.position + moveVec);

        // 마우스에 의한 회전

        if (fDown)
        {

            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);  // 스크린에서 레이를 쏘는 메소드 매개변수는 해당 위치
            RaycastHit rayHit;  // 충돌체를 담는 변수

            // 거리는 100
            // LayerMask.GetMask(레이어 이름);대신 사용한 코드
            // 여기서 11이 Floor 레이어이므로 11로 설정
            // 1 << ?? 에서 ?? 에 들어갈 숫자는 레이어 번호 1을 비트 왼쪽으로 11칸 이동 시켜라는 의미이다
            // 혹은 2^11 = 2048의 값을 해도된다
            if (Physics.Raycast(ray, out rayHit, 100, 1 << 11))
            {


                // 레이가 닿았던 지점
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

            // 순간적인 힘을 주는 모드인 Impulse
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

            // 해당 이름의 메소드를 0.4초 뒤에 실행
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
                int weaponIndex = item.value;   // value로 인덱스 구분

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
        
        // 레이를 쏴서 벽 레이어와 충돌 되면 true, 없으면 false
        isBorder = Physics.Raycast(transform.position + Vector3.up * 2f, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    // 착지 판정
    private void OnCollisionEnter(Collision collision)
    {
        
        // 태그로 지면 판정
        // Floor 태그 생성 및 지면에 Floor 태그 추가
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

            // Use로?
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
