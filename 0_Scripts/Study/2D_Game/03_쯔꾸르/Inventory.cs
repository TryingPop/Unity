using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public static Inventory instance;

    private DatabaseManager theDatabase;

    private OrderManager theOrder;

    private AudioManager theAudio;
    public string key_sound;
    public string enter_sound;
    public string cancel_sound;
    public string open_sound;
    public string beep_sound;

    private InventorySlot[] slots;      // 인벤토리 슬롯들

    private List<Item> inventoryItemList;   // 플레이어가 소지한 아이템 리스트
    private List<Item> inventoryTabList;    // 선택한 탭에 따라 다르게 보여질 아이템 리스트

    public Text Description_Text;           // 부연 설명
    public string[] tabDescription;         // 탭 부연 설명

    public Transform tf;                    // 부모객체

    public GameObject go;                   // 인벤토리 활성화 비활성화
    public GameObject[] selectedTabImages;

    public GameObject go_OOC;               // 선택지 활성화 비활성화

    public GameObject prefab_Floating_Text; 

    public int selectedItem;                // 선택된 아이템
    private int selectedTab;                // 선택된 텝

    private bool activated;                 // 인벤토리 활성화 시 true
    private bool tabActivated;              // 탭 활성화 시 true
    private bool itemActivated;             // 아이템 활성화 시 true
    private bool stopKeyInput;              // 키입력 제한 (소비할 때 질의가 나올 텐데, 그때 입력 방지)
    private bool preventExec;               // 중복실행 제한

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private OkOrCancel theOOC;

    private Equipment theEquip;

    private int page;                       // 페이지
    private int slotCount;                  // 활성화된 슬롯의 개수
    private const int MAX_SLOTS_COUNT = 12; // 최대 슬롯 개수
    
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


    private void Start()
    {
        
        theDatabase = FindObjectOfType<DatabaseManager>();
        theOrder = FindObjectOfType<OrderManager>();
        theAudio = FindObjectOfType<AudioManager>();
        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        slots = tf.GetComponentsInChildren<InventorySlot>();
        theOOC = FindObjectOfType<OkOrCancel>();
        theEquip = FindObjectOfType<Equipment>();
    }

    private void Update()
    {
        
        if (!stopKeyInput)
        {

            if (Input.GetKeyDown(KeyCode.I))
            {

                activated = !activated;

                if (activated)
                {

                    theAudio.Play(open_sound);
                    theOrder.NotMove();
                    go.SetActive(true);
                    selectedTab = 0;
                    tabActivated = true;
                    itemActivated = false;

                    ShowTab();
                }
                else
                {

                    theAudio.Play(cancel_sound);
                    StopAllCoroutines();
                    go.SetActive(false);
                    tabActivated = false;
                    itemActivated = false;
                    theOrder.Move();
                }
            }

            if (activated)
            {

                // 탭 활성화 시 키 입력 처리
                if (tabActivated)
                {

                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {

                        if (selectedTab < selectedTabImages.Length - 1)
                        {

                            selectedTab++;
                        }
                        else
                        {

                            selectedTab = 0;
                        }

                        theAudio.Play(key_sound);
                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {

                        if (selectedTab > 0)
                        {

                            selectedTab--;
                        }
                        else
                        {

                            selectedTab = selectedTabImages.Length - 1;
                        }

                        theAudio.Play(key_sound);
                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.Z))
                    {

                        theAudio.Play(enter_sound);
                        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
                        color.a = 0.25f;
                        selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                        itemActivated = true;
                        tabActivated = false;
                        preventExec = true;

                        ShowItem();
                    }
                }
                // 아이템 활성화 시 키 입력 처리
                else if (itemActivated)
                {

                    // 아이템이 없는데도 이동 소리 나오는거 방지
                    if (inventoryTabList.Count > 0)
                    {

                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {

                            if (selectedItem + 2 > slotCount)
                            {

                                if (page < (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT)
                                {

                                    page++;
                                }
                                else
                                {

                                    page = 0;
                                }

                                RemoveSlot();
                                ShowPage();
                                selectedItem = -2;
                            }

                            if (selectedItem < slotCount - 1)
                            {

                                selectedItem += 2;
                            }
                            else
                            {

                                selectedItem %= 2;
                            }

                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.UpArrow))
                        {

                            if (selectedItem - 2 > 0)
                            {

                                if (page > 0)
                                {

                                    page--;
                                }
                                else
                                {

                                    page = (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT;
                                }
                                RemoveSlot();
                                ShowPage();
                            }

                            if (selectedItem > 1)
                            {

                                selectedItem -= 2;
                            }
                            else
                            {

                                selectedItem = slotCount - selectedItem;
                            }

                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow))
                        {

                            if (selectedItem + 1 > slotCount)
                            {

                                if (page < (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT)
                                {

                                    page++;
                                }
                                else 
                                { 
                                    
                                    page = 0; 
                                }

                                RemoveSlot();
                                ShowPage();
                                selectedItem = -1;
                            }

                            if (selectedItem < slotCount)
                            {

                                selectedItem++;
                            }
                            else
                            {

                                selectedItem = 0;
                            }

                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {

                            if (selectedItem - 1 > 0)
                            {

                                if (page > 0)
                                {

                                    page--;
                                }
                                else
                                {

                                    page = (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT;
                                }
                                RemoveSlot();
                                ShowPage();
                            }

                            if (selectedItem > 0)
                            {

                                selectedItem--;
                            }
                            else
                            {

                                selectedItem = slotCount;
                            }

                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.Z) && !preventExec)
                        {

                            if (selectedTab == 0)
                            {

                                StartCoroutine(OOCCoroutine("사용", "취소"));
                            }
                            else if (selectedTab == 1)
                            {


                                StartCoroutine(OOCCoroutine("장착", "취소"));
                            }
                            else    // 비프음 출력
                            {

                                theAudio.Play(beep_sound);
                            }
                        }
                    }
                    
                    if (Input.GetKeyDown(KeyCode.X))
                    {

                        theAudio.Play(cancel_sound);
                        StopAllCoroutines();
                        itemActivated = false;
                        tabActivated = true;
                        ShowTab();
                    }
                }

                // 중복 실행 방지
                if (Input.GetKeyUp(KeyCode.Z))
                {

                    preventExec = false;
                }
            }
        }
    }


    // Ok or Cancel
    IEnumerator OOCCoroutine(string _up, string _down)
    {

        theAudio.Play(enter_sound);
        stopKeyInput = true;

        go_OOC.SetActive(true);
        theOOC.ShowTwoChoice(_up, _down);
        yield return new WaitUntil(() => !theOOC.activated);
        if (theOOC.GetResult())
        {

            for (int i = 0; i < inventoryItemList.Count; i++)
            {

                if (inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID)
                {

                    if (selectedTab == 0) 
                    {

                        theDatabase.UseItem(inventoryItemList[i].itemID);

                        if (inventoryItemList[i].itemCount > 1)
                        {

                            inventoryItemList[i].itemCount--;
                        }
                        else
                        {

                            inventoryItemList.RemoveAt(i);
                        }

                        // theAudio.Play(); // 아이템 사용 소리
                        ShowItem();
                        break;
                    }
                    else if (selectedTab == 1)
                    {

                        theEquip.EquipItem(inventoryItemList[i]);
                        inventoryItemList.RemoveAt(i);
                        ShowItem();
                    }
                }
            }
        }

        stopKeyInput = false;
        go_OOC.SetActive(false);
    }

    public void GetAnItem(int _itemID, int _count)
    {

        // 데이터 베이스 아이템 검색
        for (int i = 0; i < theDatabase.itemList.Count; i++)
        {

            // 데이터베이스에 아이템 발견
            if (_itemID == theDatabase.itemList[i].itemID)
            {

                var clone = Instantiate(prefab_Floating_Text, PlayerManager.instance.transform.position, 
                    Quaternion.Euler(Vector3.zero));    // Quaternion.identity;

                clone.GetComponent<FloatingText>().text.text = theDatabase.itemList[i].itemName + " " + _count + "개 획득 + ";
                clone.transform.SetParent(this.transform);  // 이 구문 대신에 instantiate의 4번째 인자에 추가해도 된다

                // 소지품에 같은 아이템이 있는지 검색
                for (int j = 0; j < inventoryItemList.Count; j++)
                {

                    // 소지품에 같은 아이템이 있다 -> 개수만 증가
                    if (inventoryItemList[j].itemID == _itemID)
                    {

                        if (inventoryItemList[i].itemType == Item.ItemType.Use)
                        {

                            inventoryItemList[j].itemCount += _count;
                        }
                        else
                        {

                            inventoryItemList.Add(theDatabase.itemList[i]);
                        }
                        return;
                    }
                }

                // 소지품에 해당 아이템 추가
                inventoryItemList.Add(theDatabase.itemList[i]);
                inventoryItemList[inventoryItemList.Count - 1].itemCount = _count; 
                return;
            }
        }

        // 데이터베이스에 ItemID 없음
        Debug.LogError("데이터베이스에 해당 ID것을 가진 아이템이 존재하지 않습니다.");
    }

    // 탭 활성화
    public void ShowTab()
    {

        RemoveSlot();
        SelectedTab();
    }

    // 선택된 탭을 제외하고 다른 모든 탭의 컬러 알파 값 0으로 조정
    public void SelectedTab()
    {

        StopAllCoroutines();
        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
        color.a = 0f;
        for (int i = 0; i < selectedTabImages.Length; i++)
        {

            selectedTabImages[i].GetComponent<Image>().color = color;
        }
        Description_Text.text = tabDescription[selectedTab];
        StartCoroutine(SelectedTabEffectCoroutine());
    }

    // 인벤토리 슬롯 초기화
    public void RemoveSlot()
    {

        for (int i = 0; i < slots.Length; i++)
        {

            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    }

    IEnumerator SelectedTabEffectCoroutine()
    {

        while (tabActivated)
        {

            Color color = selectedTabImages[0].GetComponent<Image>().color;
            while(color.a < 0.5f)
            {

                color.a += 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {

                color.a -= 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator SelectedItemEffectCoroutine()
    {

        while (itemActivated)
        {

            Color color = slots[0].GetComponent<Image>().color;
            while(color.a < 0.5f)
            {

                color.a += 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            } 
            while (color.a > 0f)
            {

                color.a -= 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    // 아이템 활성화 (inventoryTabList에 조건에 맞는 아이템들만 넣어주고, 인벤토리 슬롯에 출력)
    public void ShowItem()
    {

        inventoryTabList.Clear();

        RemoveSlot();
        selectedItem = 0;

        page = 0;

        switch (selectedTab)        
        {

            case 0:
                for (int i = 0; i <inventoryItemList.Count; i++)
                {

                    if (Item.ItemType.Use == inventoryItemList[i].itemType)
                    {

                        inventoryTabList.Add(inventoryItemList[i]);
                    }
                }
                break;

            case 1:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {

                    if (Item.ItemType.Equipment == inventoryItemList[i].itemType)
                    {

                        inventoryTabList.Add(inventoryItemList[i]);
                    }
                }
                break;

            case 2:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {

                    if (Item.ItemType.Quest == inventoryItemList[i].itemType)
                    {

                        inventoryItemList.Add(inventoryItemList[i]);
                    }
                }
                break;

            case 3:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {

                    if (Item.ItemType.ETC == inventoryItemList[i].itemType)
                    {

                        inventoryItemList.Add(inventoryItemList[i]);
                    }
                }
                break;
        }   // 탭에 따른 아이템 분류. 그것을 인벤토리 탭 리스트에 추가

        /*
        for (int i = 0; i < inventoryTabList.Count; i++)
        {

            slots[i].gameObject.SetActive(true);
            slots[i].AddItem(inventoryTabList[i]);
        }       // 인벤토리 탭 리스트의 내용을, 인벤토리 슬롯에 추가 메소드로 구현
        */

        ShowPage();
        SelectedItem();
    }

    // 선택된 아이템을 제외하고, 다른 모든 탭의 컬러 알파값을 0으로 조정
    public void SelectedItem()
    {

        StopAllCoroutines();
        if (slotCount > -1)
        {

            Color color = slots[0].selected_Item.GetComponent<Image>().color;
            color.a = 0;
            for (int i = 0; i <= slotCount; i++)
            {

                slots[i].selected_Item.GetComponent<Image>().color = color;
            }

            Description_Text.text = inventoryTabList[selectedItem].itemDescription;
            StartCoroutine(SelectedItemEffectCoroutine());
        }
        else
        {

            Description_Text.text = "해당 타입의 아이템을 소유하고 있지 않습니다.";
        }
    }

    // 아이템 추가 메소드
    public void EquipToInventory(Item _item)
    {

        inventoryItemList.Add(_item);
    }

    /// <summary>
    /// 세이브용 메소드
    /// </summary>
    /// <returns></returns>
    public List<Item> SaveItem()
    {

        return inventoryItemList;
    }

    /// <summary>
    /// 로드용 메소드
    /// </summary>
    /// <param name="_itemList"></param>
    public void LoadItem(List<Item> _itemList)
    {

        inventoryItemList = _itemList;
    }

    /// <summary>
    /// 페이지에 맞게 장비 소환
    /// </summary>
    public void ShowPage()
    {

        // 0으로 하면 한개 있을 때 반짝거리는 효과가 없다
        // 배열은 0부터 시작해서 슬롯 카운트가 1개 있다는 표시
        slotCount = -1;

        for (int i = page * MAX_SLOTS_COUNT; i < inventoryTabList.Count; i++)
        {
            slotCount = i - (page * MAX_SLOTS_COUNT);
            slots[slotCount].gameObject.SetActive(true);
            slots[slotCount].AddItem(inventoryTabList[i]);

            // 슬롯을 가득 채워주고 막아주는 역할
            if (slotCount == MAX_SLOTS_COUNT - 1)
            {

                break;
            }
        }
    }
}
