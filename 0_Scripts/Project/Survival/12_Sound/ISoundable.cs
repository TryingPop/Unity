using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ISoundable
{

    /// <summary>
    /// ���� ����
    /// </summary>
    public void SetVolume(float _volume);

    /// <summary>
    /// ���� ���� �� ����� ����, �Ͻ������� Ȯ��
    /// </summary>
    public void OnEnterSound(STATE_SELECTABLE _state, bool _isTemp);

    /// <summary>
    /// ���� Ż�� �� ����� ����, �Ͻ������� Ȯ��
    /// </summary>
    public void OnExitSound(STATE_SELECTABLE _state, bool _isTemp);
}