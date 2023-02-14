using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageScript : MonoBehaviour
{

    [SerializeField] private Talk[] talk;

    private static WaitForSeconds waitTime;
    private float readTime = 0.1f; // 20프레임 기준

    private int talkNum;

    [SerializeField] private TextMesh talkText;

    private void Awake()
    {
        if (waitTime == null)
        {

            waitTime = new WaitForSeconds(readTime);
        }
    }

    private void OnEnable()
    {

        SetTalkNum();

        StartCoroutine(ReadScript());
    }

    public void SetTalk(Talk[] talk)
    {

        this.talk = talk;
    }

    private void SetTalkNum()
    {

        talkNum = UnityEngine.Random.Range(0, talk.Length);
    }


    private IEnumerator ReadScript()
    {

        for (int scriptNum = 0; scriptNum < talk[talkNum].scripts.Length; scriptNum++)
        {

            talkText.text = "";

            for (int i = 0; i < Mathf.Min(talk[talkNum].scripts[scriptNum].Length, 10); i++)
            {

                talkText.text += talk[talkNum].scripts[scriptNum][i];

                yield return waitTime;
            }

            for (int repeatNum = 0; repeatNum < 8; repeatNum++) 
            {

                yield return waitTime;
            } 
        }

        gameObject.SetActive(false);
    }

    public void StopChat()
    {

        StopAllCoroutines();
     
        talkText.text = "";
        talkText.gameObject.SetActive(false);
    }
}

[Serializable]
public class Talk
{

    public string[] scripts;
}
