using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{

    [SerializeField] private UIScript[] scripts;
    [SerializeField] private Sprite[] faces;
    [SerializeField] private Vector3 initPos;
    private bool active;

    // test
    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space))
        {

            var pos = new Vector3(0, -35f, 0f); 
            scripts[0].Init(faces[0], "스크립트 내용이라고?", new Vector2(160f, 40f), ref pos);
            active = true;
        }

        if (active)
        {

            if (scripts[0].ChkTime())
            {

                scripts[0].EndPos(ref initPos);
                active = false;
            }
        }
    }
}
