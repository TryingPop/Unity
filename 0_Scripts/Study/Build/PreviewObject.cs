using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{

    // 충돌한 오브젝트의 컬라이더
    private List<Collider> colliderList = new List<Collider> ();

    [SerializeField] private int layerGround;   // 지상 레이어
    private const int IGNORE_RAYCAST_LAYER = 2;  // 무시할 레이어

    [SerializeField] private Material green;
    [SerializeField] private Material red;
    private void Update()
    {

        ChangeColor();
    }

    // 충돌하는 대상과 해당 스크립트의 대상 중 적어도 한쪽에 리지드바디가 있어야 OnTrigger??? 가 발동한다!
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

            // 레드
            SetColor(red);
        }
        else
        {

            // 초록
            SetColor(green);
        }
    }

    private void SetColor(Material mat)
    {

        // 자식들의 transform
        foreach(Transform tf_Child in this.transform)
        {

            var newMaterials = new Material[
                tf_Child.GetComponent<Renderer>().materials.Length];    // 마테리얼 갯수 가져오기

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