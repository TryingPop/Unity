using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _6_Spawner : MonoBehaviour
{

    public Transform[] spawnPoint;

    private float timer;
    public float spawnTime;

    private void Awake()
    {

        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {

        timer += Time.deltaTime;

        if (timer > spawnTime)
        {

            timer = 0f;
            Spawn();
        }
    }

    private void Spawn()
    {

        GameObject enemy = _3_GameManager.instance.pool.Get(Random.Range(0, 2));
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;     // 모든 오브젝트는 Transform을 보유하고 있고,
                                                                                                // 자기자신도 담기므로 1부터
    }
}
