using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTEnemy : MonoBehaviour
{

    [SerializeField] private float maxHp;
    [SerializeField] private float lowHealthThreshold;
    [SerializeField] private float healthRestoreRate;

    [SerializeField] private float chasingRange;
    [SerializeField] private float shootingRange;

    [SerializeField] private Transform playerTrans;

    private Material material;


    private float nowHp
    {
        get { return nowHp; }
        set { nowHp = Mathf.Clamp(value, 0, maxHp); }
    }


    void Start()
    {

        nowHp = maxHp;

        material = GetComponent<Material>();
    }

    void Update()
    {

        nowHp += Time.deltaTime * healthRestoreRate;
    }

    public float GetCurrentHealth()
    {

        return nowHp;
    }

    public void SetColor(Color color)
    {

        material.color = color;
    }
}
