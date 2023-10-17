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

    FARM = 301,
    TOWN = 302,
    ENEMYCASTLE = 351,
}

/// <summary>
/// 버튼 옵션
/// </summary>
public enum TYPE_BUTTON_OPTION
{

    CANCEL = -2,
    NULL = -1,
    NONE = 0,
    NEED_POS = 1,
    NEED_TARGET = 2,
    NEED_TARGET_OR_POS = 4,

    BUILD = VariableManager.BUILD,
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

    MOUSE_R = VariableManager.MOUSE_R,          // 건물이랑, 유닛 읽는게 다르다!
    BUILDING_CANCEL = 100,                      // 취소
}

/// <summary>
/// 업그레이드 번호
/// </summary>
public enum TYPE_MANAGEMENT
{

    UP_ATK = 1,
    UP_DEF = 2,
    UP_HP = 3,
    
    GOLD = 101,
    SUPPLY = 102,
}

public enum TYPE_KEY
{

    NONE = 0, M, S, P, H, A, Q, W, E,
    MOUSE_R = VariableManager.MOUSE_R,
}

public enum STATE_GAME
{

    NONE = 0,

    LOSE = 1,
    WIN = 2,
}

/// <summary>
/// static 변수 보관소
/// </summary>
public class VariableManager
{

    // 레이어
    public static readonly int LAYER_DEAD = 13;
    public static readonly int LAYER_BULLET = 14;
    public static readonly int LAYER_PLAYER = 17;
    public static readonly int LAYER_ENEMY = 18;
    public static readonly int LAYER_NEUTRAL = 19;
    public static readonly int LAYER_GROUND = 10;

    // 팀 idx
    public static readonly int TEAM_PLAYER = 0;
    public static readonly int TEAM_ENEMY = 1;
    public static readonly int TEAM_NEUTRAL = 2;

    // 마우스
    public const int MAX_ACTIONS = 10;
    public const int BUILD = 20;
    public const int MOUSE_R = 30;

    // 제한
    public static readonly int INFINITE = -100;
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

    public static readonly int ONE_MISS_PER_N_TIMES = 5;    // 몇 번 해야지 미스 발생하는지?

    public static readonly int INIT_UNIT_LIST_NUM = 50;
    public static readonly int INIT_BUILDING_LIST_NUM = 20;
    public static readonly int INIT_MISSILE_LIST_NUM = 50;
    public static readonly int INIT_LATE_POS = 50;
    public static readonly int INIT_NEUTRAL_LIST_NUM = 20;

    public static readonly int TYPE_SELECTABLE_INTERVAL = 100;

}