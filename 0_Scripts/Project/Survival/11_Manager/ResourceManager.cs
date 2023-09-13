using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public static ResourceManager instance;    // �̱���

    public int goldAmount;

    public int curPopulation;           // ���� ���� ���� �α�
    public int populationAmount;        // �ִ� �α�

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
    /// �ڿ� ���
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
    /// �ڿ� ȹ��
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