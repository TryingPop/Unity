using System.Collections.Generic;
using UnityEngine;


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

    public static readonly float REFUND_RATE = 0.6f;

    #endregion 기본 타입

    public static readonly WaitForSeconds BASE_WAITFORSECONDS = new WaitForSeconds(2f);
    public static readonly WaitForSeconds EFFECT_WAITFORSECONDS = new WaitForSeconds(0.3f);

    public static readonly Vector2 INIT_POS_SCRIPT = new Vector2(110f, -35f);
    public static readonly Vector2 INIT_SIZE_SCRIPT = new Vector2(100f, 40f);

    public static readonly RaycastHit[] hits = new RaycastHit[MAX_SELECT];
    public static readonly RaycastHit[] hit = new RaycastHit[1];
}