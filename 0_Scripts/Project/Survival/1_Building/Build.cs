using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{

    // �ǹ� ����!
    public GameObject prefabBuilding;

    public void SpawnBuilding()
    {

        Instantiate(prefabBuilding);
    }
}
