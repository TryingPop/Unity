using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;  // 시네머신 관련 코드
using Photon.Pun;   // PUN 관련 코드

// 시네머신 카메라가 로컬 플레이어를 추적하도록 설정
public class CameraSetup : MonoBehaviourPun
{

    private void Start()
    {
        
        // 만약 자신이 로컬 플에이러마녀
        if (photonView.IsMine)
        {

            // 씬에 있는 시네머신 가상 카메라를 찾고
            CinemachineVirtualCamera followCam =
                FindObjectOfType<CinemachineVirtualCamera>();

            // 가상 카메라의 추적 대상을 자신의 트랜스 폼으로 변경
            followCam.Follow = transform;
            followCam.LookAt = transform;
        }
    }
}