using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{

    public static ResourceManager instance;     // �̱���
                                                // ���� ���� ���� �Ϸ��� instance �����ϸ� �ȵȴ�!
                                                // �׸��� GameManager���� �����ϰ� �� ����

    public int goldAmount;              // ���� ���� ���� ����

    public int curPopulation;           // ���� ���� ���� �α�
    public int maxPopulation;           // �ִ� �α�

    [SerializeField] private Text goldText;
    [SerializeField] private Text populationText;
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

        UpdateText();
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

                return maxPopulation - curPopulation >= _price;

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
                UpdateText();
                break;

            case TYPE_RESOURCE.POPULATION:

                maxPopulation -= _amount;
                UpdateText();
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// �ڿ� ȹ��
    /// </summary>
    public void AddResources(TYPE_RESOURCE _type, int _amount)
    {

        if (_amount < 0) _amount = 0;

        switch (_type)
        {

            case TYPE_RESOURCE.GOLD:

                goldAmount += _amount;
                if (goldAmount > VariableManager.MAX_GOLD) goldAmount = VariableManager.MAX_GOLD;
                UpdateText();
                break;

            case TYPE_RESOURCE.POPULATION:

                maxPopulation += _amount;
                if (maxPopulation > VariableManager.MAX_POPULATION) maxPopulation = VariableManager.MAX_POPULATION;
                UpdateText();
                break;

            default:
                break;
        }
    }

    private void UpdateText()
    {

        goldText.text = goldAmount.ToString();
        populationText.text = $"{curPopulation} / {maxPopulation}";
    }
}