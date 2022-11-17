using System.Net;
using System.Net.Sockets;
using EchoServer;

namespace EchoServerTest;

public class ServerStartTests
{
    [Test]
    public void EchoServer_CreatesABoundServerSocket()
    {
        ServerStart.CreateListener(8765);
        Socket? listener = ServerStart.Listener;
        IPEndPoint? localEndPoint = listener!.LocalEndPoint as IPEndPoint;
        Assert.That(localEndPoint?.Address, Is.EqualTo(IPAddress.Any));
        Assert.That(localEndPoint?.Port, Is.EqualTo(8765));
    }

    [Test]
    public void ServerStart_IntegrationTestReceivesAndSends()
    {
        Thread serverThread = new Thread(new ThreadStart(BeginTestServer));

        string passedInString = "Test";
        string? result = string.Empty;

        Thread clientThread = new Thread(() => { result = BeginTestClient(passedInString); });
        
        serverThread.Start();
        clientThread.Start();
        clientThread.Join();

        Assert.That(passedInString, Is.EqualTo(result));
    }
    
    private static void BeginTestServer()
    {
        ServerStart.CreateListener(7891);
        var server = new EchoServer.EchoServer();
        server.RunServer(ServerStart.Listener!);
    }
    
    private static string? BeginTestClient(string passedInString)
    {
        TcpClient client = new TcpClient("127.0.0.1", 7891);
        var stream = client.GetStream();
        StreamReader reader = new StreamReader(stream);
        StreamWriter writer = new StreamWriter(stream);
        
        writer.WriteLine(passedInString);
        writer.Flush();
        string? result = string.Empty;
        while (!reader.EndOfStream)
        {
            result = reader.ReadLine();
        }
        return result;
    }
}