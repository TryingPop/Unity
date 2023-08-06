using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prepare : MonoBehaviour
{

    RaycastHit hit;
    public GameObject prefab;
    private MeshRenderer mesh;
    private bool doBuild;

    public Vector3 unit;

    private Vector3 pos;

    private void Awake()
    {

        mesh = GetComponentInChildren<MeshRenderer>();
        doBuild = true;
    }


    private void Update()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 5000f, LayerMask.GetMask("Ground")))
        {

            pos = hit.point;

            // ¥‹¿ß ∫§≈Õ∑Œ ∏¬√Á¡ÿ¥Ÿ
            pos.x = Mathf.FloorToInt(pos.x / unit.x) * unit.x;
            pos.y = Mathf.FloorToInt(pos.y / unit.y) * unit.y;
            pos.z = Mathf.FloorToInt(pos.z / unit.z) * unit.z;

            transform.position = pos;
        }

        if (Input.GetMouseButton(0))
        {

            if (doBuild)
            {

                Instantiate(prefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
                Build.isBuild = false;
            }
        }
    }

    private void SetColor(bool doBuild)
    {

        Color color;

        if (doBuild)
        {

            color = Color.green;
            color.a = 0.3f;
        }
        else
        {

            color = Color.red;
            color.a = 0.3f;
        }

        mesh.material.color = color;
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.tag != "Ground" && doBuild)
        {

            doBuild = false;
            SetColor(doBuild);
        }
    }


    private void OnTriggerExit(Collider other)
    {

        doBuild = true;
        SetColor(doBuild);
    }
}
