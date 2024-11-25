using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitManager : MonoBehaviour
{

    [System.Serializable]
    private class Data
    {

        [SerializeField] private Commandable[] units;
        [SerializeField] private int setLayer;

        public void Init()
        {

            for (int i = 0; i < units.Length; i++)
            {

                if (units[i] == null) continue;

                GameObject go = units[i].gameObject;
                go.layer = setLayer;
                go.SetActive(true);
            }
        }
    }

    [SerializeField] Data[] initUnits;

    private void Start()
    {
        
        for (int i = 0; i < initUnits.Length; i++)
        {

            initUnits[i].Init();
        }

        initUnits = null;
    }
}

