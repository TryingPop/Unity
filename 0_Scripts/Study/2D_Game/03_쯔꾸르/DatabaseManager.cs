using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{


    public static DatabaseManager instance;

    private PlayerStat thePlayerStat;

    public GameObject prefabs_Floating_Text;
    public GameObject parent;

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

    private void FloatText(int number, string color)
    {

        Vector3 vector = thePlayerStat.transform.position;
        vector.y += 60;

        GameObject clone = Instantiate(prefabs_Floating_Text, vector, Quaternion.Euler(Vector3.zero));

        FloatingText floatText = clone.GetComponent<FloatingText>();

        floatText.text.text = number.ToString();

        if (color == "GREEN")
        {

            floatText.text.color = Color.green;
        }
        else if (color == "BLUE")
        {

            floatText.text.color = Color.blue;
        }

        floatText.text.fontSize = 25;
        clone.transform.SetParent(parent.transform);
    }

    public void UseItem(int _itemID)
    {

        switch (_itemID)
        {

            case 10001:
                // Debug.Log("Hp가 50 회복되었습니다.");
                if (thePlayerStat.hp >= thePlayerStat.currentHp + 50)
                {

                    thePlayerStat.currentHp += 50;
                }
                else
                {

                    thePlayerStat.currentHp = thePlayerStat.hp;
                }

                FloatText(50, "GREEN");
                break;
        }
    }

    private void Start()
    {

        thePlayerStat = FindObjectOfType<PlayerStat>();
        itemList.Add(new Item(10001, "빨간 포션", "체력을 50 회복", Item.ItemType.Use));
        itemList.Add(new Item(20001, "짧은 검", "기본적인 용사의 검", Item.ItemType.Equipment, 1, 3, 0, 0, 0));    // 공 3 증가
    }
}