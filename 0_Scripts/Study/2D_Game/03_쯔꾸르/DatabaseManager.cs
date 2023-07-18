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

    // 1. �� �̵� A < - > B. ��ü �ı��Ǿ� ���� �ʱ�ȭ �ȵǰ� ����
    // 2. ���̺�� �ε�
    // 3. �̸� ����� �θ� ���ϴ�! ������
    // ������ xml���� ���� �̿��ؾ� �ϴµ� ���� ���� ��ũ��Ʈ�� ����!

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
                // Debug.Log("Hp�� 50 ȸ���Ǿ����ϴ�.");
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
        itemList.Add(new Item(10001, "���� ����", "ü���� 50 ȸ��", Item.ItemType.Use));
        itemList.Add(new Item(20001, "ª�� ��", "�⺻���� ����� ��", Item.ItemType.Equipment, 1, 3, 0, 0, 0));    // �� 3 ����
    }
}