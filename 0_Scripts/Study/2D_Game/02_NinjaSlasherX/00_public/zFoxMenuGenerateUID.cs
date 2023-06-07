using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class zFoxMenuGenerateUID : MonoBehaviour
{

    // File, Edit, Assets, GameObject 등 타이틀 바로 밑의 메뉴에 zFoxTools 항목 추가하고 하위로 UID 메뉴와 그 밑에 Generate 메뉴 추가
    [MenuItem("zFoxTools/UID/Generate")]

    public static void GenerateUID()
    {

        int guidIndex = 0;

        if (!EditorUtility.DisplayDialog(
            "UID Generate", "Generate UID?", "Ok", "Cancel"))   // 타이틀, 내용, ok 버튼, cancel 버튼
        {

            return;
        }

        Debug.Log("\n");
        Debug.Log("--- GenerateUID Begin ---");
        zFoxUID[] uidList = GameObject.Find("Stage").
            GetComponentsInChildren<zFoxUID>();

        foreach(zFoxUID uidItem in uidList)
        {

            if (uidItem.uid != null)
            {

                switch (uidItem.type) 
                {

                    case zFOXUID_TYPE.NUMBER:
                        uidItem.uid = guidIndex.ToString();
                        guidIndex++;
                        break;

                    case zFOXUID_TYPE.GUID:
                        uidItem.uid = System.Guid.NewGuid().ToString();
                        Debug.Log(string.Format("{0} {1} <- {2}", uidItem.name,
                            uidItem.transform.position, System.Guid.NewGuid()));
                        break;
                }

                EditorUtility.SetDirty(uidItem);
            }
        }

        Debug.Log("--- GenerateUID End ---");
        Debug.Log("\n");
    }

    [MenuItem("zFoxTools/UID/Delete")]
    public static void DeleteUID()
    {

        if (EditorUtility.DisplayDialog("UID Delete", "Delete UID?", "Ok", "Cancel"))
        {

            zFoxUID[] uidList = GameObject.Find("Stage").
                GetComponentsInChildren<zFoxUID>();
            foreach(zFoxUID uidItem in uidList)
            {

                uidItem.uid = "(non)";
                EditorUtility.SetDirty(uidItem);
            }
        }
    }

}
