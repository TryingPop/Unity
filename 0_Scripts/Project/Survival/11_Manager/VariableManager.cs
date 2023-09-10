/// <summary>
/// static 변수 보관소
/// </summary>
public class VariableManager
{

    // 레이어
    public static readonly int LAYER_DEAD = 13;
    public static readonly int LAYER_BULLET = 14;
    public static readonly int LAYER_PLAYER = 17;
    public static readonly int LAYER_ENEMY = 24;


    // 마우스
    public const int MOUSE_L = 10;
    public const int MOUSE_R = 20;
    public const int BUILD = 30;


    // 제한
    public static readonly int INFINITE = -100;
    public static readonly int POOLMANAGER_NOTEXIST = -1;
    public static readonly int MAX_SELECT = 30;

    public static readonly int MAX_POPULATION = 200;
    public static readonly int MAX_GOLD = 999_999_999;

    public static readonly int MAX_CONTROL_UNITS = 200;
    public static readonly int MAX_COMMANDS = 80;
    public static readonly int MAX_RESERVE_COMMANDS = 5;
}
