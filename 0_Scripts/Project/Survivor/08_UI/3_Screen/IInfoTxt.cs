using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IInfoTxt 
{


    // ������ ����
    public void SetRectTrans(RectTransform _rectTrans);

    // Ÿ��Ʋ ����
    public void SetTitle(Text _titleTxt);

    // ���� ��
    public void SetInfo(Text _descTxt);
}
