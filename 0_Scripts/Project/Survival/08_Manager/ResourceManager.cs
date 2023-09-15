using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public static ResourceManager instance;    // 싱글톤

    public int goldAmount;

    public int curPopulation;           // 현재 유지 중인 인구
    public int populationAmount;        // 최대 인구

    private void Awake()
    {
        
        if (instance = null)
        {

            instance = this;
        }
        else
        {

            Destroy(gameObject);
        }
    }

    public enum TYPE_RESOURCE { GOLD, POPULATION };

    public bool ChkResource(TYPE_RESOURCE _type, int _price)
    {

        if (_price < 0) _price = 0;

        switch (_type)
        {

            case TYPE_RESOURCE.GOLD:

                return goldAmount >= _price;

            case TYPE_RESOURCE.POPULATION:

                return populationAmount - curPopulation >= _price;

            default:

                return false;
        }
    }

    /// <summary>
    /// 자원 사용
    /// </summary>
    public void UseResources(TYPE_RESOURCE _type, int _amount)
    {

        if (_amount < 0) _amount = 0;

        switch (_type) 
        {

            case TYPE_RESOURCE.GOLD:

                goldAmount -= _amount;
                break;

            case TYPE_RESOURCE.POPULATION:

                populationAmount -= _amount;
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 자원 획득
    /// </summary>
    public void AddResource(TYPE_RESOURCE _type, int _amount)
    {

        if (_amount < 0) _amount = 0;

        switch (_type)
        {

            case TYPE_RESOURCE.GOLD:

                goldAmount += _amount;
                if (goldAmount > VariableManager.MAX_GOLD) goldAmount = VariableManager.MAX_GOLD;
                break;

            case TYPE_RESOURCE.POPULATION:

                populationAmount += _amount;
                if (populationAmount > VariableManager.MAX_POPULATION) populationAmount = VariableManager.MAX_POPULATION;
                break;

            default:
                break;
        }
    }

    /*
    private void Awake()
    {

        if (instance == null)
        {

            instance = this;
        }
        else
        {

            Destroy(gameObject);
        }
    }
    */
}