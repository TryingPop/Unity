using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{

    public int hp;
    public int currentHp;
    public int atk;
    public int def;
    public int exp;

    private void Start()
    {

        currentHp = hp;
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

        return dmg;
    }
}
