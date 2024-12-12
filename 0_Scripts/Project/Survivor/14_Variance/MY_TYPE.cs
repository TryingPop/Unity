using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TYPE : �ʱ� �����Ǹ� ������ �ʴ� ���� �ǹ��Ѵ�.
/// </summary>
public static class MY_TYPE
{

    /// <summary>
    /// ����, Ǯ��, ���� �ľǿ� ���δ�.
    /// </summary>
    public enum GAMEOBJECT
    {

        // ū Ÿ��
        NONE = 0,
        // 20 ~ 29����
        MULTI = 0b0111_1111_1111_0000_0000_0000_0000_0000,


        // 20 ���ʹ� ��Ʈ����ŷ i.e �ִ� ���� 100���� �����̴�.
        UNIT = 1 << 20,
        SUPPORT_UNIT = UNIT | (1 << 21),
        COMBAT_UNIT = UNIT | (1 << 22),
        SKILL_UNIT = UNIT | (1 << 23),
        ANIMAL = UNIT | (1 << 24),

        BUILDING = 1 << 25,
        UNFINISHED_BUILDING = BUILDING | (1 << 26),

        MISSILE = 1 << 28,

        // ����
        WORKER = SUPPORT_UNIT | 1,
        CHICKEN = ANIMAL | 1,

        BOSS_D = COMBAT_UNIT | SKILL_UNIT | 1,

        // �ǹ�
        FARM = BUILDING | 1,
        TOWN = BUILDING | 2,
        BLACKSMITH = BUILDING | 3,
        WALL = BUILDING | 4,
        WINDMILL = BUILDING | 5,
        GUILD = BUILDING | 6,

        ENEMYCASTLE = BUILDING | 11,
    }

    /// <summary>
    /// �ڿ� ����
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
    /// ���׷��̵�
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
    /// ũ��
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
    /// �̼�
    /// </summary>
    public enum MISSION
    {

        NONE = 0,

        // ���� - �ش� ������ �縳 �Ұ�
        MAIN = 0b_0000_0001,                     // ���� �̼� - �ó������� �ſ� ������ ����Ʈ
        SUB = 0b_0000_0010,                     // ���� �̼� - ���� �׸� �ȱ��� �׸��� �ó����� ����Ʈ
        HIDDEN = 0b_0000_0100,                     // ���� �̼� - ���� ���� �� �̼� ���縸 �������ְ� ������ ���� X - �ݺ� �ƴϸ� �ڵ� Remove
        EVENT = 0b_0000_1000,                     // �̺�Ʈ - ������ ������ ���� - �ݺ� �ƴϸ� �ڵ� Reomve

        // ���� �ɼ� 
        END = 0b_0001_0000,                     // ���� ����� �����Ǵ� ����Ʈ �¸� �̼� ������ ���� ���а� �����ȴ�!
        REMOVE = 0b_0010_0000,                     // �̼� �Ϸ� �� �̼� ������Ʈ���� ���� ����
        REPEAT = 0b_0100_0000,                     // �ش� �̼� �ݺ� ����
        WIN = 0b_1000_0000,                     // �¸� �̼�


        // ����
        MAIN_WIN = MAIN | WIN,                          // 0b_1000_0001, MAIN + WIN ������ �̷���� ��
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
