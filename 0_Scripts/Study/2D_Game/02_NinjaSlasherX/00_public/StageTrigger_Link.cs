using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageTrigger_Link : MonoBehaviour
{

    // �ܺ� �Ķ���� (Inspector ǥ��)
    public string jumpSceneName;
    public string jumpLabelName;

    public int jumpDir = 0;
    public bool jumpInput = true;   // false = AutoJump
    public float jumpDelayTime = 0.0f;

    // ���� �Ķ����
    Transform playerTrfm;
    PlayerController playerCtrl;

    // �ڵ� (Monobehaviour �⺻ ��� ����)
    void Awake()
    {

        playerTrfm = PlayerController.GetTransform();
        playerCtrl = playerTrfm.GetComponent<PlayerController>();
    }

    void OnTriggerEnter2D_PlayerEvent(GameObject go)
    {

        if (!jumpInput)
        {

            Jump();
        }
    }

    // �ڵ� (�� ���� ����)
    public void Jump()
    {

        // ������ ���� ����
        if (jumpSceneName == "")
        {

            jumpSceneName = SceneManager.GetActiveScene().name;
        }

        // üũ ����Ʈ
        PlayerController.checkPointEnabled = true;
        PlayerController.checkPointLabelName = jumpLabelName;
        PlayerController.checkPointSceneName = jumpSceneName;
        PlayerController.checkPointHp = PlayerController.nowHp;

        playerCtrl.ActionMove(0.0f);
        playerCtrl.activeSts = false;

        Invoke("JumpWork", jumpDelayTime);
    }

    void JumpWork()
    {

        playerCtrl.activeSts = true;

        if (SceneManager.GetActiveScene().name == jumpSceneName)
        {

            // �� �ȿ��� ����
            GameObject[] stageLinkList =
                GameObject.FindGameObjectsWithTag("EventTrigger");
            foreach(GameObject stageLink in stageLinkList)
            {

                if (stageLink.GetComponent<StageTrigger_CheckPoint>().labelName == jumpLabelName)
                {

                    playerTrfm.position = stageLink.transform.position;
                    playerCtrl.groundY = playerTrfm.position.y;
                    Camera.main.transform.position = new Vector3(
                        playerTrfm.position.x, playerTrfm.position.y, -10.0f);
                    break;
                }
            }
        }
        else
        {

            SceneManager.LoadScene(jumpSceneName);
        }
    }
}
