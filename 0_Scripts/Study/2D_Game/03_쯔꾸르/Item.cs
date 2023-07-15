using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{

    public int itemID;                  // 아이템의 고유 ID값, 중복 불가능 
    public string itemName;             // 아이템의 이름, 중복 가능
    public string itemDescription;      // 아이템 설명

    public int itemCount;               // 소지 개수
    public Sprite itemIcon;             // 아이템의 아이콘

    public ItemType itemType;

    public enum ItemType
    {

        Use,
        Equipment,
        Quest,
        ETC
    }

    public Item(int _itemID, string _itemName, string _itemDes, ItemType _itemType, int _itemCount = 1)
    {

        itemID = _itemID;
        itemName = _itemName;
        itemDescription = _itemDes;
        itemType = _itemType;
        itemCount = _itemCount;

        // Assets 폴더 바로 아래의 Resources 에 해당 파일을 넣어줘야한다
        // Resources 폴더가 없다면 해당 이름으로 생성하면 된다
        itemIcon = Resources.Load("ItemIcon/" + itemID.ToString(), typeof(Sprite)) as Sprite;
    }
}
