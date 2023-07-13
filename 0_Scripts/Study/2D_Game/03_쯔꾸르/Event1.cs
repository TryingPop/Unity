using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Event1 : MonoBehaviour
{

    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private FadeManager theFade;

    private bool flag;

    private void Start()
    {
        
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FindObjectOfType<FadeManager>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        // ���� �ٶ󺸰� z�� ������ ���
        // ǥ���� �̺�Ʈ�̱� ������
        if (!flag && Input.GetKeyDown(KeyCode.Z) && thePlayer.animator.GetFloat("DirY") == 1)
        {

            flag = true;
            StartCoroutine(EventCoroutine());
        }
    }

    IEnumerator EventCoroutine()
    {

        theOrder.PreLoadCharacter();
        theOrder.NotMove();

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => theDM.talking);

        theOrder.Move("player", "RIGHT");
        theOrder.Move("player", "RIGHT");
        theOrder.Move("player", "UP");

        yield return new WaitUntil(() => thePlayer.queue.Count == 0);

        theFade.Flash();
        theDM.ShowDialogue(dialogue_2);
        yield return new WaitUntil(() => theDM.talking);

        theOrder.Move();
    }
}
