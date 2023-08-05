using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{

    // °Ç¹° Áþ±â!
    public GameObject prefabBuilding;

    public void SpawnBuilding()
    {

        Instantiate(prefabBuilding);
    }
}
