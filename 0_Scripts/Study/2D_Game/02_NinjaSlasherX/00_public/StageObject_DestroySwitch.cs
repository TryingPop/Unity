using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject_DestroySwitch : MonoBehaviour
{

    public GameObject[] destroyObjectList;

    public void DestroyStageObject()
    {

        foreach(GameObject go in destroyObjectList)
        {

            Destroy(go);
        }

        Destroy(this.gameObject);
    }
}
