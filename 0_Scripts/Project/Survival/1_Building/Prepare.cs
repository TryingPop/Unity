using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prepare : MonoBehaviour
{

    RaycastHit hit;
    public GameObject prefab;
    private MeshRenderer mesh;

    private void Awake()
    {

        mesh = GetComponentInChildren<MeshRenderer>();
    }


    private void Update()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 5000f, LayerMask.GetMask("Ground")))
        {

            transform.position = hit.point;
        }

        if (Input.GetMouseButton(0))
        {

            Instantiate(prefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("Collision!");
        mesh.material.color = Color.red;
    }

    private void OnTriggerExit(Collider other)
    {

        mesh.material.color = Color.green;
    }
}
