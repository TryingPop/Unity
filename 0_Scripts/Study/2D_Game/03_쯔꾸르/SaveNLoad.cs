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
    public class Data       // ���̺� �����͸� ���� Ŭ����
    {

        // Ŀ���� ����ü�� ����ȭ�� �����ϳ� ���⼭�� ���Ѵ�
        // �ڼ��Ѱ� ���� ���� å���� Ȯ��!
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

        public List<int> playerItemInventory;           // �÷��̾� ������ ID
        public List<int> playerItemInventoryCount;      // �÷��̾� ��� ����
        public List<int> playerEquipItem;               // �÷��̾� ��� ������ ID

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

        Debug.Log("���� ������ �Է� ����");

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

        // ���⼭ ���� ���� ������ ������ ���� ���ϹǷ� ��ȸ�� ������� �����Ѵ�
        List<Item> itemList = theInven.SaveItem();

        for (int i = 0; i < itemList.Count; i++)
        {

            data.playerItemInventory.Add(itemList[i].itemID);
            data.playerItemInventoryCount.Add(itemList[i].itemCount);
            Debug.Log("�κ��丮�� ������ ���� �Ϸ� : " + itemList[i].itemName);
        }

        for (int i = 0; i < theEquip.equipItemList.Length; i++)
        {

            data.playerEquipItem.Add(theEquip.equipItemList[i].itemID);
            Debug.Log("������ ������ ���� �Ϸ� : " + theEquip.equipItemList[i].itemName);
        }

        // �ܺ� ���Ϸ� �����ϴ� �ڵ�
        BinaryFormatter bf = new BinaryFormatter();
        // Ȯ���ڴ� ������� ���� 
        using (FileStream file = File.Create(Application.dataPath + "/SaveFile.data"))
        {


            bf.Serialize(file, data);
        }

        Debug.Log(Application.dataPath + "�� ��ġ��\nSaveFile.data �� �����߽��ϴ�.");
    }

    internal void CallLoad()
    {

        // �ܺ� ���Ϸ� �д� �ڵ�
        BinaryFormatter bf = new BinaryFormatter();
        // Ȯ���ڴ� ������� ���� 
        using (FileStream file = File.Open(Application.dataPath + "/SaveFile.dat", FileMode.Open))
        {

            // ������ �ְ� ������ �ִ� ��츸 ����
            if (file != null && file.Length > 0)
            {

                data = bf.Deserialize(file) as Data;
            }
            else
            {

                Debug.Log("����� ���̺� ������ �����ϴ�.");
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
                        Debug.Log("������ �������� �ε� �߽��ϴ� : " + theEquip.equipItemList[i].itemName);
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
                        Debug.Log("�κ��丮 �������� �ε� �߽��ϴ� : " + theDatabase.itemList[x].itemID);
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
