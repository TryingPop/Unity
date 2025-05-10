using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;


public class ChatServer : MonoBehaviour
{

    public InputField portInput;

    TcpListener server;
    bool serverStarted;

    HashSet<ServerClient> clients;
    HashSet<ServerClient> disconnectList;

    public void ServerCreate()
    {

        clients = new();
        disconnectList = new();

        try
        {

            int port = portInput.text == "" ? 8888 : int.Parse(portInput.text);
            server = new(IPAddress.Any, port);
            server.Start();
            // 비동기로 받기
            server.BeginAcceptTcpClient(AcceptTcpClient, server);

            serverStarted = true;
            ChatUIs.Instance.ShowMessage($"서버가 {port}에서 시작되었습니다.");
        }
        catch (Exception e)
        {

            ChatUIs.Instance.ShowMessage($"Socket Error: {e.Message}");
        }
    }

    void AcceptTcpClient(IAsyncResult _ar)
    {

        TcpListener listener = (TcpListener)_ar.AsyncState;
        clients.Add(new ServerClient(listener.EndAcceptTcpClient(_ar)));
        server.BeginAcceptTcpClient(AcceptTcpClient, server);

        // 연결 메세지 보내기
        SendAllMessage("%NAME");
    }

    private void Update()
    {

        if (!serverStarted) return;

        foreach(ServerClient c in clients)
        {

            if (!IsConnected(c.tcp))
            {

                c.tcp.Close();
                disconnectList.Add(c);
                continue;
            }
            else
            {

                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable)
                {

                    string data = new StreamReader(s, true).ReadLine();
                    if (data != null)
                    {

                        OnIncomingData(c, data);
                    }
                }
            }
        }

        foreach(ServerClient c in disconnectList)
        {

            clients.Remove(c);
            SendAllMessage($"{c.clientName} 연결이 끊어졌습니다.");
        }

        disconnectList.Clear();
    }



    private void OnIncomingData(ServerClient _c, string _data)
    {

        if (_data.Contains("&NAME"))
        {

            _c.clientName = _data.Split('|')[1];
            SendAllMessage($"{_c.clientName} 연결되었습니다.");
            // 연결
            return;
        }

        // 클라연결
        SendAllMessage($"{_c.clientName} : {_data}");
    }

    private void SendAllMessage(string _data)
    {

        foreach(ServerClient c in clients)
        {

            try
            {

                StreamWriter sw = new(c.tcp.GetStream());
                sw.WriteLine(_data);
                sw.Flush();
            }
            catch (Exception e)
            {

                ChatUIs.Instance.ShowMessage($"쓰기 에러 : {e.Message}를 클라이언트에게 {c.clientName}");
            }
        }
    }

    private bool IsConnected(TcpClient _c)
    {

        try
        {

            if (_c != null && _c.Client != null && _c.Client.Connected)
            {

                if (_c.Client.Poll(0, SelectMode.SelectRead))
                    return !(_c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

                return true;
            }
            else return false;
        }
        catch
        {

            return false;
        }
    }
}

public class ServerClient
{

    public TcpClient tcp;
    public string clientName;

    public ServerClient(TcpClient _socket)
    {

        clientName = "Guest";
        tcp = _socket;
    }
}