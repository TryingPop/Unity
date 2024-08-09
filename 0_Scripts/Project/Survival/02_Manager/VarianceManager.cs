using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Selectable 종류
/// </summary>
public enum TYPE_SELECTABLE 
{ 
    
    UNFINISHED_BUILDING = -2,

    NONE = 0, 
    
    NONCOMBAT = 1,
    UNIT = 2, 

    BUILDING = 3,

    WORKER = 101,
    CHICKEN = 102,

    BOSS_D = 201,

    ENEMYCASTLE = 351,

    FARM = 301,
    TOWN = 302,
    BLACKSMITH = 303,
    WALL = 304,
    WINDMILL = 305,
    GUILD = 306,
}

/// <summary>
/// 사이즈
/// </summary>
public enum TYPE_SIZE 
{ 
    
    TINY = 1,
    SMALL = 2, 
    STANDARD = 3, 
    LARGE = 4, 
    HUGE = 5,
    GIANT = 6,
}

/// <summary>
/// -1 ~ 5 번까지는 일반 유닛이 갖는 번호
/// 6번부터는 특수!
/// </summary>
public enum STATE_SELECTABLE
{

    BUILDING_UNFINISHED = -2,
    DEAD = -1,

    NONE = 0, 
    UNIT_MOVE = 1, BUILDING_ACTION1 = 1,
    UNIT_STOP = 2, BUILDING_ACTION2 = 2,
    UNIT_PATROL = 3, BUILDING_ACTION3 = 3,
    UNIT_HOLD = 4, 
    UNIT_ATTACK = 5, UNIT_REPAIR = 5, UNIT_HEAL = 5, UNIT_SKILL0 = 5, 
    UNIT_SKILL1 = 6, 
    UNIT_SKILL2 = 7, 
    UNIT_SKILL3 = 8,

    MOUSE_R = VarianceManager.MOUSE_R,          // 건물이랑, 유닛 읽는게 다르다!
    BUILDING_CANCEL = 100,                      // 취소
}

/// <summary>
/// 업그레이드 번호
/// </summary>
public enum TYPE_MANAGEMENT
{

    UP_UNIT_ATK = 1,
    UP_UNIT_DEF,
    UP_UNIT_HP,

    UP_BUILDING_DEF,
    UP_BUILDING_HP,

    UP_SUPPLY = 11,      // 최대 인구! 제한은 못뚫는다!
    UP_TURN_GOLD = 12,

    TURN_GOLD = 101,
    CUR_SUPPLY = 102,   // 게임 상에서 사용
    MAX_SUPPLY = 103,   // 게임 상에서 사용
}

public enum TYPE_INPUT
{

    CANCEL = -1,
    NONE = 0, KEY_M, KEY_S, KEY_P, KEY_H, KEY_A, KEY_Q, KEY_W, KEY_E,
}

public enum STATE_GAME
{

    NONE = 0,

    LOSE = 1,
    WIN = 2,
}

public enum TYPE_INFO
{

    ALL = -1,
    NONE = 0,
    SLOT = 1,
    BTN = 2,
}

public enum TYPE_MISSION 
{ 

    NONE = 0,

    // 종류 - 해당 종류는 양립 불가
    MAIN                    = 0b_0000_0001,                     // 메인 미션 - 시나리오와 매우 연관된 퀘스트
    SUB                     = 0b_0000_0010,                     // 서브 미션 - 깨도 그만 안깨도 그만인 시나리오 퀘스트
    HIDDEN                  = 0b_0000_0100,                     // 히든 미션 - 성공 실패 시 미션 존재만 가르쳐주고 내용은 공개 X - 반복 아니면 자동 Remove
    EVENT                   = 0b_0000_1000,                     // 이벤트 - 완전히 숨겨진 조건 - 반복 아니면 자동 Reomve
    
    // 선택 옵션 
    END                     = 0b_0001_0000,                     // 게임 종료와 연관되는 퀘스트 승리 미션 종류에 따라 승패가 결정된다!
    REMOVE                  = 0b_0010_0000,                     // 미션 완료 후 미션 오브젝트에서 제거 유무
    REPEAT                  = 0b_0100_0000,                     // 해당 미션 반복 여부
    WIN                     = 0b_1000_0000,                     // 승리 미션


    // 조합
    MAIN_WIN                = 0b_1000_0001,                     // MAIN + WIN 합으로 이루어진 값
    MAIN_WIN_END            = 0b_1001_0001,
    MAIN_WIN_REMOVE         = 0b_1010_0001,
    MAIN_LOSE               = 0b_0000_0001,
    MAIN_LOSE_END           = 0b_0001_0001,
    MAIN_LOSE_REMOVE        = 0b_0010_0001,

    SUB_WIN                 = 0b_1000_0010,
    SUB_WIN_END             = 0b_1001_0010,
    SUB_WIN_REMOVE          = 0b_1010_0010,
    SUB_WIN_END_REMOVE      = 0b_1011_0010,
    SUB_WIN_REPEAT          = 0b_1100_0010,
    SUB_LOSE                = 0b_0000_0010,
    SUB_LOSE_END            = 0b_0001_0010,
    SUB_LOSE_REMOVE         = 0b_0010_0010,
    SUB_LOSE_END_REMOVE     = 0b_0011_0010,
    SUB_LOSE_REPEAT         = 0b_0100_0010,

    HIDDEN_WIN              = 0b_1010_0100,                     
    HIDDEN_WIN_END          = 0b_1011_0100,
    HIDDEN_WIN_REPEAT       = 0b_1100_0100,
    HIDDEN_LOSE             = 0b_0010_0100,
    HIDDEN_LOSE_END         = 0b_0011_0100,
    HIDDEN_LOSE_REPEAT      = 0b_0100_0100,

    EVENT_WIN               = 0b_1010_1000,
    EVENT_WIN_END           = 0b_1011_1000,
    EVENT_WIN_REPEAT        = 0b_1100_1000,
    EVENT_LOSE              = 0b_0010_1000,
    EVENT_LOSE_END          = 0b_0011_1000,
    EVENT_LOSE_REPEAT       = 0b_0100_1000,
}


/// <summary>
/// static 변수 보관소
/// </summary>
public class VarianceManager
{

    #region 기본 타입
    // 레이어
    public static readonly int LAYER_DEAD = 13;
    public static readonly int LAYER_BULLET = 14;
    public static readonly int LAYER_PLAYER = 17;
    public static readonly int LAYER_ENEMY = 18;
    public static readonly int LAYER_NEUTRAL = 19;
    public static readonly int LAYER_ALLY = 20;
    public static readonly int LAYER_GROUND = 10;

    // 팀 idx
    public static readonly int TEAM_PLAYER = 0;
    public static readonly int TEAM_ENEMY = 1;
    public static readonly int TEAM_NEUTRAL = 2;
    public static readonly int TEAM_ALLY = 3;

    // 마우스
    public const int MAX_ACTIONS = 10;
    public const int BUILD = 20;
    public const int MOUSE_R = 30;

    // 제한
    public static readonly int INFINITE = -100;                 // 무적
    public static readonly short POOLMANAGER_NOTEXIST = -1;
    public static readonly int MAX_SELECT = 30;             // 최대 선택가능 수

    public static readonly int MAX_SUPPLY = 200;
    public static readonly int MAX_GOLD = 99_999_999;

    public static readonly int MAX_CONTROL_UNITS = 200;
    public static readonly int MAX_BUILDINGS = 100;
    public static readonly int MAX_ENEMY_UNITS = 50;

    public static readonly int MAX_SAVE_COMMANDS = 50;
    public static readonly int MAX_RESERVE_COMMANDS = 5;

    public static readonly int MAX_USE_BUTTONS = 8;
    public static readonly int MAX_SUB_BUTTONS = 7;
    public static readonly int MAX_KEYS = 8;                // M, S, H, P, A, Q, W, E 더 추가되면 값을 늘린다!

    public static readonly int MAX_BUILD_BUILDINGS = 3;

    public static readonly int MAX_MISSIONS = 2;

    public static readonly int MIN_DAMAGE = 1;

    public static readonly int INIT_UNIT_LIST_NUM = 50;
    public static readonly int INIT_BUILDING_LIST_NUM = 20;
    public static readonly int INIT_MISSILE_LIST_NUM = 50;
    public static readonly int INIT_LATE_POS = 50;
    public static readonly int INIT_NEUTRAL_LIST_NUM = 20;
    public static readonly int INIT_ALLY_LIST_NUM = 10;

    public static readonly int TYPE_SELECTABLE_INTERVAL = 100;

    public static readonly float REFUND_RATE = 0.6f;

    #endregion 기본 타입

    public static readonly WaitForSeconds BASE_WAITFORSECONDS = new WaitForSeconds(2f);
    public static readonly WaitForSeconds EFFECT_WAITFORSECONDS = new WaitForSeconds(0.3f);

    public static Vector2 INIT_POS_SCRIPT = new Vector2(110f, -35f);
    public static Vector2 INIT_SIZE_SCRIPT = new Vector2(100f, 40f);

    public static RaycastHit[] hits = new RaycastHit[MAX_SELECT];
    public static RaycastHit[] hit = new RaycastHit[1];
}