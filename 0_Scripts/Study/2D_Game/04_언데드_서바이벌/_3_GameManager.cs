using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _3_GameManager : MonoBehaviour
{

    // ����� �ϳ��� �̱������δ� �ȸ����
    public static _3_GameManager instance;

    public _1_Player player;
    public _5_PoolManager pool;

    private void Awake()
    {
        
        instance = this;
    }
}