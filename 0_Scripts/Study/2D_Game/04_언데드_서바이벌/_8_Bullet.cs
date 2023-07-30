using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _8_Bullet : MonoBehaviour
{

    public float damage;
    public int per;         // °üÅë ¼ö

    public void Init(float damage, int per)
    {

        this.damage = damage;
        this.per = per;
    }
}