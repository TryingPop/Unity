using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{

    public int itemID;                  // �������� ���� ID��, �ߺ� �Ұ��� 
    public string itemName;             // �������� �̸�, �ߺ� ����
    public string itemDescription;      // ������ ����

    public int itemCount;               // ���� ����
    public Sprite itemIcon;             // �������� ������

    public ItemType itemType;

    public enum ItemType
    {

        Use,
        Equipment,
        Quest,
        ETC
    }

    // ��� �߰��� ����
    public int atk;
    public int def;

    public int recover_hp;
    public int recover_mp;

    public Item(int _itemID, string _itemName, string _itemDes, ItemType _itemType, int _itemCount = 1, 
        int _atk = 0, int _def = 0, int _recover_hp = 0, int _recover_mp = 0)
    {

        itemID = _itemID;
        itemName = _itemName;
        itemDescription = _itemDes;
        itemType = _itemType;
        itemCount = _itemCount;

        // Assets ���� �ٷ� �Ʒ��� Resources �� �ش� ������ �־�����Ѵ�
        // Resources ������ ���ٸ� �ش� �̸����� �����ϸ� �ȴ�
        itemIcon = Resources.Load("ItemIcon/" + itemID.ToString(), typeof(Sprite)) as Sprite;

        atk = _atk;
        def = _def;
        recover_hp = _recover_hp;
        recover_mp = _recover_mp;
    }
}
