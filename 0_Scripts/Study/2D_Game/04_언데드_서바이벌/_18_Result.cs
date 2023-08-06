using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _18_Result : MonoBehaviour
{

    public GameObject[] titles;

    public void Lose()
    {

        titles[0].SetActive(true);
    }

    public void Win()
    {

        titles[1].SetActive(true);
    }
}
