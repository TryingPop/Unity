using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleNode : Node
{

    /// <summary>
    /// FSM으로 설정
    /// 상태이름과 발동에 필요한 턴 수 이다
    /// 현재 상태 체크를 0.3초마다 하기에 0.3 x 턴수 = 시간이 된다
    /// </summary>
    public enum STATE
    {

        None = 1,   
        Heal = 8,       // 체력 회복
        Summon = 20,    // 소환
        Idle = 10,      // 대기
        Wander = -1,    // 순찰
    }

    public STATE state;             // 현재 상태

    private BTBoss ai;      

    private Vector3 destination;    // 목적지

    private byte actCnt;            // 행동 세는 카운트 0 이면
                                    // 정해진 행동 없다
    private bool smBool = true;     // 소환 상태 처음 들어온지 확인

    // 생성자
    public IdleNode(BTBoss ai)
    {

        this.ai = ai;
    }

    public override NodeState Evaluate()
    {

        Action();
        return NodeState.SUCCESS;
    }

    /// <summary>
    /// 행동 선택 및 행동 실시
    /// </summary>
    private void Action()
    {

        // idle 진입했으므로 현재 idle 불 true
        ai.nowIdleBool = true;

        // 다른 상태에서 idle로 진입했는지 확인
        if (ai.ChkIdle()) actCnt = 0;

        // 행동 설정
        SetAction();

        // 행동 시행
        ChkAction();
    }

    /// <summary>
    /// 행동 설정
    /// 체력 회복 > 대기 / 소환 / 순찰 순이다
    /// 소환 가능한 상태인 경우,
    /// 대기 확률은 40%, 소환 확률 20%, 순찰 확률 40%이다
    /// 소환 불가능 상태이면 소환확률은 순찰 확률로 바뀐다
    /// </summary>
    private void SetAction()
    {

        // 행동 상태에 진입한 경우이므로 바로 탈출
        if (actCnt != 0) return;

        // 걷는 모션 중지
        ai.WalkAnim(false);

        // 체력 회복상태이면 
        if (ai.ChkHeal()) { state = STATE.Heal; return; };

        // 대기, 소환, 순찰 행동 설정
        int num = Random.Range(0, 10);

        if (num < 4)
        {

            state = STATE.Idle;
        }
        // 소환 가능 상태 확인
        else if (num < 6 && objPooling.instance.ChkSummon())
        {

            state = STATE.Summon;
        }
        else
        {

            state = STATE.Wander;
        }
        return;
    }

    /// <summary>
    /// 행동 실행
    /// </summary>
    private void ChkAction()
    {

        // 정해진 상태에 따라 행동 실행
        if (state == STATE.Heal)
        {

            Heal();
        }
        else if (state == STATE.Wander)
        {

            Wander();
        }
        else if (state == STATE.Summon)
        {

            SummonRobo();
        }
        else if (state == STATE.Idle)
        {

            Idle();
        }
        
        return;
    }

    /// <summary>
    /// 로보 생성
    /// </summary>
    private void SummonRobo()
    {
        
        actCnt++;

        // 피격이면 accCnt 0으로 행동 재설정
        if (ai.damagedBool) 
        {

            actCnt = 0; 
            return; 
        }

        // 행동 턴 수 다차서 행동 시작
        if (actCnt >= (byte)state)
        {

            // 실행 전 행동 초기화
            actCnt = 0;

            // 소환물 있는지 확인
            if (ai.summoners.Length > 0)
            {
                // 소환
                int num = Random.Range(0, ai.summoners.Length);

                do
                {
                    // 소환 장소 설정
                    destination = SetDestination(5f);
                }
                // 소환 범위가 유니티 크기로 2 ~ 5 범위 안에 있는지 체크
                while (Vector3.Distance(ai.transform.position, destination) <= 2f);
                {
                    // 없는 경우이므로 다시 설정
                    destination = SetDestination(5f);
                }

                // 처음 소환하는 경우
                if (smBool)
                {

                    // 재진입 방지
                    smBool = false;

                    // 프리팹들 정보 전달
                    objPooling.instance.SetPrefabs(ai.summoners);
                }

                // 설정된 summoners 종류 선택
                int idx = Random.Range(0, ai.summoners.Length);

                // 오브젝트 생ㅇ성
                GameObject obj = objPooling.instance.CreateObj(idx);

                // 생성된 오브젝트 목적지로 이동
                obj.transform.position = destination;
            }

        }
    }

    /// <summary>
    /// 주변 정찰
    /// </summary>
    private void Wander()
    {

        // 처음 들어오는 경우면 목적지 설정
        if (actCnt == 0) 
        {

            // 목적지 설정
            destination = SetDestination(8f); 
            
            // 무한대 있는지 확인 있으면 목적지는 현재지역으로 대기 상태랑 동일
            if (destination == Vector3.positiveInfinity)
            {
                
                destination = ai.transform.position;
            }
            else
            {

                // 캐릭터 중점이 발바닥에 있어서 위로 0.5 올려야 이동 가능
                destination += Vector3.up * 0.5f;
            }

            // 걷는 상태 설정
            ai.WalkAnim(true);
            ai.agent.destination = destination;
            actCnt++;
        };

        // 목적지와 거리 1 이하면 멈춘다
        if (Vector3.Distance(ai.transform.position, destination) <= 1f) 
        {

            ai.WalkAnim(false);
            actCnt = 0;
        }
    }

    /// <summary>
    /// 목적지 설정
    /// </summary>
    /// <returns>목적지 거리</returns>
    private Vector3 SetDestination(float distance)
    {
        
        NavMeshHit hit;
        // 거리안에 좌표 설정
        // 거리 8 이내의 이동 가능한 좌표에 랜덤으로 선택한다
        // 원리는 pdf 참고
        NavMesh.SamplePosition(ai.transform.position + Random.insideUnitSphere * distance, out hit, distance, 1) ;
        
        return hit.position;
    }

    /// <summary>
    /// 체력 회복
    /// </summary>
    private void Heal()
    {

        // 피격되면 중단
        if (ai.damagedBool)
        {

            actCnt = 0;
            return;
        }

        actCnt++;
        
        // 힐
        if (actCnt >= (byte)state)
        {

            ai.NowHp += 10;
            actCnt = 0;
        }
    }

    /// <summary>
    /// 대기 
    /// </summary>
    private void Idle()
    {

        actCnt++;

        if (actCnt >= (byte)state)
        {

            actCnt = 0;
        }
    }
}
