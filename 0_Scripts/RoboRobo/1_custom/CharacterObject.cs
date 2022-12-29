using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{ 
    [Header("컴포넌트 및 오브젝트")]
    [Tooltip("캐릭터 애니메이터")] [SerializeField]
    private Animator chrAnim;

    [Tooltip("캐릭터 오디오")]
    public AudioSource chrAudio;

    [Tooltip("공격 소리")]
    public AudioClip atkSnd;

    [Tooltip("맞는 소리")]
    public AudioClip dmgSnd;

    [Tooltip("플레이어 스크립트")] [SerializeField]
    private ThirdPersonController playerController;

    [Tooltip("공격 이펙트")] [SerializeField]
    private ParticleSystem atkParticle;

    [Tooltip("폭발 이펙트")] [SerializeField]
    private ParticleSystem nuclearParticle;

    [Tooltip("홈런 이펙트")] [SerializeField]
    private ParticleSystem homerunParticle;

    // 공격 범위 콜라이더
    private BoxCollider atkCollider;


    void Awake()
    {

        // 캐릭터 콜라이더 가져오기 
        // 여러개면 맨 공격 반경 컴포넌트가 맨 위에 오게 해야한다.
        atkCollider = GetComponent<BoxCollider>(); 
        chrAnim = GetComponent<Animator>();
                                                   
    }

    void AttackDone() // 공격 모션 탈출 
    {

        // 예외 처리 구문
        if (chrAnim != null) // 애니메이터가 있는 경우
        {
            chrAnim.SetBool("attackChk", false); // 공격 상태 변수 
        }
        else
        {
            Debug.Log("캐릭터에 애니메이터가 없습니다.");
        }

        // 예외 처리 구문
        if (atkSnd != null && chrAudio != null)
        {
            chrAudio.PlayOneShot(atkSnd); // 공격 소리 1번 재생
        }
        else
        {
            Debug.Log("캐릭터에 오디오나 공격 소리가 없습니다.");
        }

        atkCollider.enabled = false;
    }

    void DamageDone() // 데미지 받는 모션 탈출 
    {

        // 예외 처리 구문
        if (chrAnim != null)
        {
            chrAnim.SetBool("damageChk", false); // 데미지 상태 해제
        }
        else
        {
            Debug.Log("캐릭터에 애니메이터가 없습니다.");
        }

        // 예외 처리 구문
        if (dmgSnd != null && chrAudio != null) // 소리와 오디오 둘다 존재해야지만
        {
            chrAudio.PlayOneShot(dmgSnd); // 맞는 소리 1번 재생
        }
        else 
        { 
            Debug.Log("캐릭터 오디오나 맞는 소리가 없습니다."); 
        }
    }

    IEnumerator Attack(Collider other, float time)
    {

        GiveDamage(other);

        atkCollider.enabled = false;
        
        yield return new WaitForSeconds(time);
    }

    private void GiveDamage(Collider other)
    {
        // 데미지 전달용 변수
        int dmg;

        // NuclearAttacker면 999!
        if (playerController.hidden == Stats.Hidden.NuclearAttacker)
        {
            dmg = playerController.nuclearAtk;
        }
        else
        {
            dmg = playerController.atk;
        }

        // other.SendMessage("Damaged", dmg); // enemy면 데미지 주는거
        StatsUI.instance.stats = other.GetComponent<Stats>();

        StatsUI.instance.stats?.Damaged(dmg);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {

            if (playerController.hidden == Stats.Hidden.NuclearAttacker)
            {
                Instantiate(nuclearParticle, other.transform.position + other.transform.up, Quaternion.identity);
            }
            else if (playerController.hidden == Stats.Hidden.HomeRun)
            {
                Instantiate(homerunParticle, other.transform.position + other.transform.up, Quaternion.identity);
            }
            else
            {
                Instantiate(atkParticle, other.transform.position + other.transform.up, Quaternion.identity);
            }

            if (playerController.hidden != Stats.Hidden.ContinuousAttacker)
            {

                StartCoroutine(Attack(other, 0.25f));

                // 넉백 어택 
                if (playerController.hidden == Stats.Hidden.HomeRun)
                {

                    Vector3 forceDir = ((other.transform.position - transform.position).normalized + transform.up);
                    StatsUI.instance.stats?.rd.AddForce(forceDir * playerController.forcePow, ForceMode.Impulse);
                }
            }
            else
            {

                GiveDamage(other);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerController.hidden == Stats.Hidden.ContinuousAttacker)
        {
            GiveDamage(other);
        }
    }
    
    public void HammerActive()
    {
        
        playerController.hammerObj.SetActive(true);
    }
}
