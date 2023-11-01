using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private STATE_GAME myState;

    public STATE_GAME MyState => myState;

    
    [SerializeField] protected List<Mission> playerMissions;        // �÷��̾� �¸� �̼�
    [SerializeField] protected List<Mission> enemyMissions;         // �÷��̾� �й� �̼�

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

        // �¸� ������ �ִ� 2��!
        if (playerMissions.Count > VarianceManager.MAX_MISSIONS)
        {

            for (int i = playerMissions.Count - 1; i >= VarianceManager.MAX_MISSIONS; i--)
            {

                playerMissions.RemoveAt(i);
            }
        }


        // �й� ������ �ִ� 2��!
        if (enemyMissions.Count > VarianceManager.MAX_MISSIONS)
        {

            for (int i = enemyMissions.Count -1; i >= VarianceManager.MAX_MISSIONS; i--)
            {

                enemyMissions.RemoveAt(i);
            }
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

        for (int i = 0; i < playerMissions.Count; i++)
        {

            playerMissions[i].Init(this);
        }

        for (int i = 0; i < enemyMissions.Count; i++)
        {

            enemyMissions[i].Init(this);
        }
    }

    /// <summary>
    /// �̼� Ȯ��
    /// </summary>
    /// <param name="_unit">Ȯ���� ����</param>
    /// <param name="_building">Ȯ���� �ǹ�</param>
    /// <param name="_idx">���° �̼� Ȯ������</param>
    /// <param name="_missions">Ȯ���� �̼ǹ�ȣ</param>
    private void ChkMission(Unit _unit, Building _building, List<Mission> _missions)
    {

        for (int i = 0; i <= 0; i++)
        {

            _missions[i].Chk(_unit, _building);
        }
    }

    /// <summary>
    /// �¸� Ȯ��
    /// </summary>
    private bool ChkWin(List<Mission> _missions)
    {

        for (int i = 0; i < _missions.Count; i++)
        {

            if (!_missions[i].IsSucess) return false;
        }

        return true;
    }

    /// <summary>
    /// ���� �޼��ߴ��� Ȯ��
    /// </summary>
    public void Chk(Unit _unit, Building _building)
    {

        if (IsGameOver) return;

        ChkMission(_unit, _building, playerMissions);
        if (ChkWin(playerMissions)) 
        { 

            GameOver(true);
            return;
        }
        ChkMission(_unit, _building, enemyMissions);
        if (ChkWin(enemyMissions)) GameOver(false);
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

    /// <summary>
    /// �Ͻ� �������� �̼� ������Ʈ Ű�� ������ ���� ����
    /// </summary>
    public void SetMissionObjectText()
    {

        int len = Mathf.Min(2, playerMissions.Count);
        for (int i = 0; i < len; i++)
        {

            winTxt.text = $"{playerMissions[i].GetMissionObjectText(true)}\n";
        }

        len = Mathf.Min(3, enemyMissions.Count);
        for (int i = 0; i < len; i++)
        {

            loseTxt.text = $"{enemyMissions[i].GetMissionObjectText(false)}\n";
        }
    }


}
