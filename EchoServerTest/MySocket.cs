using System.Net;
using System.Net.Sockets;

namespace EchoServerTest;

internal interface ISocket
{
    void Connect(IPAddress address, int port);
    int Send(byte[] buffer, int offset, int size);
}

public class MySocket : ISocket
{
    private readonly Socket _socket;
    private readonly ISocket _socketImplementation = null!;

    public MySocket(Socket theSocket)
    {
        _socket = theSocket;
    }

    public void Connect(IPAddress address, int port)
    {
        Console.WriteLine("Pretending to connect");
    }

    public int Send(byte[] buffer, int offset, int size)
    {
        return _socketImplementation.Send(buffer, offset, size);
    }

    public virtual void Send(byte[] stuffToSend)
    {
        _socket.Send(stuffToSend);
    }
}