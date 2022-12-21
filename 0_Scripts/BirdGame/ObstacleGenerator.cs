using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    public GameObject obstacle; // ��ֹ� ������
    
    [SerializeField]
    private float _yMin = -2.5f; // y �ּҰ�
    [SerializeField]
    private float _yMax = 4f; // y �ִ밪

    [SerializeField]
    private float _spawntimeMin = 2f; // ���� �ּ� Ÿ�� 
    [SerializeField]
    private float _spawntimeMax = 4f; // ���� �ִ� Ÿ��

    private float _spawnTime = 0f; // ���� �ð�
    private float _spawnTimer = 0f; // ���� Ÿ�̸� 


    private float xPos = 5f;

    void Update()
    {
        if (!GameManager.instance.isGameover) // ���ӿ����� �ƴϸ�
        {
            _spawnTimer += Time.deltaTime; // ���� �ð�
            if (_spawnTimer >= _spawnTime) // �����ϸ�
            {
                _spawnTimer = 0f; // �ʱ�ȭ
                _spawnTime = Random.Range(_spawntimeMin, _spawntimeMax); // ���ǰ� ����

                float yPos = Random.Range(_yMin, _yMax); // ��ֹ� �߾� ��ġ�� ����

                Instantiate(obstacle, new Vector3(xPos, yPos, 0), Quaternion.identity); // ������ ����

                StartCoroutine("addScore"); // addScore �޼��� ����
            }
        }
    }

    IEnumerator addScore() // 3�� ��� �� ���ӿ����� �ƴϸ� ���� ���
    {
        yield return new WaitForSeconds(3.5f); // 3�ʰ� ���
        // ���� ���� ���� Ȯ�� �� ���ӿ����� �ƴϸ� ���� �߰�
        if (!GameManager.instance.isGameover) 
        {
            GameManager.instance.AddScore();
        }
    }
}
