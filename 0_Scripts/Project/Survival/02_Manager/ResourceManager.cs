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

    public int curSupply;           // ���� ���� ���� �α�
    public int maxSupply;           // �ִ� �α�

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
    /// ���� �α� Ȯ��
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
    /// �ڿ� ���
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
    /// �ڿ� ȹ��
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
    /// �ش� ��ġ��ŭ �ڿ� ȹ��
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
    /// �ڿ� ��Ȳ ����
    /// </summary>
    private void UpdateText()
    {

        goldText.text = goldAmount.ToString();
        populationText.text = $"{curSupply} / {maxSupply}";
    }
}