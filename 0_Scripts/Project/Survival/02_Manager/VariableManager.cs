

public enum TYPE_SELECTABLE 
{ 
    
    NONE = 0, 
    UNIT = 1, 
    UNIT_WORKER = 2,
    UNIT_BOSS = 8,
    BUILDING = 10,
}

/// <summary>
/// 버튼 옵션
/// </summary>
public enum TYPE_BUTTON_OPTION
{

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
public enum STATE_UNIT
{
    DEAD = -1, NONE = 0, MOVE = 1, STOP = 2, PATROL = 3, HOLD = 4, ATTACK = 5, REPAIR = 5, HEAL = 5,
    SKILL0 = 5, SKILL1 = 6, SKILL2 = 7, SKILL3 = 8
}

public enum TYPE_UPGRADE
{

    ATK = 1,
    DEF = 2,
    HP = 3,
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
    public static readonly int MAX_SELECT = 30;

    public static readonly int MAX_POPULATION = 200;
    public static readonly int MAX_GOLD = 99_999_999;

    public static readonly int MAX_CONTROL_UNITS = 200;
    public static readonly int MAX_BUILDINGS = 100;

    public static readonly int MAX_SAVE_COMMANDS = 50;
    public static readonly int MAX_RESERVE_COMMANDS = 5;

    public static readonly int MAX_BUTTONS = 8;

    public static readonly int MAX_BUILD_BUILDINGS = 3;

    public static readonly int MIN_DAMAGE = 1;
}
