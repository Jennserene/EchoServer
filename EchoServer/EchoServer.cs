using System.Net.Sockets;
using System.Text;

namespace EchoServer;

public class EchoServer
{
    private Socket? _handler;
    private NetworkStream? _stream;
    public string? Data { get; private set; } = string.Empty;

    public EchoServer()
    {
        Console.WriteLine("Initializing EchoServer.");
    }

    public void RunServer(Socket listener)
    {
        StartServer(listener);
        Console.WriteLine("Stream created, beginning Echo...");
        Echo();
    }

    public void StartServer(Socket listener)
    {
        try
        {
            Console.WriteLine("Accepting connections...");
            _handler = listener.Accept();
            Console.WriteLine("Connection Accepted! Opening stream...");
            _stream = new NetworkStream(_handler);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    
    private void Echo()
    {
        string rawData = ReadStream();
        Data = ProcessRawData(rawData);
        string echoString = AssembleEchoString();
        WriteStream(echoString);
        CloseHandler();
        
    }

    private string ReadStream()
    {
        string result = string.Empty;
        do
        {
            byte[] buffer = new byte[16];
            int bytesRead = _stream!.Read(buffer, 0, buffer.Length);
            result += Encoding.UTF8.GetString(buffer, 0, bytesRead);
        } while (_stream.DataAvailable);


        return result;
    }

    private string ProcessRawData(string rawData)
    {
        string[] resultArray = rawData.Split("\r\n");
        string result = resultArray.Last();

        return result;
    }

    private string AssembleEchoString()
    {
        int contentLength = Data!.Length;
        DateTime now = DateTime.Now;
        string httpNow = now.ToUniversalTime().ToString("r"); 
        
        string result = @$"HTTP/1.1 200 OK
Date: {httpNow}
Content-Length: {contentLength}
Content-Type: text/html
Connection: Closed

{Data}";
        Console.WriteLine("ENTIRE MSG TO SEND:");
        Console.WriteLine(result);
       return result;
    }

    private void WriteStream(string stringToWrite)
    {
        byte[] msg = Encoding.ASCII.GetBytes(stringToWrite);
        _stream!.Write(msg, 0, msg.Length);
    }

    public void CloseHandler()
    {
        _stream!.Dispose();
        _handler!.Shutdown(SocketShutdown.Both);
        _handler.Close();
    }

    public bool StreamConnected()
    {
        return (_stream!.CanRead && _stream.CanWrite);
    }
}