using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{

    [SerializeField] private Text hpText;
    [SerializeField] private Text staminaText;
    [SerializeField] private Text atkText;

    /// <summary>
    /// 체력 설정
    /// </summary>
    /// <param name="nowHp">체력</param>
    public void SetHp(int nowHp)
    {

        hpText.text = $"{nowHp}";
    }

    /// <summary>
    /// 스테미나 설정
    /// </summary>
    /// <param name="stamina">스테미나</param>
    public void SetStamina(float stamina)
    {

        staminaText.text = $"{stamina * 10:F0}";
    }

    /// <summary>
    /// 공격력 설정
    /// </summary>
    /// <param name="atk">공격력</param>
    public void SetAtk(int atk)
    {

        atkText.text = $"{atk}";
    }


}
