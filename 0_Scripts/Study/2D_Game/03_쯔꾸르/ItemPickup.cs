using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    public int itemID;
    public int count;
    public string pickUpSound;

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (Input.GetKeyDown(KeyCode.Z))
        {

            AudioManager.instance.Play(pickUpSound);    // 자주 쓰면 변수로 만들어 주는 것이 좋다

            Inventory.instance.GetAnItem(itemID, count);
            Destroy(gameObject);
        }
    }
}
