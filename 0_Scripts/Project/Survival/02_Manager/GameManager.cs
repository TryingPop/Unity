using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private STATE_GAME myState;

    public STATE_GAME MyState => myState;

    [SerializeField] protected MissionManager missions;
    public MissionManager Missions { set { missions = value; } }

    // [SerializeField] protected List<Mission> playerMissions;        // �÷��̾� �¸� �̼�
    // [SerializeField] protected List<Mission> enemyMissions;         // �÷��̾� �й� �̼�

    [SerializeField] protected Text winTxt;                         // �¸� ���ǿ� �ؽ�Ʈ
    [SerializeField] protected Text loseTxt;                        // �й� ���ǿ� �ؽ�Ʈ

    [SerializeField] protected Text gameOverText;                   // ���� �������� �˸��� �ؽ�Ʈ

    private bool isStop;                                            // �޴� Ȱ��ȭ ��?

    public bool IsGameOver
    {

        get
        {

            return myState == STATE_GAME.WIN
                || myState == STATE_GAME.LOSE;
        }
    }

    public bool IsStop
    {

        set
        {

            isStop = value;
            Time.timeScale = value ? 0f : 1f;
        }

        get
        {

            return isStop;
        }
    }


    private void Awake()
    {

        if (instance == null)
        {

            instance = this;
        }
        else
        {

            Destroy(gameObject);
        }

        //���콺 ȭ�� �� �� ������ ����
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Start()
    {

        Init();
    }

    /// <summary>
    /// �̼� ���� �� �ൿ ����
    /// ���ۿ��� ��ƾ��� ������ �����Ѵ�!
    /// </summary>
    private void Init()
    {

        isStop = false;
        myState = STATE_GAME.NONE;

        missions.Init();
    }

    /// <summary>
    /// ���� ��?
    /// </summary>
    private void GameOver(bool _isWin)
    {


        if (_isWin) myState = STATE_GAME.WIN;
        else myState = STATE_GAME.LOSE;

        gameOverText.enabled = true;
        gameOverText.text = $"{myState}";
    }

    public void ChkMissions()
    {

        if (missions.IsSuccess(false))
        {

            GameOver(false);
        }
        else if (missions.IsSuccess(true))
        {

            GameOver(true);
        }
    }

    /// <summary>
    /// �Ͻ� �������� �̼� ������Ʈ Ű�� ������ ���� ����
    /// </summary>
    public void SetMissionObjectText()
    {

        missions.SetMissionObjectText(winTxt, true);
        missions.SetMissionObjectText(loseTxt, false);
    }
}
