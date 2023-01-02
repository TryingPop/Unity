using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InformationMoveKeys : MonoBehaviour
{
    
    public Button[] moveKeys;


    public bool leftDir;
    public bool rightDir;
    public bool backDir;
    public bool forwardDir;

    public bool[] dirs;
    
    public Vector2 dir;
    private bool btnPressed;
    public void Awake()
    {
        dirs = new bool[] { leftDir, rightDir, backDir, forwardDir };
    }

    private void Update()
    {
        dir = Vector2.zero;

        dir.x = - Input.GetAxisRaw("Horizontal");
        dir.y = - Input.GetAxisRaw("Vertical");

        

        if (dirs[0])
        {
            dir.x += 1;
        }

        if (dirs[1])
        {
            dir.x -= 1;
        }

        if (dirs[2])
        {
            dir.y -= 1;
        }

        if (dirs[3])
        {
            dir.y += 1;
        }
        if (dir == Vector2.zero)
        {
            transform.forward = Vector3.forward;
        }
        else
        {
            transform.forward = Vector3.forward * dir.y + Vector3.right * dir.x;
        }
    }


    public void ChkBtn(int direct)
    {
        if (direct < 4)
        {
            btnPressed = true;
        }   
        else
        {
            btnPressed = false;
        }     
        dirs[direct % 4] = btnPressed;
    }

}
