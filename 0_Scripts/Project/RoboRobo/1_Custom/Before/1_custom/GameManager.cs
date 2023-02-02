using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // GameManager �̱���
    public static GameManager instance;
    
    public int enemyNum;

    #region ResultUI
    [Header("��� â")]
    [SerializeField, Tooltip("���â UI")] private GameObject resultUI; 
    [SerializeField, Tooltip("��Ȳ ���� �ؽ�Ʈ")] private Text informText; 
    [SerializeField, Tooltip("��ư �ؽ�Ʈ")] private Text btnText;

    #endregion

    [SerializeField, Tooltip("�������� ����")] Stage[] stageInfo;

    [SerializeField, Tooltip("�⺻ ���")] private AudioClip bgmSnd;
    [SerializeField, Tooltip("ġƮ ��� Ȱ��ȭ �� ����� �뷡")] private AudioClip[] cheatSnd;
    public PlayerController controller;

    [Tooltip("��� �̼�")] public HuntingMission huntingMission;

    [SerializeField] private AudioScript myAS;

    [SerializeField] private Camera mainCam;

    public event EventHandler otherReset;

    /// <summary>
    /// ���� ����
    /// </summary>
    public enum GAMESTATE 
    { 
        Play, // ���� ��
        Gameover // ���� ����
    }
    
    public GAMESTATE state;     // ���� ���� ����


    public bool accBool;        // ���� �ð�

    public bool uiBool;         // ui ���� ����

    private bool beCheat;       // ġƮ ��� ����

    private int stageNum;       // ���� ��������

    private void Awake()
    {

        // �̱��� �Ҵ�
        // is null�� == null ���� �����ϱ�!
        if (instance == null) 
        {

            instance = this;
        }
        else
        {

            // �ڿ� �߰��Ǵ� �� ���ӸŴ����� �����
            Destroy(gameObject); 
        }

        if (huntingMission == null) huntingMission = FindObjectOfType<HuntingMission>(); 
        if (controller == null) controller = FindObjectOfType<PlayerController>();
        if (myAS == null) myAS = GetComponent<AudioScript>();
        if (mainCam == null) mainCam = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // �ʱ� ���� 
        Reset();

        beCheat = false;

        myAS?.SetSnd(bgmSnd);
        myAS?.GetSnd(true);
    }

    private void Update()
    {

        // esc�� ������ �Ͻ� ����! �����
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {

            // ���� üũ
            ChkState(); 
        }
    }

    /// <summary>
    /// �Ͻ� ���� �ؾ��ϴ��� �簳�ؾ��ϴ��� �Ǻ�
    /// </summary>
    private void ChkState() 
    {

        // ���� ���¿� ���� ��Ȳ ����
        switch (state)
        {

            // ���� ���̸� �Ͻ� ���� �� �Ͻ� ���� ui ����
            case GAMESTATE.Play:
                ChkUI();
                PauseText();
                break;

            // ���� ������ ���� ���� ui ����
            case GAMESTATE.Gameover:
                ChkUI();
                break;
            
            default:
                break;
        }
    }

    /// <summary>
    /// �Ͻ� ���� ���� Ȯ��
    /// </summary>
    private void ChkUI()
    {

        // ���콺 Ŀ��
        SetCursor();

        uiBool = !uiBool;

        // result ui uiBool�� �°� ����
        resultUI.SetActive(uiBool);
    }

    /// <summary>
    /// ���콺 Ŀ�� ����
    /// </summary>
    private void SetCursor()
    {

        // -1���̸� ���� �ʴ´�
        if (stageNum == -1) return;


        if (uiBool)
        {

            // ���콺 Ŀ�� �Ⱥ��̰� �߾ӿ� ���� 
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {

            // ���콺 Ŀ�� ���̰� ȭ�� �ۿ� ���������ϸ� ����
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

    }

    /// <summary>
    /// �Ͻ� ���� ui �ؽ�Ʈ ���� �޼ҵ�
    /// </summary>
    private void PauseText()
    {

        // �Ͻ� ���� �ؽ�Ʈ �� ��ư �̸� ���� �� ���� ȿ��
        if (uiBool)
        {

            Time.timeScale = 0f;
            btnText.text = "Resume";
            informText.text = "PAUSE";
        }
        else
        {

            // ���� �ӵ� 1�� �簳
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// ���� ���� ui �ؽ�Ʈ ���� �޼ҵ�
    /// </summary>
    /// <param name="winBool">���� Ȯ�ο� ����</param>
    private void GameOverText(bool winBool)
    {

        // ��ư �̸� �����
        

        // �̱� ���
        if (winBool)
        {

            // �¸� 
            informText.text = "WIN";
            btnText.text = "Next";
        }
        else
        {

            // �й�
            informText.text = "LOSE";
            btnText.text = "Restart";
        }
    }

    /// <summary>
    /// ���� ���� �޼ҵ�
    /// </summary>
    /// <param name="winBool">���� Ȯ��</param>
    public void GameOver(bool winBool)
    {

        // ���콺 Ŀ�� ���̰� ȭ�� �ۿ� �������� ����
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        // ���� ���� ����
        state = GAMESTATE.Gameover;

        // ���� �ӵ� 1/10���� ����
        // accBool = 10f;
        Time.timeScale = 0.1f;

        // �¸��� ��� �������� ����
        if (winBool)
        {

            AddStageNum();
        }

        // resultUI �� ������ ���� ���� ui ���� ������ �ٷ� �ٽ� ����
        if (resultUI != null)
        {

            GameOverText(winBool);

            ChkUI();
        }
        else
        {

            // ���� �� �����
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>
    /// Ŀ���� �κ��κ� ���� ��ŸƮ �޼ҵ�
    /// </summary>
    public void GameStart() 
    {
        if (state == GAMESTATE.Play)
        {

            // ���� ���� ���̸� �Ͻ� ����
            ChkUI();
            PauseText();
        }
        else if (state == GAMESTATE.Gameover)
        {


            Reset();

            ChangeClearFlags();
            // Ŀ���� �κ��κ� ��ŸƮ
            SceneManager.LoadScene(GetStageName());


            otherReset(this, EventArgs.Empty);
            controller.Init();

            // accTime = 1f;
        }
    }

    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    private void Reset()
    {

        state = GAMESTATE.Play;
        Time.timeScale = 1.0f;
        SetMission();

        if (uiBool)
        {

            ChkUI();
        }
    }

    /// <summary>
    /// �������� ��ȣ �߰�
    /// </summary>
    private void AddStageNum()
    {

        stageNum++;
        
        // ��� �������� Ŭ������ ���
        if (stageNum >= stageInfo.Length)
        {

            stageNum = -1;
        }
    }

    /// <summary>
    /// �������� ��ȣ�� �� �̸� ��������
    /// </summary>
    /// <returns></returns>
    private string GetStageName()
    {
        
        if (stageNum == -1)
        {

            return "0_title";
        }
        else
        {

            return stageInfo[stageNum].stageName;
        }
    }

    /// <summary>
    /// ���� ��ǥ �� ����
    /// </summary>
    private void SetMission()
    {

        if (stageNum != -1)
        {

            huntingMission.SetTargetNum(stageInfo[stageNum].targetNum);
        }
        else
        {

            huntingMission.SetTargetNum(0);
        }
    }

    /// <summary>
    /// �� ���� �� ���� �� ī��Ʈ ���� 
    /// �̺�Ʈ �޼ҵ�
    /// </summary>
    public void ChkWin()
    {

        huntingMission.ChangeDestroyCnt();
        huntingMission.ChkRemainEnemyCnt();

        // �� ���� 0 ���ϸ� ���� �¸� �޼ҵ� ����
        if (huntingMission.ChkWin() && state != GAMESTATE.Gameover)
        {

            // ���� �¸�
            GameOver(true);
        }
    }

    public void SetAudio()
    {
        if (!beCheat)
        {

            beCheat = true;
            myAS?.SetSnd(cheatSnd[UnityEngine.Random.Range(0, cheatSnd.Length)]);
            myAS?.GetSnd(true);
        }
    }   

    /// <summary>
    /// ȭ�� ����
    /// 3�� ����������(������) ��ī�̹ڽ� ���� 
    /// </summary>
    private void ChangeClearFlags()
    {

        if (stageNum == 2)
        {

            mainCam.clearFlags = CameraClearFlags.SolidColor;
            mainCam.backgroundColor = Color.black;
        }
        else
        {

            mainCam.clearFlags = CameraClearFlags.Skybox;
            mainCam.backgroundColor = Color.white;
        }
    }
}


[Serializable]
public class Stage
{

    public string stageName;
    public int targetNum;
}