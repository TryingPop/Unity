using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageA_TimeEffect : MonoBehaviour
{

    GameObject player;

    SpriteRenderer CameraFilter;
    // Image CameraFilter;
    Color paperColor_A = Color.black;
    Color paperColor_B = new Color(1.0f, 0.0f, 0.0f, 0.22f);

    LineRenderer Stage_BackColor;
    Color backColorST_A = new Color(1.0f, 1.0f, 1.0f, 0.22f);
    Color backColorED_A = new Color(0.0f, 0.916f, 1.0f, 1f);
    Color backColorST_B = new Color(1.0f, 0.0f, 0.0f, 1f);
    Color backColorED_B = new Color(0.86f, 1.0f, 0.0f, 1f);

    void Start()
    {

        player = PlayerController.GetGameObject();
        // CameraFilter =
        //    GameObject.Find("Filter_Paper").GetComponent<SpriteRenderer>();
        CameraFilter =
            // GameObject.Find("Filter_Paper").GetComponent<Image>();
            GameObject.Find("Filter_Paper").GetComponent<SpriteRenderer>();
        Stage_BackColor = GameObject.Find("StageA_BackColor").
            GetComponent<LineRenderer>();
        paperColor_A = CameraFilter.color;
    }

    void Update()
    {

        float t = player.transform.position.x / 33.0f;
        CameraFilter.color = Color.Lerp(paperColor_A, paperColor_B, t);
        Color st = Color.Lerp(backColorST_A, backColorST_B, t);
        Color ed = Color.Lerp(backColorED_A, backColorED_B, t);
        Stage_BackColor.SetColors(st, ed);
    }
}
