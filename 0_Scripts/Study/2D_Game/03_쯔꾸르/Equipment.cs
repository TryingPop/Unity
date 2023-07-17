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


    private const int WEAPON = 0,       // enum ��ſ� ���ٸ� ������ ���� �̿� -> �ϵ��ڵ� ����
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


    public Text[] texts;                    // ����
    public Image[] img_slots;               // ��� ���� �����ܵ�
    public GameObject go_selected_Slot_UI;  // ���õ� ��� ���� UI

    public Item[] equipItemList;            // ������ ��� ����Ʈ

    private int selectedSlot;               // ���õ� ��� ����

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

                    // �ƹ��͵� ���µ� �����Ǵ� ���� ����
                    if (equipItemList[selectedSlot].itemID != 0)
                    {

                        theAudio.Play(enter_sound);
                        inputKey = false;
                        StartCoroutine(OOCCoroutine("����", "���"));
                    }
                    else
                    {

                        // Ű�Է��� �Ȱ� Ȯ���ϱ� ���ؼ�
                        // Beep �Ҹ� �ֱ�!
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

        // ��� ���̵��� �Ǿ��� 2�� ����
        // �׸��� �տ� 2,3��° �ڸ����� ���� ��� Ÿ���� �����ȴ�
        temp = temp.Substring(0, 3);

        switch (temp)
        {

            // ��ǥ�����θ� ǥ��
            case "200":     // ����
                EquipItemCheck(WEAPON, _item);
                break;  

            case "201":     // ����
                EquipItemCheck(SHIELD, _item);
                break;

            case "202":     // �ƹķ�
                EquipItemCheck(AMULET, _item);
                break;

            case "203":     // ����
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
