using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

// �ֱ������� �������� �÷��̾� ��ó�� �����ϴ� ��ũ��Ʈ
public class ItemSpawner : MonoBehaviourPun
{

    public GameObject[] items;          // ������ ������
    public Transform playerTransform;   // �÷��̾��� Ʈ������

    public float maxDistance = 5f;      // �÷��̾� ��ġ���� �������� ��ġ�� �ִ� �ݰ�

    public float timeBetSpawnMax = 7f;  // �ִ� �ð� ����
    public float timeBetSpawnMin = 2f;  // �ּ� �ð� ����
    private float timeBetSpawn;         // ���� ����

    private float lastSpawnTime;        // ������ ���� ����

    private void Start()
    {

        // ���� ���ݰ� ������ ���� ���� �ʱ�ȭ
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0;
    }

    // �ֱ������� ������ ���� ó�� ����
    private void Update()
    {
        
        // ȣ��Ʈ������ ������ ���� ���� ����
        if (!PhotonNetwork.IsMasterClient)
        {

            return;
        }

        // ���� ������ ������ ���� �������� ���� �ֱ� �̻� ����
        // && �÷��̾� ĳ���Ͱ� ������
        if (Time.time >= lastSpawnTime + timeBetSpawn && playerTransform != null)
        {

            // ������ ���� �ð� ����
            lastSpawnTime = Time.time;

            // ���� �ֱ⸦ �������� ����
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);

            // ������ ���� ����
            Spawn();
        }
    }

    // ���� ������ ���� ó��
    private void Spawn()
    {

        // �÷��̾� ��ó���� �׺�޽� ���� ���� ��ġ ��������
        Vector3 spawnPosition =
            GetRandomPointOnNavMesh(Vector3.zero, maxDistance);

        // �ٴڿ��� 0.5��ŭ ���� �ø���
        spawnPosition += Vector3.up * 0.5f;

        // ������ �� �ϳ��� �������� ��� ���� ��ġ�� ����
        GameObject selectedItem = items[Random.Range(0, items.Length)];
        GameObject item = PhotonNetwork.Instantiate(selectedItem.name, spawnPosition, Quaternion.identity);

        // ������ �������� 5�� �ڿ� �ı�
        StartCoroutine(DestroyAfter(item, 5f));
    }
    
    // ������ PhotonNetwork.Destroy()�� ���� �����ϴ� �ڷ�ƾ
    IEnumerator DestroyAfter(GameObject target, float delay)
    {

        // delay��ŭ ���
        yield return new WaitForSeconds(delay);

        // target�� �ı����� �ʾ����� �ı� ����
        if (target != null)
        {

            PhotonNetwork.Destroy(target);
        }
    }

    // �׺�޽� ���� ������ ��ġ�� ��ȯ�ϴ� �޼ҵ�
    // center�� �߽����� distance �ݰ� �ȿ����� ������ ��ġ�� ã��
    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)
    {

        // center�� �߽����� �������� maxDistance�� �� �ȿ����� ������ ��ġ�� �ϳ��� ����
        // Random.insideUnitSphere�� �������� 1�� �� �ȿ����� ������ �� ���� ��ȯ�ϴ� ������Ƽ
        Vector3 randomPos = Random.insideUnitSphere * distance + center;

        // ����޽� ���ø��� ��� ������ �����ϴ� ����
        NavMeshHit hit;

        // maxDistance �ݰ� �ȿ��� randomPos�� ���� ����� �׺�޽� ���� �� ���� ã��
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);

        // ã�� �� ��ȯ
        return hit.position;
    }
}
