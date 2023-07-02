using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{

    public string startPoint;       // ¸Ê ½ÃÀÛ À§Ä¡

    private MovingObject thePlayer;
    private CameraManager theCamera;

    private void Start()
    {

        theCamera = FindObjectOfType<CameraManager>();
        thePlayer = FindObjectOfType<MovingObject>();

        if (startPoint == thePlayer.currentMapName)
        {

            theCamera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, theCamera.transform.position.z);
            thePlayer.transform.position = this.transform.position;
        }
    }
}