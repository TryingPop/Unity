using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{

    public GameObject prefabs_Floating_Text;
    public GameObject parent;

    public string atkSound;

    private PlayerStat thePlayerStat;

    private void Start()
    {

        thePlayerStat = FindObjectOfType<PlayerStat>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "enemy")
        {

            int dmg = collision.gameObject.GetComponent<EnemyStat>().Hit(thePlayerStat.atk);
            AudioManager.instance.Play(atkSound);

            Vector3 vector = collision.transform.position;
            vector.y += 60;

            GameObject clone = Instantiate(prefabs_Floating_Text, vector, Quaternion.Euler(Vector3.zero));
            FloatingText floatingText = clone.GetComponent<FloatingText>();

            floatingText.text.text = dmg.ToString();
            floatingText.text.color = Color.white;
            floatingText.text.fontSize = 25;
            clone.transform.SetParent(parent.transform);
        }
    }
}