using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour
{
    private Text txt;

    [SerializeField] private float onTime = 0.3f;
    [SerializeField] private float offTime = 0.1f;

    private WaitForSecondsRealtime onWaitTime;
    private WaitForSecondsRealtime offWaitTime;

    private IEnumerator fade;


    private void Awake()
    {

        txt = GetComponent<Text>();

        onWaitTime = new WaitForSecondsRealtime(onTime);
        offWaitTime = new WaitForSecondsRealtime(offTime);

        fade = Fade();
    }

    private void OnEnable()
    {

        StartCoroutine(fade);
    }

    private IEnumerator Fade()
    {

        while (true)
        {

            txt.enabled = true;
            yield return onWaitTime;

            txt.enabled = false;
            yield return offWaitTime;
        }
    }
}
