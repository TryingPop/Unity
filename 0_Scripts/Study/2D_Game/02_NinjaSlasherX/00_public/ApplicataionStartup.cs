using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicataionStartup : MonoBehaviour
{

    void Start()
    {

        Debug.Log("==== Application Startup [Ninja SlasherX] ====");
        SaveData.LoadOption();
    }
}
