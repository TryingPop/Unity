using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _3_GameManager : MonoBehaviour
{

    // 장면이 하나라서 싱글톤으로는 안만든다
    public static _3_GameManager instance;

    public _1_Player player;

    private void Awake()
    {
        
        instance = this;
    }
}