using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;           // ����Ƽ�� ���� ������Ʈ
using Photon.Realtime;      // ���� ���� ���� ���̺귯��
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks       // Photon.Pun���� �����ϴ� Ŭ����
{

    private string gameVersion = "1";   // ���� ����

    public Text connectionInfoText;     // ��Ʈ��ũ ������ ǥ���� �ؽ�Ʈ
    public Button joinButton;           // �� ���� ��ư

    // ���� ����� ���ÿ� ������ ���� ���� �õ�
    private void Start()
    {

        // ���ӿ� �ʿ��� ����(���� ����) ����
        PhotonNetwork.GameVersion = gameVersion;
        // ������ ������ ������ ���� ���� �õ�
        PhotonNetwork.ConnectUsingSettings();

        // �� ���� ��ư ��� ��Ȱ��ȭ
        joinButton.interactable = false;
        // ���� �õ� ������ �ؽ�Ʈ�� ǥ��
        connectionInfoText.text = "������ ������ ���� ��...";
    }

    // ������ ���� ���� ���� �� �ڵ� ����
    public override void OnConnectedToMaster()
    {

        // �� ���� ��ư Ȱ��ȭ
        joinButton.interactable = true;
        // ���� ���� ǥ��
        connectionInfoText.text = "�¶��� : ������ ������ �����";
    }

    // ������ ���� ���� ���� �� �ڵ� ����
    public override void OnDisconnected(DisconnectCause cause)
    {

        // �� ���� ��ư ��Ȱ��ȭ
        joinButton.interactable = false;
        //���� ���� ǥ��
        connectionInfoText.text = "�������� : ������ ������ ������� ����\n���� ��õ� ��...";

        // ������ �������� ������ �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    // �� ���� �õ�
    public void Connect()
    {

        // �ߺ� ���� �õ��� ���� ���� ���� ��ư��� ��Ȱ��ȭ
        joinButton.interactable = false;

        // ������ ������ ���� ���̶��
        if (PhotonNetwork.IsConnected)
        {

            // �� ���� ����
            connectionInfoText.text = "�뿡 ����...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {

            // ������ ������ ���� ���� �ƴ϶�� ������ ������ ���� �õ�
            connectionInfoText.text = "�������� : ������ ������ ������� ����\n���� ��õ� ��...";
            // ������ �������� ������ �õ�
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // �� ���� ���� ���� �� ������ ������ ��� �ڵ� ����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {

        // ���� ���� ǥ��
        connectionInfoText.text = "�� ���� ����, ���ο� �� ����...";
        // �ִ� 4���� ���� ������ �� �� ����
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });     // �� ����� Ȯ���ϴ� ����� �����Ƿ� ���� �̸��� �Է����� �ʾҴ�
    }

    // �뿡 ���� �Ϸ�� ��� �ڵ� ����
    public override void OnJoinedRoom()
    {

        // ���� ���� ǥ��
        connectionInfoText.text = "�� ���� ����";
        // ��� �� �����ڰ� Main ���� �ε��ϰ� ��
        PhotonNetwork.LoadLevel("Make");        // �÷��̾���� ����ȭ�� ���·� ���� �ҷ��´�
                                                // ������ �̵��ϸ� ������ �÷��̾�鵵 �ش� �޼ҵ�� ���� ������ �̵��� �ϸ� ����� ���� �� �ִ�
    }
}