using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _3_GameManager : MonoBehaviour
{

    // ����� �ϳ��� �̱������δ� �ȸ����
    public static _3_GameManager instance;

    public _1_Player player;

    private void Awake()
    {
        
        instance = this;
    }
}