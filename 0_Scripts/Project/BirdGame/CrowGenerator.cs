using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowGenerator : MonoBehaviour
{
    public GameObject nCrow; // 일반(Normal) 까마귀
    public GameObject rCrow; // 희기(Rare) 까마귀

    [Range(2, 4)]
    [SerializeField]
    private int MinNum = 2; // 초반 생성할 크로우 최소값
    [Range(4, 7)]
    [SerializeField]
    private int MaxNum = 4; // 초반 생성할 크로우 최대값

    private int _Num;

    private float[] _yPos = new float[10]; // 최대 10마리 생성할 예정

    [SerializeField]
    private float _spawntimeMin = 2f; // 스폰 최소 타임 
    [SerializeField]
    private float _spawntimeMax = 4f; // 스폰 최대 타임

    private float _spawnTime = 0f; // 스폰 시간
    private float _spawnTimer = 0f; // 스폰 타이머 


    private float _xPos = 5f; 
    private bool _isNormal = true;


    void Update()
    {
        if (!GameManager.instance.isGameover) // 게임오버가 아니면
        {
            _spawnTimer += Time.deltaTime; // 스폰 시간
            if (_spawnTimer >= _spawnTime) // 스폰하면
            {
                _spawnTimer = 0f; // 초기화
                _spawnTime = Random.Range(_spawntimeMin, _spawntimeMax); // 임의값 선택

                SetType();
                SetCrow(); // 크로우 좌표 배정
                MakeCrow(); // 크로 생성

                StartCoroutine("addScore"); // addScore 메서드 실행
            }
        }
    }

    private void SetCrow()
    {
        float yMin; // 맵을 _Num 등분 했을때 각 등분된 장소의 y좌표의 최소값
        float yMax; // 맵을 _Num 등분 했을때 각 등분된 장소의 y좌표의 최대값

        _Num = Random.Range(MinNum, MaxNum + 1); // 끝값은 포함 안되기에 +1

        for (int i = 0; i < _Num; i++) // 반복문을 이용해 계산
        {
            yMin = ((float)i) * (float)(12f / _Num) - 6f; 
            yMax = ((float)(i + 1)) * (12f / _Num) - 6f;
            _yPos[i] = Random.Range(yMin, yMax); // 구간의 최소값과 최대값 사이의 임의의 y변수
        }
    }

    private void MakeCrow() // 까마귀 생성
                            // 일반 까마귀와 레어 까마귀의 속도가 다르므로
                            // 같은 시간에 생성하기 위해 instatiate를 사용
    {
        if (_isNormal) // 앞에서 일반 까마귀가 선택된 경우
        {
            for (int i = 0; i < _Num; i++) // 반복문을 이용해 각 좌표에 맞는 까마귀 생성
            {
                Instantiate(nCrow, new Vector3(_xPos, _yPos[i], 0), Quaternion.identity); // nCrow 앞에서 구한 좌표에 생성
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

    IEnumerator addScore() // 게임오버가 아니면 점수 상승
                           // 박스 콜라이더를 이용해 onTriggerExit 로 점수를 할 수 있었지만
                           // 직접 플레이하며 다른 버그나 어색한 부분을 찾아보기 위해
                           // 대기 시간 값을 일일히 대입해가며 Coroutine으로 점수 주는 방법 선택
    {
        if (_isNormal) // 일반 까마귀면 3초
        {
            yield return new WaitForSeconds(3f); // 3 초간 대기
        }
        else // 희기 까마귀면 1.3초
        {
            yield return new WaitForSeconds(1.3f); // 1.3 초간 대기
        }
        // 게임 오버 상태 확인 후 게임오버가 아니면 점수 추가
        if (!GameManager.instance.isGameover) 
        {
            GameManager.instance.AddScore();
        }
        
        // 임의성으로 한 번에 최대 2단계 난이도 상승할 수 있게 Coroutine문안에 추가 
        if (GameManager.score != 0 && GameManager.score % GameManager.lvlup == 0) 
        {
            LvlUpdate(); // 최소 나오는 수와 최대 나오는 수가 각각 1씩 증가한다
                         // 최고 난이도까지 상승 시 최소 4마리와 최대 7마리 사이로 나오게 된다.
        }
    }

    private void SetType() // 일반 까마귀인지 희기 까마귀인지 판별
    {
        int num = 2 + (GameManager.score / 4); // 스코어 4점 단위로 희기 까마귀 출현 확률 상승
        _isNormal = (Random.Range(0, num) < 2 ? true : false); // 4점부터 33% 확률로 희기 까마귀 생성
                                                               // 8점은 50%, 12점은 66% 희기 까마귀 확률 서서히 증가
    }

    private void LvlUpdate() // 까마귀 나오는 갯수 증가 4 ~ 7
    {
        MinNum = Mathf.Min((GameManager.score / GameManager.lvlup) + MinNum, 4); // 난이도 최고 단계에서 최소 4마리
        MaxNum = Mathf.Min((GameManager.score / GameManager.lvlup) + MaxNum, 7); // 난이도 최고 단계에서 최대 7마리
    }
}
