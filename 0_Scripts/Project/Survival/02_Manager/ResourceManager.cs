using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{

    public static ResourceManager instance;     // 싱글톤
                                                // 적도 돈을 벌게 하려면 instance 선언하면 안된다!
                                                // 그리고 GameManager에서 관리하게 할 예정

    public int goldAmount;              // 현재 보유 중인 골드양

    public int curPopulation;           // 현재 유지 중인 인구
    public int maxPopulation;           // 최대 인구

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



    public bool ChkResource(TYPE_MANAGEMENT _type, int _price)
    {

        if (_price < 0) _price = 0;

        switch (_type)
        {

            case TYPE_MANAGEMENT.GOLD:

                return goldAmount >= _price;

            case TYPE_MANAGEMENT.POPULATION:

                return maxPopulation - curPopulation >= _price;

            default:

                return false;
        }
    }

    /// <summary>
    /// 자원 사용
    /// </summary>
    public void UseResources(TYPE_MANAGEMENT _type, int _amount)
    {

        if (_amount < 0) _amount = 0;

        switch (_type) 
        {

            case TYPE_MANAGEMENT.GOLD:

                goldAmount -= _amount;
                UpdateText();
                break;

            case TYPE_MANAGEMENT.POPULATION:

                maxPopulation -= _amount;
                UpdateText();
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 자원 획득
    /// </summary>
    public void AddResources(TYPE_MANAGEMENT _type, int _amount)
    {

        if (_amount < 0) _amount = 0;

        switch (_type)
        {

            case TYPE_MANAGEMENT.GOLD:

                goldAmount += _amount;
                if (goldAmount > VariableManager.MAX_GOLD) goldAmount = VariableManager.MAX_GOLD;
                UpdateText();
                break;

            case TYPE_MANAGEMENT.POPULATION:

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