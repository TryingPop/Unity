using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{


    public static DatabaseManager instance;

    private void Awake()
    {
        
        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {

            Destroy(gameObject);
        }
    }

    // 1. 씬 이동 A < - > B. 객체 파괴되어 내용 초기화 안되게 관리
    // 2. 세이브와 로드
    // 3. 미리 만들어 두면 편하다! 아이템
    // 원래는 xml같은 것을 이용해야 하는데 양이 적어 스크립트로 관리!

    public string[] var_name;
    public float[] var;

    public string[] switch_name;
    public bool[] switches;

    public List<Item> itemList = new List<Item>();

    private void Start()
    {

        // itemList.Add(new Item(10001, "빨간 포션", "체력을 50 회복", Item.ItemType.Use));
    }
}