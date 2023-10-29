using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IInfoTxt 
{


    // 사이즈 조절
    public void SetRectTrans(RectTransform _rectTrans);

    // 타이틀 설정
    public void SetTitle(Text _titleTxt);

    // 설명 글
    public void SetInfo(Text _descTxt);
}
