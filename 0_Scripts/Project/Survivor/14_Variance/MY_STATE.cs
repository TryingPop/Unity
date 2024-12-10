using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State : 게임 중에 변화할 수 있다.
/// </summary>
public static class MY_STATE
{

    public enum GAMEOBJECT
    {

        CANCEL = -3,
        BUILDING_UNFINISHED = -2,
        DEAD = -1,

        NONE = 0,
        UNIT_MOVE = 1, BUILDING_ACTION1 = 1,
        UNIT_STOP = 2, BUILDING_ACTION2 = 2,
        UNIT_PATROL = 3, BUILDING_ACTION3 = 3,
        UNIT_HOLD = 4,
        UNIT_ATTACK = 5, UNIT_SKILL0 = 5,
        UNIT_SKILL1 = 6,
        UNIT_SKILL2 = 7,
        UNIT_SKILL3 = 8,

        MOUSE_R = VarianceManager.MOUSE_R,
    }

    public enum GAME
    {

        NONE = 0,

        WIN = 1,
        LOSE = 2,
    }

    public enum INPUT
    {

        CANCEL = -1,
        NONE = 0, KEY_M, KEY_S, KEY_P, KEY_H, KEY_A, KEY_Q, KEY_W, KEY_E,
    }
}
