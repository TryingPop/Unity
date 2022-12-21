using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // GameManager �̱���
    public static GameManager instance; 

    #region ResultUI
    [Header("��� â")]
    [SerializeField] [Tooltip("���â UI")]
    private GameObject resultUI; 

    // �Ͻ� ����, �й�, �¸� �ۿ� ����
    [SerializeField] [Tooltip("��Ȳ ���� �ؽ�Ʈ")]
    private Text informText; 

    [SerializeField] [Tooltip("��ư �ؽ�Ʈ")]
    private Text btnText;

    // 5�� ����
    [SerializeField] [Tooltip("ġƮ ��ư��")]
    private Image[] CheatBtns;


    #endregion

    // �⺻ �뷡
    [SerializeField] [Tooltip("�⺻ ���")]
    private AudioClip bgmSnd;

    // ġƮ �뷡
    [SerializeField] [Tooltip("ġƮ ��� Ȱ��ȭ �� ����� �뷡")] 
    private AudioClip[] cheatSnd;

    private AudioSource audio;

    /// <summary>
    /// ���� ����
    /// </summary>
    public enum GAMESTATE { 
                            Play, // ���� ��
                            Gameover // ���� ����
                          }


    // ���� ���� ����
    [SerializeField]
    private GAMESTATE state;

    // �� �� 0���ϸ� �¸�
    private int enemyCount;
    
    // ���� �ð�
    public float accTime;

    // ui ���� ����
    public bool uiBool;

    // ���� Ȱ��ȭ�� ġƮ ��ȣ
    private int cheatBtnNum;

    // ġƮ ��� ����
    private bool beCheat;

    private void Awake()
    {

        // ȭ�鿡 ���콺 Ŀ�� ��װ� �Ⱥ��̰� �ϱ�
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 

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
        enemyCount = 0;
        accTime = 1f;
        SetCheatBtnNum(0);

        beCheat = false;
        audio = GetComponent<AudioSource>();
        audio.clip = bgmSnd;
        audio.Play();
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
            SceneManager.LoadScene("2_custom");
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    /// <summary>
    /// ���� �� �� ���� üũ
    /// </summary>
    public void ChkEnemyCount()
    {
        enemyCount++;
    }

    /// <summary>
    /// �� ���� �� ���� �� ī��Ʈ ���� 
    /// �̺�Ʈ �޼ҵ�
    /// </summary>
    public void ChkWin()
    {

        // �� ī��Ʈ ����
        enemyCount--;

        // �� ���� 0 ���ϸ� ���� �¸� �޼ҵ� ����
        if (enemyCount <= 0) 
        {

            // ���� �¸�
            GameOver(true);
        }
    }

    public void SetCheatBtnNum(int num)
    {
        // Į�� �ʱ�ȭ
        CheatBtns[cheatBtnNum].color = Color.white;


        cheatBtnNum = num;
        Color color = Color.gray;
        color.a = 0.5f;
        CheatBtns[cheatBtnNum].color = color;
    }


    public void SetAudio()
    {
        if (!beCheat)
        {
            beCheat = true;
            audio.clip = cheatSnd[Random.Range(0, cheatSnd.Length)];
            audio.Play();
        }
    }

}