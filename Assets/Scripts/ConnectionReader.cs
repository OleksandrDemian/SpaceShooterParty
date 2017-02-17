using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class ConnectionReader
{
    private StreamWriter writer;
    private StreamReader reader;
    private NetworkStream stream;
    private IJoystickListener listener;
    private int id;
    public bool initialized = false;

    public ConnectionReader(TcpClient client, int id)
    {
        stream = client.GetStream();
        reader = new StreamReader(stream);
        writer = new StreamWriter(stream);
        this.id = id;
    }

    public void Write(string message)
    {
        writer.WriteLine(message);
        writer.Flush();
    }

    //float time = 0;
    public void Read()
    {
        //time += Time.deltaTime;
        while (stream.DataAvailable)
        {
            string message = reader.ReadLine();
            listener.OnMessageRead(message);
        }
        //Debug.Log(time);
        //time = 0;
    }

    public int ID
    {
        get { return id; }
    }

    public void SetJoystickListener(IJoystickListener listener)
    {
        this.listener = listener;
    }
}