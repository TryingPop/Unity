using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _14_Item : MonoBehaviour
{

    public _13_ItemData data;
    public int level;
    public _9_Weapon weapon;
    public _15_Gear gear;

    private Image icon;
    private Text textLevel;
    private Text textName;
    private Text textDesc;

    private void Awake()
    {

        // 자기자신도 이미지가 포함되어져 있다
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        // 텍스트는 설명글 같이 여러 개 이다
        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];

        textName.text = data.itemName;
    }

    private void OnEnable()
    {

        textLevel.text = $"Lv.{level + 1}";
        switch (data.itemType)
        {

            case _13_ItemData.ItemType.Melee:
            case _13_ItemData.ItemType.Range:

                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;

            case _13_ItemData.ItemType.Glove:
            case _13_ItemData.ItemType.Shoe:

                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;

            default:
                textDesc.text = string.Format(data.itemDesc);
                break;
        }
        
    }

    /*
    private void LateUpdate()
    {

        textLevel.text = $"Lv.{(level) + 1}";
    }
    */
    public void OnClick()
    {

        switch (data.itemType)
        {

            // Melee와 Range 이용 방법은 같다
            case _13_ItemData.ItemType.Melee:
            case _13_ItemData.ItemType.Range:
                if (level == 0)
                {

                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<_9_Weapon>();
                    weapon.Init(data);
                }
                else
                { 

                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }

                level++;
                break;

            case _13_ItemData.ItemType.Glove:
            case _13_ItemData.ItemType.Shoe:
                if (level == 0)
                {

                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<_15_Gear>();
                    gear.Init(data);
                }
                else
                {

                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }

                level++;
                break;

            case _13_ItemData.ItemType.Heal:

                _3_GameManager.instance.health = _3_GameManager.instance.maxHealth;
                break;
        }


        if (level == data.damages.Length)
        {

            // 만렙이 되면 비활성화
            GetComponent<Button>().interactable = false;
        }
    }
}
