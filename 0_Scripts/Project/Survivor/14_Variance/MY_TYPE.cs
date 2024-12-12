using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TYPE : 초기 설정되면 변하지 않는 것을 의미한다.
/// </summary>
public static class MY_TYPE
{

    /// <summary>
    /// 종류, 풀링, 선택 파악에 쓰인다.
    /// </summary>
    public enum GAMEOBJECT
    {

        // 큰 타입
        NONE = 0,
        // 20 ~ 29까지
        MULTI = 0b0111_1111_1111_0000_0000_0000_0000_0000,


        // 20 부터는 비트마스킹 i.e 최대 종류 100만개 제한이다.
        UNIT = 1 << 20,
        SUPPORT_UNIT = UNIT | (1 << 21),
        COMBAT_UNIT = UNIT | (1 << 22),
        SKILL_UNIT = UNIT | (1 << 23),
        ANIMAL = UNIT | (1 << 24),

        BUILDING = 1 << 25,
        UNFINISHED_BUILDING = BUILDING | (1 << 26),

        MISSILE = 1 << 28,

        // 유닛
        WORKER = SUPPORT_UNIT | 1,
        CHICKEN = ANIMAL | 1,

        BOSS_D = COMBAT_UNIT | SKILL_UNIT | 1,

        // 건물
        FARM = BUILDING | 1,
        TOWN = BUILDING | 2,
        BLACKSMITH = BUILDING | 3,
        WALL = BUILDING | 4,
        WINDMILL = BUILDING | 5,
        GUILD = BUILDING | 6,

        ENEMYCASTLE = BUILDING | 11,
    }

    /// <summary>
    /// 자원 종류
    /// </summary>
    public enum RESOURCE
    {

        NONE = 0,

        GOLD = 1 << 20,
        SUPPLY = 1 << 21,

        TURN_GOLD = GOLD | 1,
        CUR_SUPPLY = SUPPLY | 1,
        MAX_SUPPLY = SUPPLY | 2,
    }


    /// <summary>
    /// 업그레이드
    /// </summary>
    public enum UPGRADE
    {

        NONE = 0,

        UNIT = 1 << 20,
        BUILDING = 1 << 25,
        RESOURCE = 1 << 30,

        HP = 1 << 10,
        DEF = 1 << 11,
        ATK = 1 << 12,

        GOLD = 1 << 10,
        SUPPLY = 1 << 11,

        UNIT_HP = UNIT | HP,
        UNIT_DEF = UNIT | DEF,
        UNIT_ATK = UNIT | ATK,

        BUILDING_HP = BUILDING | HP,
        BUILDING_DEF = BUILDING | DEF,

        ADD_TURN_GOLD = RESOURCE | GOLD | 1,
        ADD_KILL_GOLD = RESOURCE | GOLD | 2,

        ADD_MAX_SUPPLY = RESOURCE | SUPPLY | 1,
    }

    /// <summary>
    /// 크기
    /// </summary>
    public enum SIZE
    {

        NONE = 0,

        TINY = 1,
        SMALL = 2,
        STANDARD = 3,
        LARGE = 4,
        HUGE = 5,
        GIANT = 6,
    }

    /// <summary>
    /// UI?
    /// </summary>
    public enum UI
    {

        ALL = -1,
        NONE = 0,
        SLOT = 1,
        BTN = 2,
    }

    /// <summary>
    /// 미션
    /// </summary>
    public enum MISSION
    {

        NONE = 0,

        // 종류 - 해당 종류는 양립 불가
        MAIN = 0b_0000_0001,                     // 메인 미션 - 시나리오와 매우 연관된 퀘스트
        SUB = 0b_0000_0010,                     // 서브 미션 - 깨도 그만 안깨도 그만인 시나리오 퀘스트
        HIDDEN = 0b_0000_0100,                     // 히든 미션 - 성공 실패 시 미션 존재만 가르쳐주고 내용은 공개 X - 반복 아니면 자동 Remove
        EVENT = 0b_0000_1000,                     // 이벤트 - 완전히 숨겨진 조건 - 반복 아니면 자동 Reomve

        // 선택 옵션 
        END = 0b_0001_0000,                     // 게임 종료와 연관되는 퀘스트 승리 미션 종류에 따라 승패가 결정된다!
        REMOVE = 0b_0010_0000,                     // 미션 완료 후 미션 오브젝트에서 제거 유무
        REPEAT = 0b_0100_0000,                     // 해당 미션 반복 여부
        WIN = 0b_1000_0000,                     // 승리 미션


        // 조합
        MAIN_WIN = MAIN | WIN,                          // 0b_1000_0001, MAIN + WIN 합으로 이루어진 값
        MAIN_WIN_END = MAIN | WIN | END,                // 0b_1001_0001,
        MAIN_WIN_REMOVE = MAIN | WIN | REMOVE,          // 0b_1010_0001,
        MAIN_LOSE = MAIN,                               // 0b_0000_0001,
        MAIN_LOSE_END = MAIN | END,                     // 0b_0001_0001,
        MAIN_LOSE_REMOVE = MAIN | REMOVE,               // 0b_0010_0001,

        SUB_WIN = SUB | WIN,                            // 0b_1000_0010,
        SUB_WIN_END = SUB | WIN | END,                  // 0b_1001_0010,
        SUB_WIN_REMOVE = SUB | WIN | REMOVE,            // 0b_1010_0010,
        SUB_WIN_END_REMOVE = SUB | WIN | END | REMOVE,  // 0b_1011_0010,
        SUB_WIN_REPEAT = SUB | WIN | REPEAT,            // 0b_1100_0010,
        SUB_LOSE = SUB,                                 // 0b_0000_0010,
        SUB_LOSE_END = SUB | END,                       // 0b_0001_0010,
        SUB_LOSE_REMOVE = SUB | REMOVE,                 // 0b_0010_0010,
        SUB_LOSE_END_REMOVE = SUB | END | REMOVE,       // 0b_0011_0010,
        SUB_LOSE_REPEAT = SUB | REPEAT,                 // 0b_0100_0010,

        HIDDEN_WIN = HIDDEN | WIN | REMOVE,             // 0b_1010_0100,
        HIDDEN_WIN_END = HIDDEN | WIN | REMOVE | WIN,   // 0b_1011_0100,
        HIDDEN_WIN_REPEAT = HIDDEN | WIN | REPEAT,      // 0b_1100_0100,
        HIDDEN_LOSE = HIDDEN | REMOVE,                  // 0b_0010_0100,
        HIDDEN_LOSE_END = HIDDEN | REMOVE | END,        // 0b_0011_0100,
        HIDDEN_LOSE_REPEAT = HIDDEN | REPEAT,           // 0b_0100_0100,

        EVENT_WIN = EVENT | WIN | REMOVE,               // 0b_1010_1000,
        EVENT_WIN_END = EVENT | WIN | REMOVE | END,     // 0b_1011_1000,
        EVENT_WIN_REPEAT = EVENT | WIN | REPEAT,        // 0b_1100_1000,
        EVENT_LOSE = EVENT | REMOVE,                    // 0b_0010_1000,
        EVENT_LOSE_END = EVENT | REMOVE | END,          // 0b_0011_1000,
        EVENT_LOSE_REPEAT = EVENT | REPEAT,             // 0b_0100_1000,
    }
}
