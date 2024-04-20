using System.Net;
using System.Net.Sockets;
using System.Text;

MyUdpListener udpListener = new MyUdpListener();
udpListener.Start();

class MyUdpListener
{
    public void Start()
    {
        // Set the TcpListener on port 13000.
        Int32 port = 4000;
        IPAddress localAddr = IPAddress.Parse("127.0.0.1");

        UdpClient listener = new UdpClient(port);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, port);


        // Enter the listening loop.
        bool keepProcessing = true;
        while(keepProcessing)
        {
            try
            {
                Console.WriteLine($"Waiting for a udp packet to be sent to {localAddr}/{GetPublicIP()} :: {port} ");
                byte[] receivedBytes = listener.Receive(ref groupEP);
                string receivedMessage = Encoding.GetEncoding("latin1").GetString(receivedBytes, 0, receivedBytes.Length);
                Console.WriteLine($"Received message from {groupEP}: {receivedMessage}");
            }
            catch(SocketException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                keepProcessing = false;
                listener.Close();
            }
            finally
            {
            }
        }

        Console.WriteLine("\nHit enter to continue...");
        Console.Read();
    }

    public static string GetPublicIP()
    {
        string url = "http://checkip.dyndns.org";
        System.Net.WebRequest req = System.Net.WebRequest.Create(url);
        System.Net.WebResponse resp = req.GetResponse();
        System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
        string response = sr.ReadToEnd().Trim();
        string[] a = response.Split(':');
        string a2 = a[1].Substring(1);
        string[] a3 = a2.Split('<');
        string a4 = a3[0];
        return a4;
    }
}