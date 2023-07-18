using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyStat : MonoBehaviour
{

    public int hp;
    public int currentHp;
    public int atk;
    public int def;
    public int exp;

    public GameObject healthBarBackGround;  // ������ ��尡 ����� ������ ĵ������ �ʿ��ϴ�
    public Image healthBarFilled;           // �̹��� ������Ʈ���� �̹��� Ÿ���� filled �� �ٲ������ϰ�
                                            // Fill Origin�� left�� ��������Ѵ�
                                            
    private void Start()
    {

        currentHp = hp;

        healthBarFilled.fillAmount = 1f;
    }

    public int Hit(int _playerAtk)
    {

        int dmg;

        if (def > _playerAtk)
        {

            dmg = 1;
        }
        else
        {

            dmg = _playerAtk - def;
        }

        if (currentHp <= 0)
        {

            PlayerStat.instance.currentExp += exp;
            Destroy(this.gameObject);
        }

        healthBarFilled.fillAmount = (float)currentHp / hp;
        healthBarBackGround.SetActive(true);
        StopAllCoroutines();                // �ڷ�ƾ ������ ���� �ڿ��� �Ҹ��ϱ⿡
                                            // �����Ӹ��� �����ϴ� ��쿡 �̷��� �ڵ带 ���� ������ �ɸ� �� �ִ�    
        StartCoroutine(WaitCoroutine());
        return dmg;
    }

    IEnumerator WaitCoroutine()
    {

        yield return new WaitForSeconds(3f);
        healthBarBackGround.SetActive(false);
    }
}
