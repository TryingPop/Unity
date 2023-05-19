using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UIElements;

public class StageTrigger_EventObject : MonoBehaviour
{

    // 외부 파라미터 (Inspector 표시)
    [System.Serializable] public class Rigidbody2DParam
    {

        public bool enabled = true;
        public float mass = 1.0f;
        public float linearDrag = 0.0f;
        public float angularDrag = 0.05f;
        public float gravityScale = 1.0f;
        public bool fixedAngle = false;
        public bool isKinematic = false;
        public RigidbodyInterpolation2D interpolation =
            RigidbodyInterpolation2D.None;
        public RigidbodySleepMode2D sleepingMode = 
            RigidbodySleepMode2D.StartAwake;
        public CollisionDetectionMode2D collisionDetection =
            // CollisionDetectionMode2D.None;
            CollisionDetectionMode2D.Discrete;

        [Header("------------------")]
        public Vector2 centerOfMass = new Vector2(0.0f, 0.0f);
        public Vector2 velocity = new Vector2(0.0f, 0.0f);
        public float angularVelocity = 0.0f;

        [Header("------------------")]
        public bool addForceEnabled = false;
        public Vector2 addForcePower = new Vector2(0.0f, 0.0f);
        public bool addForceAtPositionEnabled = false;
        public GameObject addForceAtPositionObject;
        public Vector2 addForceAtPositionPower = new Vector2(0.0f, 0.0f);
        public bool addRelativeForceEnabled = false;
        public Vector2 addRelativeForcePower = new Vector2(0.0f, 0.0f);
        public bool addTorqueEnabled = false;
        public float addTorquePower = 0.0f;
        public bool movePositionEnabled = false;
        public Vector2 movePosition = new Vector2(0.0f, 0.0f);
        public bool moveRotationEnabled = false;
        public float moveRotation = 0.0f;
    }

    public float runTime = 0.0f;
    public float destroyTime = 0.0f;

    [Space(10)]
    public bool sendMessageObjectEnabled = false;
    public string sendMessageString = "OnTriggerEnter2D_PlayerEvent";
    public GameObject[] sendMessageObjectList;
    public bool instantiateGameObjectEnabled = false;
    public GameObject[] instantiateGameObjectList;

    [Space(10)]
    public Rigidbody2DParam rigidbody2DParam;

    // 외부 파라미터
    // [System.NonSerialized]
    [HideInInspector] public bool triggerOn = false;

    // 코드 (MonoBehaviour 기본 기능 구현)
    void OnTriggerEnter2D_PlayerEvent(GameObject go)
    {

        Invoke("runTriggerWork", runTime);
    }

    void runTriggerWork()
    {

        if (rigidbody2DParam.enabled)
        {

            if (gameObject.GetComponent<Rigidbody2D>() == null)
            {

                gameObject.AddComponent<Rigidbody2D>();
            }

            Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.mass = rigidbody2DParam.mass;
            rigidbody2D.drag = rigidbody2DParam.linearDrag;
            rigidbody2D.angularDrag = rigidbody2DParam.angularDrag;
            rigidbody2D.gravityScale = rigidbody2DParam.gravityScale;
            rigidbody2D.fixedAngle = rigidbody2DParam.fixedAngle;
            rigidbody2D.isKinematic = rigidbody2DParam.isKinematic;
            rigidbody2D.interpolation = rigidbody2DParam.interpolation;
            rigidbody2D.sleepMode = rigidbody2DParam.sleepingMode;
            rigidbody2D.collisionDetectionMode = rigidbody2DParam.collisionDetection;

            rigidbody2D.centerOfMass = rigidbody2DParam.centerOfMass;
            rigidbody2D.velocity = rigidbody2DParam.velocity;
            rigidbody2D.angularVelocity = rigidbody2DParam.angularVelocity;

            if (rigidbody2DParam.addForceEnabled)
            {

                rigidbody2D.AddForce(rigidbody2DParam.addForcePower);
            }
            if (rigidbody2DParam.addForceAtPositionEnabled)
            {

                rigidbody2D.AddForceAtPosition(
                    rigidbody2DParam.addForceAtPositionPower,
                    rigidbody2DParam.addForceAtPositionObject.transform.position);
            }
            if (rigidbody2DParam.addRelativeForceEnabled)
            {

                rigidbody2D.AddRelativeForce(rigidbody2DParam.addRelativeForcePower);
            }
            if (rigidbody2DParam.addTorqueEnabled)
            {

                rigidbody2D.MovePosition(rigidbody2DParam.movePosition);
            }
            if (rigidbody2DParam.moveRotationEnabled)
            {

                rigidbody2D.MoveRotation(rigidbody2DParam.moveRotation);
            }
        }
        
        if (sendMessageObjectEnabled && sendMessageObjectList != null)
        {

            foreach(GameObject go in sendMessageObjectList)
            {

                go.SendMessage(sendMessageString, gameObject);
            }
        }

        if (instantiateGameObjectEnabled && instantiateGameObjectList != null)
        {

            foreach(GameObject go in instantiateGameObjectList)
            {

                Instantiate(go);
            }
        }

        if (destroyTime > 0.0f)
        {

            Destroy(gameObject, destroyTime);
        }

        triggerOn = true;
    }
}