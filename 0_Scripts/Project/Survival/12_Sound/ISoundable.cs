using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ISoundable
{

    /// <summary>
    /// 볼륨 조절
    /// </summary>
    public void SetVolume(float _volume);

    /// <summary>
    /// 상태 진입 시 재생할 사운드, 일시적인지 확인
    /// </summary>
    public void OnEnterSound(STATE_SELECTABLE _state, bool _isTemp);

    /// <summary>
    /// 상태 탈출 시 재생할 사운드, 일시적인지 확인
    /// </summary>
    public void OnExitSound(STATE_SELECTABLE _state, bool _isTemp);
}