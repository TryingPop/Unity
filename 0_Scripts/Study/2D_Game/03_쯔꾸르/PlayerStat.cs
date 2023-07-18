using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{

    public static PlayerStat instance;

    public int character_Lv;
    public int[] needExp;
    public int currentExp;


    public int hp;
    public int currentHp;
    public int mp;
    public int currentMp;

    public int atk;
    public int def;

    public int recover_hp;      // 1초당 hp회복력
    public int recover_mp;      // 1초당 mp회복력

    public string dmgSound;

    public GameObject prefabs_Floating_text;
    public GameObject parent;

    public float time;
    private float current_time;

    private void Awake()
    {

        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {

            Destroy(gameObject);
        }
    }

    private void Start()
    {

        currentHp = hp;
        current_time = time;
    }

    public void Hit(int _enemyAtk)
    {

        int dmg = 0;

        if (def >= _enemyAtk)
        {

            dmg = 1;
        }
        else
        {

            dmg = _enemyAtk - def;
        }

        currentHp -= dmg;

        if (currentHp <= 0)
        {

            Debug.Log("체력 0 미만, 게임오버");
        }

        AudioManager.instance.Play(dmgSound);

        Vector3 vector = this.transform.position;
        vector.y += 60;

        GameObject clone = Instantiate(prefabs_Floating_text, vector, Quaternion.identity);

        FloatingText floatingText = clone.GetComponent<FloatingText>();
        floatingText.text.text = dmg.ToString();
        floatingText.text.color = Color.red;
        floatingText.text.fontSize = 25;

        clone.transform.SetParent(parent.transform);

        StopAllCoroutines();
        StartCoroutine(HitCoroutine());
    }


    IEnumerator HitCoroutine()
    {

        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 0;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);

        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);

        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);

        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);

        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);

        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
    }

    private void Update()
    {

        if (currentExp >= needExp[character_Lv - 1])
        {

            character_Lv++;
            hp += character_Lv * 2;
            mp += character_Lv + 2;

            currentHp = hp;
            currentMp = mp;
            atk++;
            def++;
        }

        current_time -= Time.deltaTime;
        if (current_time < 0)
        {

            if (recover_hp > 0)
            {

                if (currentHp + recover_hp <= hp)
                {

                    currentHp += recover_hp;
                }
                else
                {

                    currentHp = hp;
                }
            }

            current_time = 0;
        }
    }
}
