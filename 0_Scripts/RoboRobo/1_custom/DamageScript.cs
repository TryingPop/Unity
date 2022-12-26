using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : Billboard
{
    [Tooltip("������ �ؽ�Ʈ �Ž�")]
    [SerializeField]
    private TextMesh txt;

    private void Awake()
    {
        cameraTrans = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.position += Vector3.up * Time.deltaTime;
        transform.LookAt(cameraTrans);
    }

    public void SetTxt(string str)
    {
        txt.text = str;
        txt.gameObject.SetActive(true);
    }
}