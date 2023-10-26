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

            // ���� �ѱ�� �ʱ�ȭ
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

            // �ƹ����µ� �ƴ� ���� Ű�Է��� �����ϴ�!
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

        // ��Ʈ�ٴ� �ٷ� ���� �Ҵ�!
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {

            UIManager.instance.ActiveHitBar = !UIManager.instance.ActiveHitBar;
        }
    }
}
