using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Item")]
public class Item : ScriptableObject
{

    public enum ItemType
    {

        Equipment,                      // 장비
        Used,                           // 소모품
        Ingredient,                     // 재료
        ETC,
    }

    [TextArea] public string itemDesc;  // 아이템 설명
                                        // TextArea는 기존에 한줄만 사용가능하던게 
                                        // 엔터키를 이용해 두 줄 이상 이용가능
    public string itemName;             // 아이템의 이름
    public ItemType itemType;           // 아이템의 유형
    public Sprite itemImage;            // 아이템의 이미지
    public GameObject itemPrefab;       // 아이템의 프리팹

    public string weaponType;           // 무기 유형
}
