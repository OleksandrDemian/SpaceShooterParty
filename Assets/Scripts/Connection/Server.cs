using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

class Server
{
    public static Server Instance
    {
        get;
        private set;
    }

    private TcpListener server;
    private bool acceptConnection = true;
    private List<ConnectionReader> connections = new List<ConnectionReader>();
    private int nextID = 0;
    private bool hasNewConnection = false;
    private int maxPlayers = 4;

    public Server(int port)
    {
        Instance = this;
        server = new TcpListener(IPAddress.Any, port);
        server.Start();

        Thread runThread = new Thread(new ThreadStart(Run));
        runThread.Start();
    }

    public void AllowNewConnections(bool action)
    {
        acceptConnection = action;
    }

    public void Run()
    {
        Debug.Log("Server started");
        do
        {
            TcpClient client = server.AcceptTcpClient();
            if (connections.Count > maxPlayers)
                continue;
            connections.Add(new ConnectionReader(client, nextID));
            hasNewConnection = true;
            nextID++;
            Debug.Log("Connected");
        } while (acceptConnection);
    }

    public void AddNewConnection()
    {
        if (!hasNewConnection)
            return;
        
        for (int i = 0; i < connections.Count; i++)
        {
            if (!connections[i].initialized)
            {
                ConnectionManager.Instance.InstantiatePlayer(connections[i]);
            }
        }
        hasNewConnection = false;
    }

    public void RemoveConnection(ConnectionReader con)
    {
        connections.Remove(con);
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "";
    }

    public void Stop()
    {
        server.Stop();
        Debug.Log("Server stoped!");
    }
}