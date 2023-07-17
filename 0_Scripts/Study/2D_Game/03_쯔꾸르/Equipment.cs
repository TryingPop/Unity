using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{

    private OrderManager theOrder;
    private AudioManager theAudio;
    private PlayerStat thePlayerStat;
    private Inventory theInven;
    private OkOrCancel theOOC;


    public string key_sound;
    public string enter_sound;
    public string open_sound;
    public string close_sound;
    public string takeoff_sound;


    private const int WEAPON = 0,       // enum 대신에 쓴다면 다음과 같이 이용 -> 하드코딩 방지
                    SHIELD = 1,
                    AMULET = 2,
                    LEFT_RING = 3,
                    RIGHT_RING = 4,
                    HELMET = 5,
                    ARMOR = 6,
                    LEFT_GLOVE = 7,
                    RIGHT_GLOVE = 8,
                    BELT = 9,
                    LEFT_BOOTS = 10,
                    RIGHT_BOOTS = 11;

    public GameObject go;
    public GameObject go_OOC;


    public Text[] texts;                    // 스텟
    public Image[] img_slots;               // 장비 슬롯 아이콘들
    public GameObject go_selected_Slot_UI;  // 선택된 장비 슬롯 UI

    public Item[] equipItemList;            // 장착된 장비 리스트

    private int selectedSlot;               // 선택된 장비 슬롯

    public bool activated = false;
    private bool inputKey = true;

    private void Start()
    {

        theInven = FindObjectOfType<Inventory>();
        thePlayerStat = FindObjectOfType<PlayerStat>();
        theAudio = FindObjectOfType<AudioManager>();
        theOrder = FindObjectOfType<OrderManager>();
        theOOC = FindObjectOfType<OkOrCancel>();
    }


    private void Update()
    {

        if (inputKey)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {

                activated = !activated;

                if (activated)
                {

                    theOrder.NotMove();
                    theAudio.Play(open_sound);
                    go.SetActive(true);
                    selectedSlot = 0;
                    SelectedSlot();
                    ClearEquip();
                    ShowEquip();

                }
                else
                {

                    theOrder.Move();
                    theAudio.Play(close_sound);
                    go.SetActive(false);
                    ClearEquip();
                }
            }

            if (activated)
            {

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {

                    if (selectedSlot > 0)
                    {

                        selectedSlot--;
                    }
                    else
                    {

                        selectedSlot = img_slots.Length - 1;
                    }

                    SelectedSlot();
                    theAudio.Play(key_sound);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {

                    if (selectedSlot < img_slots.Length - 1)
                    {

                        selectedSlot++;
                    }
                    else
                    {

                        selectedSlot = 0;
                    }

                    SelectedSlot();
                    theAudio.Play(key_sound);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {

                    if (selectedSlot < img_slots.Length - 1)
                    {

                        selectedSlot++;
                    }
                    else
                    {

                        selectedSlot = 0;
                    }

                    SelectedSlot();
                    theAudio.Play(key_sound);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {


                    if (selectedSlot > 0)
                    {

                        selectedSlot--;
                    }
                    else
                    {

                        selectedSlot = img_slots.Length - 1;
                    }

                    SelectedSlot();
                    theAudio.Play(key_sound);
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {

                    // 아무것도 없는데 해제되는 것을 방지
                    if (equipItemList[selectedSlot].itemID != 0)
                    {

                        theAudio.Play(enter_sound);
                        inputKey = false;
                        StartCoroutine(OOCCoroutine("벗기", "취소"));
                    }
                    else
                    {

                        // 키입력이 된걸 확인하기 위해서
                        // Beep 소리 넣기!
                    }
                }
            }
        }
    }

    public void ClearEquip()
    {

        Color color = img_slots[0].color;
        color.a = 0f;

        for (int i = 0; i < img_slots.Length; i++)
        {

            img_slots[i].sprite = null;
            img_slots[i].color = color;
        }
    }

    public void ShowEquip()
    {

        Color color = img_slots[0].color;
        color.a = 1f;

        for (int i = 0; i < img_slots.Length; i++)
        {

            if (equipItemList[i].itemID != 0)
            {

                img_slots[i].sprite = equipItemList[i].itemIcon;
                img_slots[i].color = color;
            }
        }
    }

    public void SelectedSlot()
    {

        go_selected_Slot_UI.transform.position = img_slots[selectedSlot].transform.position;
    }

    public void EquipItem(Item _item)
    {

        string temp = _item.itemID.ToString();

        // 장비 아이디의 맨앞을 2로 설정
        // 그리고 앞에 2,3번째 자리수에 의해 장비 타입이 결정된다
        temp = temp.Substring(0, 3);

        switch (temp)
        {

            // 대표적으로만 표시
            case "200":     // 무기
                EquipItemCheck(WEAPON, _item);
                break;  

            case "201":     // 방패
                EquipItemCheck(SHIELD, _item);
                break;

            case "202":     // 아뮬렛
                EquipItemCheck(AMULET, _item);
                break;

            case "203":     // 반지
                EquipItemCheck(LEFT_RING, _item);
                break;

        }
    }

    public void EquipItemCheck(int _count, Item _item)
    {

        if (equipItemList[_count].itemID == 0)
        {

            equipItemList[_count] = _item;
        }
        else
        {

            theInven.EquipToInventory(equipItemList[_count]);
            equipItemList[_count] = _item;
        }
    }

    IEnumerator OOCCoroutine(string _up, string _down)
    {

        go_OOC.SetActive(true);
        theOOC.ShowTwoChoice(_up, _down);
        yield return new WaitUntil(() => !theOOC.activated);

        if (theOOC.GetResult())
        {

            theInven.EquipToInventory(equipItemList[selectedSlot]);
            equipItemList[selectedSlot] = new Item(0, "", "", Item.ItemType.Equipment);
            theAudio.Play(takeoff_sound);
            ClearEquip();
            ShowEquip();
        }

        inputKey = true;
        go_OOC.SetActive(false);
    }
}
