using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SaveNLoad : MonoBehaviour
{

    [System.Serializable]
    public class Data       // 세이브 데이터를 담을 클래스
    {

        // 커스텀 구조체도 직렬화가 가능하나 여기서는 안한다
        // 자세한건 좀비 게임 책에서 확인!
        public float playerX;
        public float playerY;
        public float playerZ;

        public int playerLv;
        public int playerHp;
        public int playerMp;

        public int playerCurrentHp;
        public int playerCurrentMp;
        public int playerCurrentExp;

        public int playerRecoverHp;
        public int playerRecoverMp;

        public int playerAtk;
        public int playerDef;

        public int added_atk;
        public int added_def;
        public int added_recover_hp;
        public int added_recover_mp;

        public List<int> playerItemInventory;           // 플레이어 아이템 ID
        public List<int> playerItemInventoryCount;      // 플레이어 장비 수량
        public List<int> playerEquipItem;               // 플레이어 장비 아이템 ID

        public string mapName;
        public string sceneName;

        public List<bool> swList;
        public List<string> swNameList;
        public List<string> varNameList;
        public List<float> varNumberList;
    }

    private PlayerManager thePlayer;
    private PlayerStat thePlayerStat;
    private DatabaseManager theDatabase;
    private Inventory theInven;
    private Equipment theEquip;

    public Data data;

    private Vector3 vector;

    private FadeManager theFade;

    public void CallSave()
    {

        theDatabase = FindObjectOfType<DatabaseManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        thePlayerStat = FindObjectOfType<PlayerStat>();
        theEquip = FindObjectOfType<Equipment>();
        theInven = FindObjectOfType<Inventory>();

        data.playerX = thePlayer.transform.position.x;
        data.playerY = thePlayer.transform.position.y;
        data.playerZ = thePlayer.transform.position.z;

        data.playerLv = thePlayerStat.character_Lv;
        data.playerHp = thePlayerStat.hp;
        data.playerMp = thePlayerStat.mp;

        data.playerCurrentHp = thePlayerStat.currentHp;
        data.playerCurrentMp = thePlayerStat.currentMp;
        data.playerCurrentExp = thePlayerStat.currentExp;

        data.playerRecoverHp = thePlayerStat.recover_hp;
        data.playerRecoverMp = thePlayerStat.recover_mp;

        data.playerAtk = thePlayerStat.atk;
        data.playerDef = thePlayerStat.def;

        data.added_atk = theEquip.added_atk;
        data.added_def = theEquip.added_def;
        data.added_recover_hp = theEquip.added_recover_hp;
        data.added_recover_mp = theEquip.added_recover_mp;


        data.mapName = thePlayer.currentMapName;
        data.sceneName = thePlayer.currentSceneName;

        Debug.Log("기초 데이터 입력 성공");

        data.varNameList.Clear();
        data.varNumberList.Clear();

        data.swNameList.Clear();
        data.swList.Clear();

        data.playerItemInventory.Clear();
        data.playerItemInventoryCount.Clear();

        data.playerEquipItem.Clear();

        for (int i = 0; i < theDatabase.var_name.Length; i++)
        {

            data.varNameList.Add(theDatabase.var_name[i]);
            data.varNumberList.Add(theDatabase.var[i]);
        }

        for (int i = 0; i < theDatabase.switch_name.Length; i++)
        {

            data.swNameList.Add(theDatabase.switch_name[i]);
            data.swList.Add(theDatabase.switches[i]);
        }

        // 여기서 장착 중인 아이템 갯수에 접근 못하므로 우회한 방법으로 접근한다
        List<Item> itemList = theInven.SaveItem();

        for (int i = 0; i < itemList.Count; i++)
        {

            data.playerItemInventory.Add(itemList[i].itemID);
            data.playerItemInventoryCount.Add(itemList[i].itemCount);
            Debug.Log("인벤토리의 아이템 저장 완료 : " + itemList[i].itemName);
        }

        for (int i = 0; i < theEquip.equipItemList.Length; i++)
        {

            data.playerEquipItem.Add(theEquip.equipItemList[i].itemID);
            Debug.Log("장착된 아이템 저장 완료 : " + theEquip.equipItemList[i].itemName);
        }

        // 외부 파일로 저장하는 코드
        BinaryFormatter bf = new BinaryFormatter();
        // 확장자는 마음대로 가능 
        using (FileStream file = File.Create(Application.dataPath + "/SaveFile.data"))
        {


            bf.Serialize(file, data);
        }

        Debug.Log(Application.dataPath + "의 위치에\nSaveFile.data 로 저장했습니다.");
    }

    internal void CallLoad()
    {

        // 외부 파일로 읽는 코드
        BinaryFormatter bf = new BinaryFormatter();
        // 확장자는 마음대로 가능 
        using (FileStream file = File.Open(Application.dataPath + "/SaveFile.dat", FileMode.Open))
        {

            // 파일이 있고 내용이 있는 경우만 실행
            if (file != null && file.Length > 0)
            {

                data = bf.Deserialize(file) as Data;
            }
            else
            {

                Debug.Log("저장된 세이브 파일이 없습니다.");
            }
        }

        if (data != null)
        {

            theDatabase = FindObjectOfType<DatabaseManager>();
            thePlayer = FindObjectOfType<PlayerManager>();
            thePlayerStat = FindObjectOfType<PlayerStat>();
            theEquip = FindObjectOfType<Equipment>();
            theInven = FindObjectOfType<Inventory>();

            theFade = FindObjectOfType<FadeManager>();

            theFade.FadeOut();

            thePlayer.currentMapName = data.mapName;
            thePlayer.currentSceneName = data.sceneName;

            vector.Set(data.playerX, data.playerY, data.playerZ);
            thePlayer.transform.position = vector;

            thePlayerStat.character_Lv = data.playerLv;
            thePlayerStat.hp = data.playerHp;
            thePlayerStat.mp = data.playerMp;

            thePlayerStat.currentHp = data.playerCurrentHp;
            thePlayerStat.currentMp = data.playerCurrentMp;
            thePlayerStat.currentExp = data.playerCurrentExp;

            thePlayerStat.recover_hp = data.playerRecoverHp;
            thePlayerStat.recover_mp = data.playerRecoverMp;

            thePlayerStat.atk = data.playerAtk;
            thePlayerStat.def = data.playerDef;

            theEquip.added_atk = data.added_atk; 
            theEquip.added_def = data.added_def;
            theEquip.added_recover_hp = data.added_recover_hp;
            theEquip.added_recover_mp = data.added_recover_mp;

            theDatabase.var = data.varNumberList.ToArray();
            theDatabase.var_name = data.varNameList.ToArray();
            theDatabase.switches = data.swList.ToArray();
            theDatabase.switch_name = data.swNameList.ToArray();

            for (int i = 0;  i < theEquip.equipItemList.Length; i++)
            {

                for (int x = 0; x < theDatabase.itemList.Count; x++)
                {

                    if (data.playerEquipItem[i] == theDatabase.itemList[x].itemID)
                    {

                        theEquip.equipItemList[i] = theDatabase.itemList[x];
                        Debug.Log("장착된 아이템을 로드 했습니다 : " + theEquip.equipItemList[i].itemName);
                        break;
                    }
                }
            }

            List<Item> itemList = new List<Item>();
            for (int i = 0; i < data.playerItemInventory.Count; i++)
            {

                for (int x = 0; x < theDatabase.itemList.Count; x++)
                {

                    if (data.playerItemInventory[i] == theDatabase.itemList[x].itemID)
                    {

                        itemList.Add(theDatabase.itemList[x]);
                        Debug.Log("인벤토리 아이템을 로드 했습니다 : " + theDatabase.itemList[x].itemID);
                        break;
                    }
                }
            }

            for (int i = 0; i <data.playerItemInventoryCount.Count; i++)
            {

                itemList[i].itemCount = data.playerItemInventoryCount[i];
            }

            theInven.LoadItem(itemList);
            theEquip.ShowTxt();

            StartCoroutine(WaitCoroutine());
        }
    }

    IEnumerator WaitCoroutine()
    {

        yield return new WaitForSeconds(2f);
        GameManager theGM = FindObjectOfType<GameManager>();
        theGM.LoadStart();

        SceneManager.LoadScene(data.sceneName);
    }
}
