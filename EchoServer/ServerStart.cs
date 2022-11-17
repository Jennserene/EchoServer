using System.Net;
using System.Net.Sockets;

namespace EchoServer;

public static class ServerStart
{
    private static Socket? _listener;
    

    public static void Main()
    {
        Console.WriteLine("Binding Socket...");
        CreateListener();
        Console.WriteLine("Socket Bound, Starting Server.");
        bool exit = false;
        while (!exit)
        {
            EchoServer server = new EchoServer();
            server.RunServer(_listener!);
            Console.WriteLine($"Server Data: {server.Data}");
            if (server.Data == "exit")
            {
                exit = true;
            }
        }
    }
    
    public static void CreateListener(int portNum = 8080)
    {
        IPAddress ipAddress = IPAddress.Any;
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, portNum);
        _listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _listener.Bind(localEndPoint);
        _listener.Listen(10);
    }

    public static Socket? Listener => _listener;
}