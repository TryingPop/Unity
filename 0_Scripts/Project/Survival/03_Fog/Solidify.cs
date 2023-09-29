using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Solidify : MonoBehaviour
{

    public Shader flatShader;
    protected Camera cam;

    private void OnEnable()
    {

        cam = GetComponent<Camera>();
        cam.SetReplacementShader(flatShader, "");
    }
}
