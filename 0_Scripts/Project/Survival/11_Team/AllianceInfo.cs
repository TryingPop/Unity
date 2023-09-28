using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllianceInfo
{

    [SerializeField] private LayerMask allyLayer;
    [SerializeField] private LayerMask enemyLayer;

    public void SetAlly(int _layer)
    {

        allyLayer |= (1 << _layer);
        enemyLayer &= ~(1 << _layer);
    }

    public void SetEnemy(int _layer)
    {

        allyLayer &= ~(1 << _layer);
        enemyLayer |= (1 << _layer);
    }


    public LayerMask GetLayer(bool _isAlly)
    {

        return _isAlly ? allyLayer : enemyLayer;
    }
}