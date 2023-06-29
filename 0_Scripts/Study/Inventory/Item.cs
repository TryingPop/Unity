using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Item")]
public class Item : ScriptableObject
{

    public enum ItemType
    {

        Equipment,                      // ���
        Used,                           // �Ҹ�ǰ
        Ingredient,                     // ���
        ETC,
    }

    [TextArea] public string itemDesc;  // ������ ����
                                        // TextArea�� ������ ���ٸ� ��밡���ϴ��� 
                                        // ����Ű�� �̿��� �� �� �̻� �̿밡��
    public string itemName;             // �������� �̸�
    public ItemType itemType;           // �������� ����
    public Sprite itemImage;            // �������� �̹���
    public GameObject itemPrefab;       // �������� ������

    public string weaponType;           // ���� ����
}
