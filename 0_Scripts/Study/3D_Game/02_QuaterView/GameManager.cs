using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("ī�޶�")]
    public GameObject menuCam;
    public GameObject gameCam;


    [Header("������Ʈ")]
    public Player player;
    public Boss boss;

    [Header("����")]
    public GameObject itemShop;
    public GameObject weaponShop;
    public GameObject startZone;

    public int stage;
    public float playTime;

    public bool isBattle;

    [Header("���� ���� ��")]
    // �����ִ� ������ ��
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;
    public int enemyCntD;

    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;

    [Header("�г�")]
    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject overPanel;
    public Text maxScoreText;
    public Text curScoreText;
    public Text bestText;

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

        enemyList = new List<int>();
        maxScoreText.text = $"{PlayerPrefs.GetInt("MaxScore"):n0}";

        // �÷��̾� ���ھ ������ ����
        if (PlayerPrefs.HasKey("MaxScore"))
        {

            PlayerPrefs.SetInt("MaxScore", 0);
        }
    }

    public void GameStart()
    {

        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }

    public void GameOver()
    {

        gamePanel.SetActive(false);
        overPanel.SetActive(true);
        curScoreText.text = scoreText.text;

        int maxScore = PlayerPrefs.GetInt("MaxScore");
        if (player.score > maxScore)
        {

            bestText.gameObject.SetActive(true);
            PlayerPrefs.SetInt("MaxScore", player.score);
        }
    }

    public void Restart()
    {

        // SceneManager.LoadScene(0);   // ���� �ϳ��� ����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        if (player.equipWeapon == null)
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
        if (boss != null)
        {

            // �����°� �ƴ� Ȱ��ȭ ��Ȱ��ȭ�� �ߴ�
            // bossHealthGroup.anchoredPosition = Vector3.down * 50f;
            bossHealthBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1, 1);
        }
        // else
        // {

            // bossHealthGroup.anchoredPosition = Vector3.up * 200f;
        // }
    }

    // Ư�� �ݶ��̴��� �浹 �� ������ ����
    public void StageStart()
    {

        itemShop.SetActive(false);
        weaponShop.SetActive(false);
        startZone.SetActive(false);

        foreach(Transform zone in enemyZones)
        {

            zone.gameObject.SetActive(true);
        }

        isBattle = true;
        StartCoroutine(InBattle());
    }

    private IEnumerator InBattle()
    {

        if (stage % 5 == 0)
        {

            enemyCntD++;
            GameObject instantEnemy = Instantiate(enemies[3], enemyZones[2].position, enemyZones[2].rotation);
            Boss enemy = instantEnemy.GetComponent<Boss>();
            enemy.target = player.transform;
            enemy.manager = this;

            bossHealthGroup.gameObject.SetActive(true); // ���ǿ����� ��ġ ���������� �׳� ���ٰ� Ų��
            boss = enemy;
        }
        else
        {

            for (int i = 0; i < stage; i++)
            {

                int ran = Random.Range(0, 3);
                enemyList.Add(ran);

                switch (ran)
                {

                    case 0:
                        enemyCntA++;
                        break;

                    case 1:
                        enemyCntB++;
                        break;

                    case 2:
                        enemyCntC++;
                        break;
                }
            }

            while (enemyList.Count > 0)
            {

                int randZone = Random.Range(0, 4);
                GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZones[randZone].position, enemyZones[randZone].rotation);
                Enemy enemy = instantEnemy.GetComponent<Enemy>();
                enemy.target = player.transform;
                enemy.manager = this;
                enemyList.RemoveAt(0);

                yield return new WaitForSeconds(4f);
            }
        }

        while (enemyCntA + enemyCntB + enemyCntC + enemyCntD > 0)
        {

            yield return null;
        }

        yield return new WaitForSeconds(4f);
        boss = null;
        StageEnd();
    }

    public void StageEnd()
    {

        itemShop.SetActive(true);
        weaponShop.SetActive(true);
        startZone.SetActive(true);

        foreach (Transform zone in enemyZones)
        {

            zone.gameObject.SetActive(false);
        }

        isBattle = false;
        stage++;
        player.transform.position = Vector3.zero;
    }
}
