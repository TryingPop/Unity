using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int maxHealth;
    public int curHealth;

    private Rigidbody rigid;
    private BoxCollider boxCollider;

    private Material mat;

    // private static long testNum;

    private void Awake()
    {

        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Melee")
        {

            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;

            Vector3 reactVec = transform.position - other.transform.position;


            StartCoroutine(OnDamage(reactVec));
            // Debug.Log("Melee : " + curHealth);
        }
        else if (other.tag == "Bullet")
        {

            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            
            Vector3 reactVec = transform.position - other.transform.position;

            Destroy(other.gameObject);  // 총알 파괴
            StartCoroutine(OnDamage(reactVec));
            // Debug.Log("Ragne : " + curHealth);
        }
    }

    private IEnumerator OnDamage(Vector3 reactVec, bool isGrenade = false)
    {

        mat.color = Color.red;

        // Debug.Log($"코루틴 생성 횟수 {++testNum}");

        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {

            mat.color = Color.white;
        }
        else
        {

            // 여기에 넣을 때는 레이어 숫자 그대로 넣어주면 된다
            // LayerMask쪽은 다르게 !
            gameObject.layer = 25;
            mat.color = Color.gray;

            if (isGrenade)
            {

                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3f;

                rigid.freezeRotation = false;   // X, Z 축회전 다시 활성화
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
                rigid.AddTorque(reactVec * 15, ForceMode.Impulse);
            }
            else
            {

                reactVec = reactVec.normalized;
                reactVec += Vector3.up;

                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            }

            Destroy(gameObject, 4f);
        }
    }

    internal void HitByGrenade(Vector3 explosionPos)
    {

        curHealth -= 300;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec, true));
    }
}
