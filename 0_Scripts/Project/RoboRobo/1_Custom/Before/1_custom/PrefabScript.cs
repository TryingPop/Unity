using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabScript : MonoBehaviour
{
    [Tooltip("ÆÄ±« ½Ã°£")]
    [SerializeField]
    private float destroyTime;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
