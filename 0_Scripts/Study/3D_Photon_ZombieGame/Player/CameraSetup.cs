using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;  // �ó׸ӽ� ���� �ڵ�
using Photon.Pun;   // PUN ���� �ڵ�

// �ó׸ӽ� ī�޶� ���� �÷��̾ �����ϵ��� ����
public class CameraSetup : MonoBehaviourPun
{

    private void Start()
    {
        
        // ���� �ڽ��� ���� �ÿ��̷�����
        if (photonView.IsMine)
        {

            // ���� �ִ� �ó׸ӽ� ���� ī�޶� ã��
            CinemachineVirtualCamera followCam =
                FindObjectOfType<CinemachineVirtualCamera>();

            // ���� ī�޶��� ���� ����� �ڽ��� Ʈ���� ������ ����
            followCam.Follow = transform;
            followCam.LookAt = transform;
        }
    }
}