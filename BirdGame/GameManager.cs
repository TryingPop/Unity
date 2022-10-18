using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱���

    private AudioSource audioSource;

    public Button restartBtn; // ����� ��ư
    public Text scoreText; // ���� �ؽ�Ʈ
    public Image gameoverImg; // ���ӿ��� �̹���
    public Text bestscoreText; // �ְ� ���� �ؽ�Ʈ

    public bool isGameover; // ���ӿ��� �ٸ� �������� Ȯ��
    public static int score; // ����
    public static int bestscore; // �ְ���
    public const int lvlup = 15; // 15 ������ ���̵� ���!
                                 // const�̹Ƿ� static
    void Awake()
    {
        if (instance == null) // �ν��Ͻ��� ���� ��� ����
        {
            instance = this; // ���粨�� �ν��Ͻ��� �ִ´�
        }
        else // �ν��Ͻ��� �̹� �����ϴ� ���
        {
            Destroy(gameObject); // �ڿ� ���� ����� �ı�
        }
    }

    private void Start()
    {
        score = 0; // ���ھ� 0���� �ʱ�ȭ
                   // static���� �����̹Ƿ� ������ �ʱ�ȭ�� ������ ���� ��� �̾ ����.

        audioSource = GetComponent<AudioSource>(); // �� ��ũ��Ʈ�� �����ϴ� ������Ʈ���� ����� ������Ʈ �ڵ����� ã��
        if (audioSource != null) // ����� ���� ó��
        {
            audioSource.Play(); // ����� ���
        }
        else
        {
            Debug.Log("���� �Ŵ����� ����� �ҽ��� �����ϴ�."); // ������ ����� �α׷� �����ֱ�
        }
        
    }

    public void Gameover() // ���� ����
    {
        // ���� ó�� ����
        if (score > bestscore) // ���� ���ھ �ְ� ���ھ�� ���� ���
        {
            bestscore = score; // �ְ� ���ھ ���� ���ھ�� ����
        }

        isGameover = true; // ��� �̵� �� ���, ��� ���� ��Ȱ��ȭ
        
        // ���� ó�� ����
        if (restartBtn != null) // ����ŸƮ ��ư�� ���� ���
        {
            restartBtn.gameObject.SetActive(true); // ����� ��ư Ȱ��ȭ
        }
        else // ���� ���
        {
            Debug.Log("���� �Ŵ����� ����ŸƮ ��ư�� �����ϴ�."); // ���ٰ� ����� ����
            Restart(); // ���� �����
        }

        // ���� ó�� ����
        if (gameoverImg != null) // ���� ���� �̹����� �ִ� ���
        {
            gameoverImg.gameObject.SetActive(true); // ���ӿ��� �̹��� Ȱ��ȭ
        }
        else
        {
            Debug.Log("���� �Ŵ����� ���ӿ��� �̹����� �����ϴ�."); // ���� ���� �̹��� ���ٰ� ����� ����
        }

        // ���� ó�� ����
        if (bestscoreText != null) // �ְ� ���� �ؽ�Ʈ�� ���� ��
        {
            bestscoreText.gameObject.SetActive(true); // Ȱ��ȭ
            bestscoreText.text = $"Best Score : {bestscore}"; // �ְ� ���� ����
        }
        else // ���� ���
        {
            Debug.Log("���� �Ŵ����� �ְ����� �ؽ�Ʈ�� �����ϴ�."); // �ְ� ���� �ؽ�Ʈ ���ٰ� ����� ����
        }
    }

    public void Restart() // ����ŸƮ
                          // restartbtn���� onclick �̺�Ʈ�� ���δ�
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ����� �ٽ� �ҷ�����
    }

    public void AddScore() // ���ھ� �߰�
    {
        score++; // ���ھ� �߰�
        // ���� ó������
        if (scoreText != null)
        {
            scoreText.text = $"Score : {score}"; // ���ھ� �޸�
        }
        else
        {
            Debug.Log("���� �Ŵ����� ���ھ� �ؽ�Ʈ�� �����ϴ�.");
        }
    }
}
