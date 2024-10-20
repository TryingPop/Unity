using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Craft
{

    public string craftName;                        // 이름
    public GameObject go_Prefab;                    // 실제 설치될 프리팹
    public GameObject go_PreviewPrefab;             // 미리보기 프리팹
}

public class CraftManual : MonoBehaviour
{

    [SerializeField] private GameObject go_BaseUI;

    [SerializeField] private Craft[] craft_tap;     // 

    private GameObject go_Preview;                  // 미리보기 프리팹을 담을 변수
    private GameObject go_Prefab;                   // 실제 생성될 프리팹을 담을 변수

    private bool isActivated = false;
    private bool isPreviewActivated = false;

    [SerializeField] private Transform tf_Player;

    // Raycast 필요 변수 선언
    private RaycastHit hitInfo;
    
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
        {

            Window();
        }
        
        if (isPreviewActivated)
        {

            PreviewPositionUpdate();
        }


        if (Input.GetButtonDown("Fire1"))
        {

            Build();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            Cancel();
        }
    }

    private void Window()
    {

        if (!isActivated)
        {

            OpenWindow();
        }
        else
        {

            CloseWindow();
        }
    }

    private void OpenWindow()
    {

        isActivated = true;
        go_BaseUI.SetActive(true);
    }

    private void CloseWindow()
    {

        isActivated = false;
        go_BaseUI.SetActive(false);
    }

    public void SlotClick(int _slotNumber)
    {

        go_Preview = Instantiate(craft_tap[_slotNumber].go_PreviewPrefab, tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_tap[_slotNumber].go_Prefab;
        isPreviewActivated = true;
        go_BaseUI.SetActive(false);

    }

    private void Cancel()
    {

        if (isPreviewActivated)
        {

            Destroy(go_Preview);
        }

        isPreviewActivated = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;

        go_BaseUI.SetActive(false);
    }

    private void PreviewPositionUpdate()
    {

        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {

            if (hitInfo.transform != null)
            {

                Vector3 _location = hitInfo.point;
                go_Preview.transform.position = _location;
            }
        }
    }

    private void Build()
    {

        if (isPreviewActivated && go_Preview.GetComponent<PreviewObject>().IsBuildable())
        {

            Instantiate(go_Prefab, hitInfo.point, Quaternion.identity);
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
        }
    }
}
