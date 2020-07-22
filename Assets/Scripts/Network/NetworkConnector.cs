using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class NetworkConnector : MonoBehaviour
{
    public Text ErrorText;
    public InputField UserName;
    public Dropdown UserClass;

    TcpClient client;
    NetworkStream stream;

    void Start()
    {
        try
        {
            client = new TcpClient("localhost", 54665);
            stream = client.GetStream();

            /*byte[] rawData = Encoding.UTF8.GetBytes(data);
            stream.Write(rawData, 0, rawData.Length);*/
            StartCoroutine(CheckMessage());
        }
        catch(Exception e)
        {
            ErrorText.text = e.Message;
        }
    }

    public void Auth()
    {
        AuthRequest request = new AuthRequest();
        request.UserName = UserName.text;
        request.UserClass = UserClass.itemText.text;

        string data = JsonUtility.ToJson(request);
        byte[] rawData = Encoding.UTF8.GetBytes(data);
        stream.Write(rawData, 0, rawData.Length);
    }

    IEnumerator CheckMessage()
    {
        while (stream != null)
        {
            if (stream.DataAvailable)
            {
                byte[] bytes = new byte[client.ReceiveBufferSize];
                int length = stream.Read(bytes, 0, bytes.Length);
                string s = Encoding.UTF8.GetString(bytes, 0, length);
                if (s == "") continue;
                Debug.LogFormat("GET: {0}", s);
            }
            yield return new WaitForSeconds(1);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        client.Close();
    }
}

[Serializable]
public class NetworkRequest
{
    public string key = "asdqwer";
}

[Serializable]
public class AuthRequest: NetworkRequest
{
    public string UserName;
    public string UserClass;
}
