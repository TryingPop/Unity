using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public enum Type
    {

        Ammo,
        Coin,
        Grenade,
        Heart,
        Weapon,
    };

    public Type type;
    public int value;
    public float rotateSpeed;

    private void Update()
    {

        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
