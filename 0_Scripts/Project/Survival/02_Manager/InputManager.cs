using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    private TYPE_INPUT myState;
    public TYPE_INPUT MyState
    {

        get 
        {

            // 정보 넘기고 초기화
            var state = myState;
            myState = TYPE_INPUT.NONE;
            return state; 
        }
    }

    public int SetKey { set { myState = (TYPE_INPUT)value; } }
    private void Update()
    {
        
        if (myState == TYPE_INPUT.NONE)
        {

            // 아무상태도 아닐 때만 키입력이 가능하다!
            if (Input.GetKeyDown(KeyCode.M)) myState = TYPE_INPUT.KEY_M;
            else if (Input.GetKeyDown(KeyCode.S)) myState = TYPE_INPUT.KEY_S;
            else if (Input.GetKeyDown(KeyCode.P)) myState = TYPE_INPUT.KEY_P;
            else if (Input.GetKeyDown(KeyCode.H)) myState = TYPE_INPUT.KEY_H;
            else if (Input.GetKeyDown(KeyCode.A)) myState = TYPE_INPUT.KEY_A;
            else if (Input.GetKeyDown(KeyCode.Q)) myState = TYPE_INPUT.KEY_Q;
            else if (Input.GetKeyDown(KeyCode.W)) myState = TYPE_INPUT.KEY_W;
            else if (Input.GetKeyDown(KeyCode.E)) myState = TYPE_INPUT.KEY_E;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) myState = TYPE_INPUT.CANCEL;

        // 히트바는 바로 끄고 켠다!
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {

            UIManager.instance.ActiveHitBar = !UIManager.instance.ActiveHitBar;
        }
    }
}
