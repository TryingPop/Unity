using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrap : MonoBehaviour
{

    private Rigidbody[] rigidbodys;

    [SerializeField] private GameObject go_Meat;
    [SerializeField] private int damage;

    private bool isActivated = false;

    private AudioSource theAudio;

    [SerializeField] private AudioClip sound_Activate;

    private void Start()
    {
        
        rigidbodys = GetComponentsInChildren<Rigidbody>();
        theAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!isActivated)
        {

            if (other.gameObject.tag != "Untagged")
            {

                isActivated = true;
                theAudio.clip = sound_Activate;
                theAudio.Play();

                Destroy(go_Meat);

                for (int i = 0; i < rigidbodys.Length; i++)
                {

                    rigidbodys[i].useGravity = true;
                    rigidbodys[i].isKinematic = false;
                }

                if (other.transform.name == "Player")
                {

                    // µ¥¹ÌÁö
                }
            }
        }
    }
}
