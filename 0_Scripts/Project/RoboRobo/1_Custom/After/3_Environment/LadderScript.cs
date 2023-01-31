using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            GameManager.instance.controller.ChkLadder(true);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {

            GameManager.instance.controller.ChkLadder(false);
        }
    }
}
