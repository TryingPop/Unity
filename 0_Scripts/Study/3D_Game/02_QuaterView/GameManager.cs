using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject menuCam;
    public GameObject gameCam;

    public Player player;
    public Boss boss;

    public int stage;
    public float playTime;

    public bool isBattle;

    // �����ִ� ������ ��
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;

    [Header("�г�")]
    public GameObject menuPanel;
    public GameObject gamePanel;
    public Text maxScoreText;

    [Header("���� ��� UI")]
    public Text scoreText;

    // �ð��� ��������
    [Header("���� ��� UI")]
    public Text stageText;
    public Text playTimeText;

    // �÷��̾� ������
    [Header("���� �ϴ� UI")]
    public Text playerHealthText;
    public Text playerAmmoText;
    public Text playerCoinText;

    // ����
    [Header("�߾� �ϴ� UI")]
    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weaponRImg;

    // �� ����
    [Header("���� �ϴ� UI")]
    public Text enemyAText;
    public Text enemyBText;
    public Text enemyCText;

    // ��������
    [Header("�߾� ��� UI")]
    public RectTransform bossHealthGroup;
    public RectTransform bossHealthBar;

    private void Awake()
    {

        maxScoreText.text = $"{PlayerPrefs.GetInt("MaxScore"):n0}";
    }

    public void GameStart()
    {

        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }

    private void Update()
    {
        
        if (isBattle)
        {

            playTime += Time.deltaTime;
        }
    }

    private void LateUpdate()
    {

        // ���� ��� UI
        scoreText.text = $"{player.score:n0}";

        // ���� ��� UI
        stageText.text = "STAGE " + stage;
        int hour = (int)(playTime / 3600);
        int minute = (int)((playTime - hour * 3600) / 60);
        int second = (int)playTime % 60;
        playTimeText.text = $"{hour:00}:{minute:00}:{second:00}";

        // ���� �ϴ� UI
        // �÷��̾� UI
        playerHealthText.text = player.health + " / " + player.maxHealth;
        if (player.equipWeapon== null)
        {

            playerAmmoText.text = "- / " + player.ammo;
        }
        else if (player.equipWeapon.type != Weapon.Type.Melee)
        {

            playerAmmoText.text = player.equipWeapon.curAmmo + " / " + player.ammo;
        }
        else
        {

            playerAmmoText.text = "- / " + player.ammo;
        }
        playerCoinText.text = $"{player.coin:n0}";

        // �߾� �ϴ� UI
        // ���� UI
        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weaponRImg.color = new Color(1, 1, 1, player.hasGrenades > 0 ? 1 : 0);

        // ���� �ϴ�
        // ���� ���� UI
        enemyAText.text = enemyCntA.ToString();
        enemyBText.text = enemyCntB.ToString();
        enemyCText.text = enemyCntC.ToString();

        // ���� ü�� UI
        bossHealthBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1, 1);
    }
}
