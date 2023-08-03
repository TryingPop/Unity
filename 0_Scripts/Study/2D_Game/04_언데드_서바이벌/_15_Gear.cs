using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _15_Gear : MonoBehaviour
{

    public _13_ItemData.ItemType type;

    // ������ ������
    public float rate;

    public void Init(_13_ItemData data)
    {

        // Basic Set
        name = "Gear " + data.itemId;
        transform.parent = _3_GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        type = data.itemType;
        rate = data.damages[0];

        ApplyGear();
    }

    private void ApplyGear()
    {

        switch (type)
        {

            case _13_ItemData.ItemType.Glove:

                RateUp();
                break;

            case _13_ItemData.ItemType.Shoe:

                SpeedUp();
                break;
        }
    }

    public void LevelUp(float rate)
    {

        this.rate = rate;

        ApplyGear();
    }

    private void RateUp()
    {

        _9_Weapon[] weapons = transform.parent.GetComponentsInChildren<_9_Weapon>();

        foreach(_9_Weapon weapon in weapons)
        {

            switch (weapon.id)
            {

                case 0:
                    // ȸ�� �ӵ�
                    weapon.speed = 150 + (150 * rate);

                    break;

                default:
                    // �÷��̾��� �⺻ ���� �ӵ�
                    weapon.speed = 0.5f * (1f - rate);
                    break;
            }
        }
    }

    private void SpeedUp()
    {

        // �÷��̾��� �⺻ �ӵ�
        float speed = 3f;
        _3_GameManager.instance.player.speed = speed + speed * rate;
    }
}
