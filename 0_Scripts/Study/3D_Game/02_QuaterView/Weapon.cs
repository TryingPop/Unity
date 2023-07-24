using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public enum Type
    {

        Melee,
        Ranged,
    }

    public Type type;

    public int damage;
    public float rate;

    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    public Transform bulletPos;
    public GameObject bullet;

    public Transform bulletCasePos;
    public GameObject bulletCase;

    public int maxAmmo;
    public int curAmmo;

    // 플레이어가 사용
    public void Use()
    {

        if (type == Type.Melee)
        {

            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if (type == Type.Ranged && curAmmo > 0)
        {

            curAmmo--;
            StartCoroutine("Shot");
        }
    }

    private IEnumerator Shot()
    {

        // 총알 발사
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();

        // AddForce로 해도된다
        bulletRigid.velocity = bulletPos.forward * 50;
        yield return null;

        // 탄피 배출
        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
        // 총알 나가는 방향을 봐야한다
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3f, -1f) + Vector3.up * Random.Range(2f, 4f);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        // 회전
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }

    private IEnumerator Swing()
    {

        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;

    }
}
