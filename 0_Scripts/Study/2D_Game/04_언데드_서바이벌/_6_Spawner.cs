using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _6_Spawner : MonoBehaviour
{

    public Transform[] spawnPoint;
    public _7_spawnData[] spawnData;

    private float timer;
    public float spawnTime;

    private int level;
    private float levelTime;

    private void Awake()
    {

        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = _3_GameManager.instance.maxGameTime / spawnData.Length;
    }

    private void Update()
    {

        if (!_3_GameManager.instance.isLive) return;

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(_3_GameManager.instance.gameTime / levelTime), spawnData.Length - 1); 

        if (timer > spawnData[level].spawnTime)
        {

            timer = 0f;
            Spawn();
        }
    }

    private void Spawn()
    {

        GameObject enemy = _3_GameManager.instance.pool.Get(0);
        enemy.GetComponent<_4_Enemy>().Init(spawnData[level]);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;     // ��� ������Ʈ�� Transform�� �����ϰ� �ְ�,
                                                                                                // �ڱ��ڽŵ� ���Ƿ� 1����
    }
}