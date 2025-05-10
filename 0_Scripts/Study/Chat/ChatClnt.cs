using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System;

using UnityEngine;
using UnityEngine.UI;

public class ChatClnt : MonoBehaviour
{

    public InputField ipInput, portInput, nickInput;
    private string clntName;

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter sw;
    private StreamReader sr;

    public void ConnectToServer()
    {

        if (socketReady) return;

        string host = ipInput.text == "" ? "127.0.0.1" : ipInput.text;
        int port = portInput.text == "" ? 8888 : int.Parse(portInput.text);

        try
        {

            socket = new(host, port);
            stream = socket.GetStream();
            sw = new(stream);
            sr = new(stream);
            socketReady = true;
        }
        catch (Exception e)
        {

            ChatUIs.Instance.ShowMessage($"클라이언트 소켓 에러 : {e.Message}");
        }
    }

    private void Update()
    {
        
        if (socketReady && stream.DataAvailable)
        {

            string data = sr.ReadLine();
            if (data != null) OnIncomingData(data);
        }
    }

    void OnIncomingData(string _data)
    {

        if (_data == "%NAME")
        {

            clntName = nickInput.text == "" ? "Guest" + UnityEngine.Random.Range(1_000, 10_000) : nickInput.text;
            SendServerMessage($"&NAME|{clntName}");
            return;
        }

        ChatUIs.Instance.ShowMessage(_data);
    }

    private void SendServerMessage(string _data)
    {

        if (!socketReady) return;

        sw.WriteLine(_data);
        sw.Flush();
    }

    public void OnSendButton(InputField _sendInput)
    {

        if (!Input.GetButtonDown("Submit")) return;
        _sendInput.ActivateInputField();
        if (_sendInput.text.Trim() == "") return;

        string message = _sendInput.text;
        _sendInput.text = "";
        SendServerMessage(message);
    }


    private void OnApplicationQuit()
    {

        CloseSocket();
    }

    private void CloseSocket()
    {

        if (!socketReady) return;

        sw.Close();
        sr.Close();
        socket.Close();
        socketReady = false;
    }
}
