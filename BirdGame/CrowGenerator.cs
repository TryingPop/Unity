using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowGenerator : MonoBehaviour
{
    public GameObject nCrow; // �Ϲ�(Normal) ���
    public GameObject rCrow; // ���(Rare) ���

    [Range(2, 4)]
    [SerializeField]
    private int MinNum = 2; // �ʹ� ������ ũ�ο� �ּҰ�
    [Range(4, 7)]
    [SerializeField]
    private int MaxNum = 4; // �ʹ� ������ ũ�ο� �ִ밪

    private int _Num;

    private float[] _yPos = new float[10]; // �ִ� 10���� ������ ����

    [SerializeField]
    private float _spawntimeMin = 2f; // ���� �ּ� Ÿ�� 
    [SerializeField]
    private float _spawntimeMax = 4f; // ���� �ִ� Ÿ��

    private float _spawnTime = 0f; // ���� �ð�
    private float _spawnTimer = 0f; // ���� Ÿ�̸� 


    private float _xPos = 5f; 
    private bool _isNormal = true;


    void Update()
    {
        if (!GameManager.instance.isGameover) // ���ӿ����� �ƴϸ�
        {
            _spawnTimer += Time.deltaTime; // ���� �ð�
            if (_spawnTimer >= _spawnTime) // �����ϸ�
            {
                _spawnTimer = 0f; // �ʱ�ȭ
                _spawnTime = Random.Range(_spawntimeMin, _spawntimeMax); // ���ǰ� ����

                SetType();
                SetCrow(); // ũ�ο� ��ǥ ����
                MakeCrow(); // ũ�� ����

                StartCoroutine("addScore"); // addScore �޼��� ����
            }
        }
    }

    private void SetCrow()
    {
        float yMin; // ���� _Num ��� ������ �� ��е� ����� y��ǥ�� �ּҰ�
        float yMax; // ���� _Num ��� ������ �� ��е� ����� y��ǥ�� �ִ밪

        _Num = Random.Range(MinNum, MaxNum + 1); // ������ ���� �ȵǱ⿡ +1

        for (int i = 0; i < _Num; i++) // �ݺ����� �̿��� ���
        {
            yMin = ((float)i) * (float)(12f / _Num) - 6f; 
            yMax = ((float)(i + 1)) * (12f / _Num) - 6f;
            _yPos[i] = Random.Range(yMin, yMax); // ������ �ּҰ��� �ִ밪 ������ ������ y����
        }
    }

    private void MakeCrow() // ��� ����
                            // �Ϲ� ��Ϳ� ���� ����� �ӵ��� �ٸ��Ƿ�
                            // ���� �ð��� �����ϱ� ���� instatiate�� ���
    {
        if (_isNormal) // �տ��� �Ϲ� ��Ͱ� ���õ� ���
        {
            for (int i = 0; i < _Num; i++) // �ݺ����� �̿��� �� ��ǥ�� �´� ��� ����
            {
                Instantiate(nCrow, new Vector3(_xPos, _yPos[i], 0), Quaternion.identity); // nCrow �տ��� ���� ��ǥ�� ����
            }
        }
        else
        {
            for (int i = 0; i < _Num; i++)
            {
                Instantiate(rCrow, new Vector3(_xPos, _yPos[i], 0), Quaternion.identity);
            }
        }
    }

    IEnumerator addScore() // ���ӿ����� �ƴϸ� ���� ���
                           // �ڽ� �ݶ��̴��� �̿��� onTriggerExit �� ������ �� �� �־�����
                           // ���� �÷����ϸ� �ٸ� ���׳� ����� �κ��� ã�ƺ��� ����
                           // ��� �ð� ���� ������ �����ذ��� Coroutine���� ���� �ִ� ��� ����
    {
        if (_isNormal) // �Ϲ� ��͸� 3��
        {
            yield return new WaitForSeconds(3f); // 3 �ʰ� ���
        }
        else // ��� ��͸� 1.3��
        {
            yield return new WaitForSeconds(1.3f); // 1.3 �ʰ� ���
        }
        // ���� ���� ���� Ȯ�� �� ���ӿ����� �ƴϸ� ���� �߰�
        if (!GameManager.instance.isGameover) 
        {
            GameManager.instance.AddScore();
        }
        
        // ���Ǽ����� �� ���� �ִ� 2�ܰ� ���̵� ����� �� �ְ� Coroutine���ȿ� �߰� 
        if (GameManager.score != 0 && GameManager.score % GameManager.lvlup == 0) 
        {
            LvlUpdate(); // �ּ� ������ ���� �ִ� ������ ���� ���� 1�� �����Ѵ�
                         // �ְ� ���̵����� ��� �� �ּ� 4������ �ִ� 7���� ���̷� ������ �ȴ�.
        }
    }

    private void SetType() // �Ϲ� ������� ��� ������� �Ǻ�
    {
        int num = 2 + (GameManager.score / 4); // ���ھ� 4�� ������ ��� ��� ���� Ȯ�� ���
        _isNormal = (Random.Range(0, num) < 2 ? true : false); // 4������ 33% Ȯ���� ��� ��� ����
                                                               // 8���� 50%, 12���� 66% ��� ��� Ȯ�� ������ ����
    }

    private void LvlUpdate() // ��� ������ ���� ���� 4 ~ 7
    {
        MinNum = Mathf.Min((GameManager.score / GameManager.lvlup) + MinNum, 4); // ���̵� �ְ� �ܰ迡�� �ּ� 4����
        MaxNum = Mathf.Min((GameManager.score / GameManager.lvlup) + MaxNum, 7); // ���̵� �ְ� �ܰ迡�� �ִ� 7����
    }
}
