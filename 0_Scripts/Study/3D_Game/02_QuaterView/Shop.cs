using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{

    public RectTransform uiGroup;
    public Animator anim;

    private Player enterPlayer;

    public GameObject[] itemObjs;
    public int[] itemPrices;
    public Transform[] itemPos;

    public Text talkText;
    public string[] talkData;

    public void Enter(Player player)
    {

        enterPlayer = player;
        uiGroup.anchoredPosition = Vector3.zero;
    }

    public void Exit()
    {

        anim.SetTrigger("doHello");
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }

    public void Buy(int index)
    {

        int price = itemPrices[index];
        if (price > enterPlayer.coin)
        {

            StopAllCoroutines();
            StartCoroutine(Talk());
            return;
        }

        enterPlayer.coin -= price;
        Vector3 randVec = Vector3.right * Random.Range(-3, 3)
            + Vector3.forward * Random.Range(-3, 3);

        Instantiate(itemObjs[index], itemPos[index].position + randVec + Vector3.up, itemPos[index].rotation);
    }

    private IEnumerator Talk()
    {

        talkText.text = talkData[1];

        yield return new WaitForSeconds(2f);

        talkText.text = talkData[0];
    }
}