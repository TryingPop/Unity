using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

// ������ ���ӿ��� ���θ� �����ϴ� ���� �Ŵ���
public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{

    // �̱��� ���ٿ� ������Ƽ
    public static GameManager instance
    {

        get
        {

            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {

                // ������ GameManager ������Ʈ�� ã�Ƽ� �Ҵ�
                m_instance = FindObjectOfType<GameManager>();
            }

            // �̱��� ������Ʈ ��ȯ
            return m_instance;
        }
    }

    private static GameManager m_instance;          // �̱����� �Ҵ�� static ����

    public GameObject playerPrefab;                 // ������ �÷��̾� ĳ���� ������

    private int score = 0;                          // ���� ���� ����
    public bool isGameover { get; private set; }    // ���ӿ��� ����

    // �ֱ������� �ڵ� ����Ǵ� ����ȭ �޼ҵ�
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        // ���� ������Ʈ��� ����κ��� �����
        if (stream.IsWriting)
        {

            // ��Ʈ��ũ�� ���� score �� ������
            stream.SendNext(score);
        }
        else
        {

            // ����� ������Ʈ��� �б� �κ��� �����

            // ��Ʈ��ũ�� ���� score �� �ޱ�
            score = (int)stream.ReceiveNext();

            // ����ȭ�Ͽ� ���� ������ UI�� ǥ��
            UIManager.instance.UpdateScoreText(score);
        }
    }

    private void Awake()
    {
        
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
        if (instance != this)
        {

            // �ڽ��� �ı�
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        // ������ ���� ��ġ ����
        Vector3 randomSpawnPos = Random.insideUnitSphere * 5f;
        // ��ġ y ���� 0���� ����
        randomSpawnPos.y = 0f;

        // ��Ʈ��ũ���� ��� Ŭ���̾�Ʈ���� ���� ����
        // �ش� ���� ������Ʈ�� �ֵ����� ���� �޼ҵ带 ���� ������ Ŭ���̾�Ʈ�� ����
        PhotonNetwork.Instantiate(playerPrefab.name, randomSpawnPos, Quaternion.identity);
    }

    // ������ �߰��ϰ� UI ����
    public void AddScore(int newScore)
    {

        // ���ӿ����� �ƴ� ���¿����� ���� �߰� ����
        if (!isGameover)
        {

            // �����߰�
            score += newScore;

            // ���� UI �ؽ�Ʈ ����
            UIManager.instance.UpdateScoreText(score);
        }
    }

    // ���ӿ��� ó��
    public void EndGame()
    {

        // ���ӿ��� ���¸� ������ ����
        isGameover = true;

        // ���ӿ��� UI Ȱ��ȭ
        UIManager.instance.SetActiveGameoverUI(true);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            PhotonNetwork.LeaveRoom();
        }
    }

    // ���� ���� �� �ڵ� ����Ǵ� �޼ҵ�
    public override void OnLeftRoom()
    {

        // ���� ������ �κ� ������ ���ư�
        SceneManager.LoadScene("Lobby");
    }
}