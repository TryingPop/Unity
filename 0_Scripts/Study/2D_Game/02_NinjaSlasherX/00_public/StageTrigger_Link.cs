using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class StageTrigger_Link : MonoBehaviour
{

    // �ܺ� �Ķ���� (Inspector ǥ��)
    public string jumpSceneName;
    public string jumpLabelName;

    public int jumpDir = 0;
    public bool jumpInput = true;   // false = AutoJump
    public float jumpDelayTime = 0.0f;

    public bool save = true;
    public bool sePlay = true;

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

        if (save)
        {

            SaveData.SaveGamePlay();
        }

        playerCtrl.ActionMove(0.0f);
        playerCtrl.activeSts = false;

        if (sePlay)
        {

            AppSound.instance.SE_OBJ_EXIT.Play();
        }

        Invoke("JumpWork", jumpDelayTime);      // ���Ѿ �� ���� ������Ʈ�� �ı��Ǿ� �� ������ ���Ѵ�
                                                // �׷��� �÷��̾� ��Ʈ�ѷ����� ���۶� �̵��ϴ� �ڵ尡 �ִ�
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

            PlayerController.startFadeTime = 0.5f;
            SceneManager.LoadScene(jumpSceneName);
        }
    }
}
