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

    public GameObject healthBarBackGround;  // 랜더러 모드가 월드로 설정된 캔버스가 필요하다
    public Image healthBarFilled;           // 이미지 컴포넌트에서 이미지 타입을 filled 로 바꿔저야하고
                                            // Fill Origin을 left로 맞춰줘야한다
                                            
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
        StopAllCoroutines();                // 코루틴 생성에 많은 자원을 소모하기에
                                            // 프레임마다 공격하는 경우에 이러한 코드를 쓰면 과부하 걸릴 수 있다    
        StartCoroutine(WaitCoroutine());
        return dmg;
    }

    IEnumerator WaitCoroutine()
    {

        yield return new WaitForSeconds(3f);
        healthBarBackGround.SetActive(false);
    }
}
