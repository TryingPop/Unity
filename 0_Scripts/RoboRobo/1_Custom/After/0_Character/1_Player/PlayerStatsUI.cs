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
    /// ü�� ����
    /// </summary>
    /// <param name="nowHp">ü��</param>
    public void SetHp(int nowHp)
    {

        hpText.text = $"{nowHp}";
    }

    /// <summary>
    /// ���׹̳� ����
    /// </summary>
    /// <param name="stamina">���׹̳�</param>
    public void SetStamina(float stamina)
    {

        staminaText.text = $"{stamina * 10:F0}";
    }

    /// <summary>
    /// ���ݷ� ����
    /// </summary>
    /// <param name="atk">���ݷ�</param>
    public void SetAtk(int atk)
    {

        atkText.text = $"{atk}";
    }


}
