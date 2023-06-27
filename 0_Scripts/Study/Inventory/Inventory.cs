using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    
    public static bool inventoryActivated = false;  // 카메라 이동 막기


    // 필요한 컴포넌트
    [SerializeField] GameObject go_InventoryBase;   // 
    [SerializeField] GameObject go_SlotsParent;

    private Slot[] slots;   // 슬롯

    private void Start()
    {

        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    private void Update()
    {

        TypeOpenInventory();
    }

    private void TypeOpenInventory()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {

            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
            {

                OpenInventory();
            }
            else
            {

                CloseInventory();
            }
        }
    }

    private void OpenInventory()
    {

        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {

        go_InventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count)
    {

        if (_item == null) 
        {

            return;
        }

        if (Item.ItemType.Equipment != _item.itemType)
        {

            for (int i = 0; i < slots.Length; i++)
            {

                if (slots[i].item != null)
                {

                    if (slots[i].item.itemName == _item.itemName)
                    {

                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }



        for (int i = 0; i < slots.Length; i++)
        {

            if (slots[i].item == null)
            {

                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}
