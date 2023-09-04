using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePrefabs : MonoBehaviour
{

    [SerializeField] private int prefabIdx;

    private void OnDisable()
    {

        PoolManager.instance.UsedPrefab(gameObject, prefabIdx);
    }
}
