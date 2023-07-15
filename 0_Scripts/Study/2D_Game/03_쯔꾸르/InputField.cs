using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputField : MonoBehaviour
{

    private PlayerManager thePlayer;

    public Text text;


    private void Start()
    {

        thePlayer = FindObjectOfType<PlayerManager>();

    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Return))
        {

            thePlayer.gameObject.name = text.text;
            Destroy(this.gameObject);
        }
    }
}
