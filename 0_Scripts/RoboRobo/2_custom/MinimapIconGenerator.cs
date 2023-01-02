using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIconGenerator : MonoBehaviour
{

    public GameObject minimapPrefab;


    void Start()
    {
        Instantiate(minimapPrefab, transform.position + Vector3.up * 5f, Quaternion.identity, transform);
    }
}
