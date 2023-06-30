using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{

    // �浹�� ������Ʈ�� �ö��̴�
    private List<Collider> colliderList = new List<Collider> ();

    [SerializeField] private int layerGround;   // ���� ���̾�
    private const int IGNORE_RAYCAST_LAYER = 2;  // ������ ���̾�

    [SerializeField] private Material green;
    [SerializeField] private Material red;
    private void Update()
    {

        ChangeColor();
    }

    // �浹�ϴ� ���� �ش� ��ũ��Ʈ�� ��� �� ��� ���ʿ� ������ٵ� �־�� OnTrigger??? �� �ߵ��Ѵ�!
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {

            colliderList.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {

            colliderList.Remove(other);
        }
    }


    private void ChangeColor()
    {

        if (colliderList.Count > 0)
        {

            // ����
            SetColor(red);
        }
        else
        {

            // �ʷ�
            SetColor(green);
        }
    }

    private void SetColor(Material mat)
    {

        // �ڽĵ��� transform
        foreach(Transform tf_Child in this.transform)
        {

            var newMaterials = new Material[
                tf_Child.GetComponent<Renderer>().materials.Length];    // ���׸��� ���� ��������

            for (int i = 0; i < newMaterials.Length; i++)
            {

                newMaterials[i] = mat;
            }

            tf_Child.GetComponent<Renderer>().materials = newMaterials;
        }
    }

    public bool IsBuildable()
    {

        return colliderList.Count == 0;
    }
}