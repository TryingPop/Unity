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

    // 남아있는 적들의 수
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;

    [Header("패널")]
    public GameObject menuPanel;
    public GameObject gamePanel;
    public Text maxScoreText;

    [Header("좌측 상단 UI")]
    public Text scoreText;

    // 시간과 스테이지
    [Header("우측 상단 UI")]
    public Text stageText;
    public Text playTimeText;

    // 플레이어 아이템
    [Header("좌측 하단 UI")]
    public Text playerHealthText;
    public Text playerAmmoText;
    public Text playerCoinText;

    // 무기
    [Header("중앙 하단 UI")]
    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weaponRImg;

    // 적 관련
    [Header("우측 하단 UI")]
    public Text enemyAText;
    public Text enemyBText;
    public Text enemyCText;

    // 보스관련
    [Header("중앙 상단 UI")]
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

        // 좌측 상단 UI
        scoreText.text = $"{player.score:n0}";

        // 우측 상단 UI
        stageText.text = "STAGE " + stage;
        int hour = (int)(playTime / 3600);
        int minute = (int)((playTime - hour * 3600) / 60);
        int second = (int)playTime % 60;
        playTimeText.text = $"{hour:00}:{minute:00}:{second:00}";

        // 좌측 하단 UI
        // 플레이어 UI
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

        // 중앙 하단 UI
        // 무기 UI
        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weaponRImg.color = new Color(1, 1, 1, player.hasGrenades > 0 ? 1 : 0);

        // 우측 하단
        // 몬스터 숫자 UI
        enemyAText.text = enemyCntA.ToString();
        enemyBText.text = enemyCntB.ToString();
        enemyCText.text = enemyCntC.ToString();

        // 보스 체력 UI
        bossHealthBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1, 1);
    }
}
