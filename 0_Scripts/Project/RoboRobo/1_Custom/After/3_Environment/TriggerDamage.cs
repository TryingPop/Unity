using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour
{

    [SerializeField] private int dmg;
    private static List<GameObject> objs = new List<GameObject>();


    private void OnTriggerStay(Collider other)
    {
        if (!objs.Contains(other.gameObject))
        {

            objs.Add(other.gameObject);
            other.gameObject.GetComponent<Stats>().OnDamaged(dmg);
            StartCoroutine(Timer(other.gameObject));
        }
    }

    private IEnumerator Timer(GameObject obj)
    {

        yield return new WaitForSeconds(1f);
        objs.Remove(obj);
    }
}
