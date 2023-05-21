using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject_MoveBlock : MonoBehaviour
{

    public Vector3 velocityA = new Vector3(1.0f, 0.0f, 0.0f);
    public Vector3 velocityB = new Vector3(-1.0f, 0.0f, 0.0f);
    public float switchingTime = 5.0f;
    public float vt = 0;

    bool turn = false;
    float changeTime = 0.0f;

    new Rigidbody2D rigidbody2D;

    private void Awake()
    {

        rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {

        if (changeTime <= 0.0f)
        {

            changeTime = Time.fixedTime;
            rigidbody2D.velocity = velocityA;
        }

        if (Time.fixedTime + vt > changeTime + switchingTime)
        {

            rigidbody2D.velocity = turn ? velocityA : velocityB;
            turn = turn ? false : true; // turn = !turn;
            changeTime = Time.fixedTime;
        }
    }
}
