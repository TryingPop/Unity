using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    public static StatsUI instance;

    [SerializeField] private Text hpText;
    [SerializeField] private Text staminaText;
    [SerializeField] private Text atkText;

    [SerializeField] private GameObject EnemyHpGameObj;
    [SerializeField] private Transform EnemyHpBar;
    [SerializeField] private Text EnemyHpTxt;
    [SerializeField] private float activeHpTime;

    private WaitForSeconds hpTime;
    private int seeCnt;
    
    private void Awake()
    {

        if (instance == null)
        {

            instance = this;
        }
        else
        {

            Destroy(this);
        }

        hpTime = new WaitForSeconds(activeHpTime);
    }

    public void SetHp(int nowHp)
    {

        hpText.text = $"{nowHp}";
    }

    public void SetStamina(float stamina)
    {

        staminaText.text = $"{stamina * 10:F0}";
    }

    public void SetAtk(int atk)
    {

        atkText.text = $"{atk}";
    }

    public void SetEnemyHp(Stats targetStatus)
    {

        StartCoroutine(ActiveEnemyHp());
        EnemyHpBar.localScale = Vector3.forward + Vector3.up + Vector3.right * targetStatus.GetHpBar();
        EnemyHpTxt.text = targetStatus.GetHpBar() <= 0 ? "0%" : $"{100 * targetStatus.GetHpBar():F1}%";
    }

    private IEnumerator ActiveEnemyHp()
    {
        EnemyHpGameObj.SetActive(true);
        seeCnt++;

        yield return hpTime;
        
        seeCnt--;

        if (seeCnt == 0)
        {
            EnemyHpGameObj.SetActive(false);
        }
    }
}
