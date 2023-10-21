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

    public int curSupply;           // 현재 유지 중인 인구
    public int maxSupply;           // 최대 인구

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


    /// <summary>
    /// 골드와 인구 확인
    /// </summary>
    public bool ChkResources(TYPE_MANAGEMENT _type, int _price)
    {

        if (_price < 0) _price = 0;

        switch (_type)
        {

            case TYPE_MANAGEMENT.GOLD:

                return goldAmount >= _price;

            case TYPE_MANAGEMENT.SUPPLY:

                return maxSupply - curSupply >= _price;
            default:
                return false;
        }
    }

    public bool ChkResources(int _gold, int _supply)
    {

        return _gold <= goldAmount
            && _supply <= maxSupply - curSupply;
    }

    /// <summary>
    /// 자원 사용
    /// </summary>
    public void UseResources(TYPE_MANAGEMENT _type, int _amount)
    {

        switch (_type) 
        {

            case TYPE_MANAGEMENT.GOLD:

                UseResources(_amount, 0);
                break;

            case TYPE_MANAGEMENT.SUPPLY:

                UseResources(0, _amount);
                break;

            default:
                break;
        }
    }

    public void UseResources(int _gold, int _supply)
    {

        if (_gold < 0) _gold = 0;

        goldAmount -= _gold;

        if (_supply < 0)
        {

            maxSupply -= _supply;
            if (maxSupply > VariableManager.MAX_SUPPLY) maxSupply = VariableManager.MAX_SUPPLY;
        }
        else curSupply += _supply;

        UpdateText();
    }

    /// <summary>
    /// 자원 획득
    /// </summary>
    public void AddResources(TYPE_MANAGEMENT _type, int _amount)
    {

        switch (_type)
        {

            case TYPE_MANAGEMENT.GOLD:

                AddResources(_amount, 0);
                break;

            case TYPE_MANAGEMENT.SUPPLY:

                AddResources(0, _amount);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 해당 수치만큼 자원 획득
    /// </summary>
    public void AddResources(int _gold, int _supply)
    {

        if (_gold < 0) _gold = 0;
        goldAmount += _gold;

        if (_supply < 0)
        {

            maxSupply += _supply;
            if (maxSupply < 0) maxSupply = 0;
        }
        else 
        { 
            
            curSupply -= _supply; 
            if (curSupply < 0) curSupply = 0;
        }

        UpdateText();
    }

    /// <summary>
    /// 자원 현황 갱신
    /// </summary>
    private void UpdateText()
    {

        goldText.text = goldAmount.ToString();
        populationText.text = $"{curSupply} / {maxSupply}";
    }
}