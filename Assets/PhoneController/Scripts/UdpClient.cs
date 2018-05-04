using UnityEngine;
using System.Collections;
  
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

public class UdpClient : MonoBehaviour
{
    
    Socket socket;  
    EndPoint serverEnd; 
    IPEndPoint ipEnd;  
    string recvStr;  
    string sendStr;   
    byte[] recvData = new byte[1024];   
    byte[] sendData = new byte[1024]; 
    int recvLen; 
    Thread connectThread;

    //public string sendingIP;
    public UnityEngine.UI.InputField ipaddress;
    public int sendingPort;

    public GyroToRotation gtr;
    bool isConnect;
    public GameObject checkMarker;

     
    public void InitSocket()
    {
        //
        ipEnd = new IPEndPoint(IPAddress.Parse(ipaddress.text), sendingPort);
        //
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        serverEnd = (EndPoint)sender;
        //print("waiting for sending UDP dgram");

        // 
        SocketSend("Is anybody there?");
        isConnect = false;

        //开启一个线程连接，必须的，否则主线程卡死  
        connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }

    void SocketSend(string sendStr)
    {
        //  
        sendData = new byte[1024];
        // 
        sendData = Encoding.ASCII.GetBytes(sendStr);
        // 
        socket.SendTo(sendData, sendData.Length, SocketFlags.None, ipEnd);
    }

    void SocketSend(byte[] sendStr)
    {
        // 
        socket.SendTo(sendStr, sendStr.Length, SocketFlags.None, ipEnd);
    }

    Byte[] encodePacket(Vector3 rr, Vector2 touchpos, int istate)
    {
        Byte[] packet = new Byte[4 + 4 + 4 + 4 + 4 + 4];
        Byte[] rx = BitConverter.GetBytes(rr.x);
        Byte[] ry = BitConverter.GetBytes(rr.y);
        Byte[] rz = BitConverter.GetBytes(rr.z);
        Byte[] state, x, y;

        // support one input right now
        state = BitConverter.GetBytes(istate);
        x = BitConverter.GetBytes(touchpos.x / Screen.width);
        y = BitConverter.GetBytes(touchpos.y / Screen.height);


        int index = 0;
        System.Buffer.BlockCopy(rx, 0, packet, index, rx.Length);
        index += rx.Length;
        System.Buffer.BlockCopy(ry, 0, packet, index, ry.Length);
        index += ry.Length;
        System.Buffer.BlockCopy(rz, 0, packet, index, rz.Length);
        index += rz.Length;
        System.Buffer.BlockCopy(state, 0, packet, index, state.Length);
        index += state.Length;
        System.Buffer.BlockCopy(x, 0, packet, index, x.Length);
        index += x.Length;
        System.Buffer.BlockCopy(y, 0, packet, index, y.Length);

        return packet;
    }

    //
    void SocketReceive()
    {
        //
        while (true)
        {
            //  
            recvData = new byte[1024];
            //  
            recvLen = socket.ReceiveFrom(recvData, ref serverEnd);
            print("message from: " + serverEnd.ToString()); //  
            //  
            recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
            print(recvStr);

            isConnect = true;
        }
    }

    //  
    void SocketQuit()
    {
        //  
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
        //  
        if (socket != null)
            socket.Close();
    }

    // Use this for initialization  
    void Start()
    {
        //InitSocket();   
    }

    // Update is called once per frame  
    void Update()
    {
        if (isConnect)
        {
            // send [rx,ry,rz,state,x,y]
            Vector2 touchpos = Input.touchCount > 0 ? Input.touches[0].position : new Vector2(0, 0);
            int state = Input.touchCount > 0 ? (Input.touches[0].phase - TouchPhase.Began + 1) : 0;
            Byte[] packet = encodePacket(gtr.RotationRate, touchpos, state);
            SocketSend(packet);
            checkMarker.SetActive(true);
        }
        else
        {
            checkMarker.SetActive(false);
        }
    }

    void OnApplicationQuit()
    {
        SocketQuit();
    }
}