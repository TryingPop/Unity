using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zFoxGameObjectLoader : MonoBehaviour
{

    // �ܺ� �Ķ���� (Inspector ǥ��)
    public GameObject[] LoadGameObjectList_Awake;
    public GameObject[] LoadGameObjectList_Start;
    public GameObject[] LoadGameObjectList_Update;
    public GameObject[] LoadGameObjectList_FixedUpdate;

    // �ܺ� �Ķ����
    [HideInInspector]
    public Dictionary<string, GameObject>
        loadedGameObjectList_Awake = new Dictionary<string, GameObject>();
    [HideInInspector] public bool loaded_Awake = false;
    [HideInInspector] public Dictionary<string, GameObject>
        loadedGameObjectList_Start = new Dictionary<string, GameObject>();
    [HideInInspector] public bool loaded_Start = false;
    [HideInInspector] public Dictionary<string, GameObject>
        loadedGameObjectList_Update = new Dictionary<string, GameObject>();
    [HideInInspector] public bool loaded_Update = false;
    [HideInInspector] public Dictionary<string, GameObject>
        loadedGameObjectList_FixedUpdate = new Dictionary<string, GameObject>();
    [HideInInspector] public bool loaded_FixedUpdate = false;

    // ���� �Ķ����
    bool loaded = false;

    // �ڵ� (MonoBehaviour �⺻ ��� ����)
    void Awake()
    {

        // �� ���� ������Ʈ�� �ε�ƴ��� �˻�
        bool loadedAll = false;
        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject go in gos)
        {

            zFoxGameObjectLoader fol = go.GetComponent<zFoxGameObjectLoader>();
            if (fol)
            {

                if (fol.loaded)
                {

                    loadedAll = true;
                    break;
                }
            }
        }

        if (loadedAll)
        {

            Destroy(gameObject);
            return;
        }

        loaded = true;

        // Awake ó�� ����
        if (loaded_Awake)
        {

            loaded_Awake = true;
            LoadGameObject(LoadGameObjectList_Awake, loadedGameObjectList_Awake);
        }
    }

    void Start()
    {
        
        // Start ó�� ����
        if (!loaded_Start)
        {

            loaded_Start = true;
            LoadGameObject(LoadGameObjectList_Start, loadedGameObjectList_Start);
        }
    }

    void Update()
    {
        
        // Update ó�� ����
        if (!loaded_Update)
        {

            loaded_Update = true;
            LoadGameObject(LoadGameObjectList_Update, loadedGameObjectList_Update);
        }
    }


    void FixedUpdate()
    {
        
        // FixedUpdate ó�� ����
        if (!loaded_FixedUpdate)
        {

            loaded_FixedUpdate = true;
            LoadGameObject(LoadGameObjectList_FixedUpdate, loadedGameObjectList_FixedUpdate);
        }
    }


    void LoadGameObject(GameObject[] loadGameObjectList, 
        Dictionary<string, GameObject> loadedGameObjectList)
    {

        // �δ��� �� ��ȯ �� �������� �ʵ��� ����
        // �ε��� ���� ������Ʈ�� �ڽĿ��� �����ǹǷ� �ε�� �͵� �������� �ʴ´�
        DontDestroyOnLoad(this);

        // ��ϵǾ� �ִ� ���� ������Ʈ�� �ҷ��´�
        foreach(GameObject go in loadGameObjectList)
        {

            if (go)
            {

                if (loadedGameObjectList.ContainsKey(go.name))
                {

                    // �ε��
                }
                else
                {

                    // �ε�
                    GameObject goInstance = Instantiate(go) as GameObject;
                    goInstance.name = go.name;
                    goInstance.transform.parent = gameObject.transform;
                    loadedGameObjectList.Add(go.name, goInstance);
                    // Debug.Load(string.Format("Loaded GameObject {0}", go.name));
                }
            }
        }
    }
}
