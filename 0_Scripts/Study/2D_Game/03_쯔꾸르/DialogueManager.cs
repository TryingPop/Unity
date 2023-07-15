using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public static DialogueManager instance;

    #region Singleton
    private void Awake()
    {
        
        if (instance == null)
        {

            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {

            Destroy(gameObject);
        }
    }
    #endregion Singleton

    public Text text;

    public SpriteRenderer rendererSprite;               // Sprite를 관리하는 컴포넌트라 보면 된다
    public SpriteRenderer rendererDialogueWindow;

    private List<string> listSentences;
    public List<Sprite> listSprites;
    public List<Sprite> listDialogueWindows;

    private int count;          // 대화 진행 상황 카운트

    public Animator animSprite;
    public Animator animDialogueWindow;

    public string typeSound;
    public string enterSound;

    private AudioManager theAudio;
    // private OrderManager theOrder;

    public bool talking = false;
    private bool keyActivated = false;
    private bool onlyText = false;

    private void Start()
    {
        count = 0;
        text.text = "";

        listSentences = new List<string>();
        listSprites = new List<Sprite>();
        listDialogueWindows = new List<Sprite>();

        theAudio = FindObjectOfType<AudioManager>();
        // theOrder = FindObjectOfType<OrderManager>();
    }

    public void ShowText(string[] _sentences)
    {

        talking = true;
        onlyText = true;

        for (int i = 0; i < _sentences.Length; i++)
        {

            listSentences.Add(_sentences[i]);
        }

        StartCoroutine(StartTextCoroutine());
    }

    public void ShowDialogue(Dialogue dialogue)
    {

        talking = true;
        onlyText = false;
        // 이벤트에서 다룬다
        // theOrder.NotMove();

        for (int i = 0; i < dialogue.sentences.Length; i++)
        {

            listSentences.Add(dialogue.sentences[i]);
            listSprites.Add(dialogue.sprites[i]);
            listDialogueWindows.Add(dialogue.dialogueWindows[i]);
        }

        animSprite.SetBool("Appear", true);
        animDialogueWindow.SetBool("Appear", true);
        StartCoroutine(StartDialogueCoroutine());
    }

    public void ExitDialogue()
    {

        text.text = "";
        count = 0;
        listSentences.Clear();
        listSprites.Clear();
        listDialogueWindows.Clear();
        animSprite.SetBool("Appear", false);
        animDialogueWindow.SetBool("Appear", false);

        talking = false;
        // 이벤트에서 다룬다
        // theOrder.Move();
    }

    IEnumerator StartTextCoroutine()
    {

        keyActivated = true;

        for (int i = 0; i < listSentences[count].Length; i++)
        {

            text.text += listSentences[count][i];
            if (i% 7 == 1)
            {

                theAudio.Play(typeSound);
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator StartDialogueCoroutine()
    {

        if (count > 0)
        {
            if (listDialogueWindows[count] != listDialogueWindows[count - 1])
            {

                animSprite.SetBool("Change", true);
                animDialogueWindow.SetBool("Appear", false);
                yield return new WaitForSeconds(0.2f);
                rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindows[count];
                rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
                animDialogueWindow.SetBool("Appear", true);
                animSprite.SetBool("Change", false);
            }
            else if (listSprites[count] != listSprites[count - 1])
            {

                animSprite.SetBool("Change", true);

                yield return new WaitForSeconds(0.1f);

                rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
                animSprite.SetBool("Change", false);
            }
            else
            {

                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {

            rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindows[count];
            rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
        }

        keyActivated = true;
        for (int i = 0; i < listSentences[count].Length; i++)
        {

            text.text += listSentences;        // 한글자씩 출력

            if (i%7 == 1)
            {

                theAudio.Play(typeSound);
            }

            yield return new WaitForSeconds(0.01f);
        }

    }

    private void Update()
    {
        
        if (talking && keyActivated && Input.GetKeyDown(KeyCode.Z))
        {

            keyActivated = false;
            count++;
            text.text = "";
            theAudio.Play(enterSound);

            if (count == listSentences.Count - 1)
            {

                StopAllCoroutines();
                ExitDialogue();
            }
            else
            {

                StopAllCoroutines();

                if (onlyText)
                {

                    StartCoroutine(StartTextCoroutine());
                }
                else
                {

                    StartCoroutine(StartDialogueCoroutine());
                }
            }
        }
    }
}
