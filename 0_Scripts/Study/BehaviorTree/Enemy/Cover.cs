using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{

    [SerializeField] private Transform[] coverSpot;

    public Transform[] GetCoverSpots()
    {

        return coverSpot;
    }
}
