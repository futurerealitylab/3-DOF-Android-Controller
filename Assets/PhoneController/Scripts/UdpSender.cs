using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;
using System.Net;

public class UdpSender : MonoBehaviour {

    UdpClient udpClient;
    UdpClient udpClientB;

    public GyroToRotation gtr;

    //public string host;
    public int port;

    bool isConnect;
    public UnityEngine.UI.InputField ipaddress;

    public void connect()
    {
        try
        {
            udpClient.Connect(ipaddress.text, port);
            // Sends a message to the host to which you have connected.
            Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");
            print("connected");
            udpClient.Send(sendBytes, sendBytes.Length);
            isConnect = true;
        }
        catch (Exception e)
        {
            print(e.ToString());
        }
    }

    // Use this for initialization
    void Start () {
        // This constructor arbitrarily assigns the local port number.
        udpClient = new UdpClient();
        isConnect = false;
    }

    void receivePackets()
    {
        //IPEndPoint object will allow us to read datagrams sent from any source.
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

        // Blocks until a message returns on this socket from a remote host.
        Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
        string returnData = Encoding.ASCII.GetString(receiveBytes);

        // Uses the IPEndPoint object to determine which of these two hosts responded.
        float f = BitConverter.ToSingle(receiveBytes, 0);
        print("This is the message you receiveBytes " +
                                     f);
        print("This message was sent from " +
                                    RemoteIpEndPoint.Address.ToString() +
                                    " on their port number " +
                                    RemoteIpEndPoint.Port.ToString());
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
	
	// Update is called once per frame
	void Update () {
        if (isConnect)
        {
            // send [rx,ry,rz,state,x,y]
            Vector2 touchpos = Input.touchCount > 0 ? Input.touches[0].position : new Vector2(0, 0);
            int state = Input.touchCount > 0 ? (Input.touches[0].phase - TouchPhase.Began + 1) : 0;
            Byte[] packet = encodePacket(gtr.RotationRate, touchpos, state);
            udpClient.Send(packet, packet.Length);
        }
    }

    void OnApplicationQuit()
    {
        udpClient.Close();
    }
}
