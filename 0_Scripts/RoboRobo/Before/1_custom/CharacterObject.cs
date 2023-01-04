using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{ 
    [Header("������Ʈ �� ������Ʈ")]
    [Tooltip("ĳ���� �ִϸ�����")] [SerializeField]
    private Animator chrAnim;

    [Tooltip("ĳ���� �����")]
    public AudioSource chrAudio;

    [Tooltip("���� �Ҹ�")]
    public AudioClip atkSnd;

    [Tooltip("�´� �Ҹ�")]
    public AudioClip dmgSnd;

    [Tooltip("�÷��̾� ��ũ��Ʈ")] [SerializeField]
    private ThirdPersonController playerController;

    [Tooltip("���� ����Ʈ")] [SerializeField]
    private ParticleSystem atkParticle;

    [Tooltip("���� ����Ʈ")] [SerializeField]
    private ParticleSystem nuclearParticle;

    [Tooltip("Ȩ�� ����Ʈ")] [SerializeField]
    private ParticleSystem homerunParticle;

    // ���� ���� �ݶ��̴�
    private BoxCollider atkCollider;


    void Awake()
    {

        // ĳ���� �ݶ��̴� �������� 
        // �������� �� ���� �ݰ� ������Ʈ�� �� ���� ���� �ؾ��Ѵ�.
        atkCollider = GetComponent<BoxCollider>(); 
        chrAnim = GetComponent<Animator>();
                                                   
    }

    void AttackDone() // ���� ��� Ż�� 
    {

        // ���� ó�� ����
        if (chrAnim != null) // �ִϸ����Ͱ� �ִ� ���
        {
            chrAnim.SetBool("attackChk", false); // ���� ���� ���� 
        }
        else
        {
            Debug.Log("ĳ���Ϳ� �ִϸ����Ͱ� �����ϴ�.");
        }

        // ���� ó�� ����
        if (atkSnd != null && chrAudio != null)
        {
            chrAudio.PlayOneShot(atkSnd); // ���� �Ҹ� 1�� ���
        }
        else
        {
            Debug.Log("ĳ���Ϳ� ������� ���� �Ҹ��� �����ϴ�.");
        }

        atkCollider.enabled = false;
    }

    void DamageDone() // ������ �޴� ��� Ż�� 
    {

        // ���� ó�� ����
        if (chrAnim != null)
        {
            chrAnim.SetBool("damageChk", false); // ������ ���� ����
        }
        else
        {
            Debug.Log("ĳ���Ϳ� �ִϸ����Ͱ� �����ϴ�.");
        }

        // ���� ó�� ����
        if (dmgSnd != null && chrAudio != null) // �Ҹ��� ����� �Ѵ� �����ؾ�����
        {
            chrAudio.PlayOneShot(dmgSnd); // �´� �Ҹ� 1�� ���
        }
        else 
        { 
            Debug.Log("ĳ���� ������� �´� �Ҹ��� �����ϴ�."); 
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
        // ������ ���޿� ����
        int dmg;

        // NuclearAttacker�� 999!
        if (playerController.hidden == Stats.Hidden.NuclearAttacker)
        {
            dmg = playerController.nuclearAtk;
        }
        else
        {
            dmg = playerController.atk;
        }

        // other.SendMessage("Damaged", dmg); // enemy�� ������ �ִ°�
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

                // �˹� ���� 
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
