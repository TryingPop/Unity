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
    [SerializeField] [Tooltip("���â UI")]
    private GameObject resultUI; 

    // �Ͻ� ����, �й�, �¸� �ۿ� ����
    [SerializeField] [Tooltip("��Ȳ ���� �ؽ�Ʈ")]
    private Text informText; 

    [SerializeField] [Tooltip("��ư �ؽ�Ʈ")]
    private Text btnText;

    #endregion

    // �⺻ �뷡
    [SerializeField] [Tooltip("�⺻ ���")]
    private AudioClip bgmSnd;

    // ġƮ �뷡
    [SerializeField] [Tooltip("ġƮ ��� Ȱ��ȭ �� ����� �뷡")] 
    private AudioClip[] cheatSnd;

    private AudioSource audioSource;

    [SerializeField] [Tooltip("�÷��̾� �ִϸ�����")]
    private ThirdPersonController thirdPersonController;

    [Tooltip("��� �̼�")]
    public HuntingMission huntingMission;

    public event EventHandler Reset;

    /// <summary>
    /// ���� ����
    /// </summary>
    public enum GAMESTATE { 
                            Play, // ���� ��
                            Gameover // ���� ����
                          }


    // ���� ���� ����
    public GAMESTATE state;

    // ���� �ð�
    public float accTime;

    // ui ���� ����
    public bool uiBool;

    // ġƮ ��� ����
    private bool beCheat;

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
            Destroy(this); 
        }
        

        // �ʱ� ���� 
        state = GAMESTATE.Play;
        Time.timeScale = 1.0f;
        accTime = 1f;


        if (huntingMission == null)
        {

            huntingMission = FindObjectOfType<HuntingMission>();
        }

        if (thirdPersonController == null)
        {

            thirdPersonController = FindObjectOfType<ThirdPersonController>();
        }

        beCheat = false;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgmSnd;
        audioSource.Play();
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

        // ui�� ���� ���
        if (uiBool)
        {

            // ���콺 Ŀ�� �Ⱥ��̰� �߾ӿ� ���� 
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // ���� Ȯ��
            uiBool = false;
        }

        // ui�� �Ѵ� ���
        else 
        {

            // ���콺 Ŀ�� ���̰� ȭ�� �ۿ� ���������ϸ� ����
            Cursor.visible = true; 
            Cursor.lockState = CursorLockMode.Confined; 

            // ui ���� Ȯ��
            uiBool = true;
        }

        // result ui uiBool�� �°� ����
        resultUI.SetActive(uiBool);
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
    /// <param name="isWin">���� Ȯ�ο� ����</param>
    private void GameOverText(bool isWin)
    {

        // ��ư �̸� �����
        btnText.text = "Restart";

        // �̱� ���
        if (isWin)
        {

            // �¸� 
            informText.text = "WIN";
        }
        else
        {

            // �й�
            informText.text = "LOSE";
        }
    }


    /// <summary>
    /// ���� ���� �޼ҵ�
    /// </summary>
    /// <param name="isWin">���� Ȯ��</param>
    public void GameOver(bool isWin)
    {

        // ���콺 Ŀ�� ���̰� ȭ�� �ۿ� �������� ����
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;


        // ���� ���� ����
        state = GAMESTATE.Gameover;

        // ���� �ӵ� 1/10���� ����
        accTime = 10f;
        Time.timeScale = 0.1f;

        // resultUI �� ������ ���� ���� ui ���� ������ �ٷ� �ٽ� ����
        if (resultUI != null)
        {

            GameOverText(isWin);

            thirdPersonController.chrAnimator.SetTrigger("GameOver");
            thirdPersonController.chrAnimator.SetBool("isWin", isWin);
            thirdPersonController.hammerObj.SetActive(false);


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

            // Ŀ���� �κ��κ� ��ŸƮ
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            Reset(this, EventArgs.Empty);

            state = GAMESTATE.Play;
            Time.timeScale = 1.0f;
            accTime = 1f;
        }
    }


    /// <summary>
    /// �� ���� �� ���� �� ī��Ʈ ���� 
    /// �̺�Ʈ �޼ҵ�
    /// </summary>
    public void ChkWin()
    {

        // �� ī��Ʈ ����
        // enemyCount--;
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
            audioSource.clip = cheatSnd[UnityEngine.Random.Range(0, cheatSnd.Length)];
            audioSource.Play();
        }
    }

}