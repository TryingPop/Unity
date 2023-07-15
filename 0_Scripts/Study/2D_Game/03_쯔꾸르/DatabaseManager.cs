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

    // 1. �� �̵� A < - > B. ��ü �ı��Ǿ� ���� �ʱ�ȭ �ȵǰ� ����
    // 2. ���̺�� �ε�
    // 3. �̸� ����� �θ� ���ϴ�! ������
    // ������ xml���� ���� �̿��ؾ� �ϴµ� ���� ���� ��ũ��Ʈ�� ����!

    public string[] var_name;
    public float[] var;

    public string[] switch_name;
    public bool[] switches;

    public List<Item> itemList = new List<Item>();

    private void Start()
    {

        // itemList.Add(new Item(10001, "���� ����", "ü���� 50 ȸ��", Item.ItemType.Use));
    }
}