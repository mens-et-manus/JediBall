using System;
using System.IO;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;


  // UdpPacket provides packetIO over UDP
  public class UDPPacketIO 
  {
    private UdpClient Receiver;
    private bool socketsOpen;
    private int localPort;

    void Start()
    {
        //do nothing. init must be called
    }

  	public void init(string hostIP, int localPort){
        LocalPort = localPort;
        socketsOpen = false;
  	}


    ~UDPPacketIO()
    {
        // latest time for this socket to be closed
        if (IsOpen())
            Close();
    }


    // Open a UDP socket and create a UDP sender.

    //returns - True on success, false on failure.</returns>
    public bool Open()
    {
        try
        {
           IPEndPoint listenerIp = new IPEndPoint(IPAddress.Any, localPort);
            Receiver = new UdpClient(listenerIp);
            socketsOpen = true;

            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("cannot open udp client interface at port "+localPort);
            Debug.LogWarning(e);
        }

        return false;
    }

    // Close the socket currently listening, and destroy the UDP sender device.
    public void Close()
    {
        if (Receiver != null)
        {
            Receiver.Close();
             Debug.Log("UDP receiver closed");
        }
        Receiver = null;
        socketsOpen = false;

    }

    public void OnDisable()
    {
        Close();
    }

    // Query the open state of the UDP socket.
    // returns - True if open, false if closed.
    public bool IsOpen()
    {
      return socketsOpen;
    }
    // Receive a packet of bytes over UDP.

    // buffer - The buffer to be read into.
    // returns - The number of bytes read, or 0 on failure
    public int ReceivePacket(byte[] buffer)
    {
//		Debug.Log ("Receiving Packet");
        if (!IsOpen())
            Open();
        if (!IsOpen())
            return 0;

      	IPEndPoint iep = new IPEndPoint(IPAddress.Any, localPort);
//		Debug.Log ("Receiver! "+Receiver);
//		Debug.Log ("IEP! " + iep); 
      	byte[] incoming = Receiver.Receive( ref iep );
//		Debug.Log ("Incoming!! "+incoming);
      	int count = Math.Min(buffer.Length, incoming.Length);
      	System.Array.Copy(incoming, buffer, count);
//		Debug.Log (count);
      	return count;
    }

   
    // The local port you're listening on.
    public int LocalPort
    {
      get
      {
        return localPort;
      }
      set
      {
        localPort = value;
      }
    }
}
