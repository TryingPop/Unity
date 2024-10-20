using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zFoxGameObjectLoader : MonoBehaviour
{

    // 외부 파라미터 (Inspector 표시)
    public GameObject[] LoadGameObjectList_Awake;
    public GameObject[] LoadGameObjectList_Start;
    public GameObject[] LoadGameObjectList_Update;
    public GameObject[] LoadGameObjectList_FixedUpdate;

    // 외부 파라미터
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

    // 내부 파라미터
    bool loaded = false;

    // 코드 (MonoBehaviour 기본 기능 구현)
    void Awake()
    {

        // 각 게임 오브젝트가 로드됐는지 검사
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

        // Awake 처리 실행
        if (loaded_Awake)
        {

            loaded_Awake = true;
            LoadGameObject(LoadGameObjectList_Awake, loadedGameObjectList_Awake);
        }
    }

    void Start()
    {
        
        // Start 처리 실행
        if (!loaded_Start)
        {

            loaded_Start = true;
            LoadGameObject(LoadGameObjectList_Start, loadedGameObjectList_Start);
        }
    }

    void Update()
    {
        
        // Update 처리 실행
        if (!loaded_Update)
        {

            loaded_Update = true;
            LoadGameObject(LoadGameObjectList_Update, loadedGameObjectList_Update);
        }
    }


    void FixedUpdate()
    {
        
        // FixedUpdate 처리 실행
        if (!loaded_FixedUpdate)
        {

            loaded_FixedUpdate = true;
            LoadGameObject(LoadGameObjectList_FixedUpdate, loadedGameObjectList_FixedUpdate);
        }
    }


    void LoadGameObject(GameObject[] loadGameObjectList, 
        Dictionary<string, GameObject> loadedGameObjectList)
    {

        // 로더가 씬 전환 때 삭제되지 않도록 설정
        // 로드할 게임 오브젝트는 자식에게 설정되므로 로드된 것도 삭제되지 않는다
        DontDestroyOnLoad(this);

        // 등록되어 있는 게임 오브젝트를 불러온다
        foreach(GameObject go in loadGameObjectList)
        {

            if (go)
            {

                if (loadedGameObjectList.ContainsKey(go.name))
                {

                    // 로드됨
                }
                else
                {

                    // 로드
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
