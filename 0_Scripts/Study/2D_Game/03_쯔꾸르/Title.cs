using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{


    private FadeManager theFade;
    private AudioManager theAudio;
    public string click_sound;

    private PlayerManager thePlayer;
    private GameManager theGM;

    private void Start()
    {
        
        theFade = FindObjectOfType<FadeManager>();
        theAudio = FindObjectOfType<AudioManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theGM = FindObjectOfType<GameManager>();
    }

    public void StartGame()
    {

        StartCoroutine(GameStartCoroutine());
    }

    IEnumerator GameStartCoroutine()
    {

        theFade.FadeOut();
        theAudio.Play(click_sound);
        yield return new WaitForSeconds(2f);

        {

            SpriteRenderer playerSpriteRenderer = thePlayer.GetComponent<SpriteRenderer>();
            Color color = playerSpriteRenderer.color;
            color.a = 1f;
            playerSpriteRenderer.color = color;
        }

        thePlayer.currentMapName = "forest";
        thePlayer.currentSceneName = "start";

        theGM.LoadStart();
    }
}
