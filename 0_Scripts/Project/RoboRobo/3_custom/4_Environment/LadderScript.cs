using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        // 플레이어인 경우 계속 점프 가능
        if (other.tag == "Player")
        {

            GameManager.instance.controller.ChkLadder(true);
        }
    }


    private void OnTriggerExit(Collider other)
    {

        // 탈출 시 플레이어면 불가 상태로 변경
        if (other.tag == "Player")
        {

            GameManager.instance.controller.ChkLadder(false);
        }
    }
}
