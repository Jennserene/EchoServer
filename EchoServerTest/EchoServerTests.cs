using System.Net;
using System.Net.Sockets;
using EchoServer;

namespace EchoServerTest;

public class EchoServerTests
{
    [Test]
    public void EchoServer_StreamIsConnected()
    {
        EchoServer.EchoServer? server = null;
        Thread serverThread = new Thread(new ThreadStart(() => { server = BeginTestServer(); }));
        Thread clientThread = new Thread(new ThreadStart(BeginTestClient));

        serverThread.Start();
        clientThread.Start();
        serverThread.Join();
        
        Assert.That(server!.StreamConnected, Is.True);

        server.CloseHandler();
    }

    private static EchoServer.EchoServer BeginTestServer()
    {
        ServerStart.CreateListener(1234);
        Socket? listener = ServerStart.Listener;
        EchoServer.EchoServer server = new EchoServer.EchoServer();
        server.StartServer(listener!);
        return server;
    }
    
    private static void BeginTestClient()
    {
        TcpClient client = new TcpClient("127.0.0.1", 1234);
        var stream = client.GetStream();
    }
}