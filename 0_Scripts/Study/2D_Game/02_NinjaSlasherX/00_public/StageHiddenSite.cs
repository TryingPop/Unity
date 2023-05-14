using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageHiddenSite : MonoBehaviour
{

    [SerializeField] SpriteRenderer[] sprites;

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "PlayerBody")
        {

            for (int i = 0; i < sprites.Length; i++) 
            {

                sprites[i].color = new Color(0f, 0f, 0f, 0.8f);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.tag == "PlayerBody")
        {

            for (int i = 0; i < sprites.Length; i++)
            {

                sprites[i].color = Color.white;
            }
        }
    }
}
