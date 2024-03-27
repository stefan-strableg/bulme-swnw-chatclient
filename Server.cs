using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ChatClient
{
    public class Server
    {

        public static int discoveryPort = 25777;
        private TcpListener listener;

        private Thread discoverThread;

        private List<Thread> threads = new List<Thread>();
        private List<TcpClient> clients = new List<TcpClient>();
        public bool isRunning;

        public Server(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            discoverThread = new Thread(DiscoveryServer);
        }

        public void Start()
        {
            listener.Start();
            isRunning = true;
            Debug.WriteLine("[Server] Running...");
            Thread loopThread = new Thread(Loop);
            loopThread.Start();
            discoverThread.Start();
        }

        private void Loop()
        {
            try
            {
                while (isRunning)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    clients.Add(client);

                    Thread clientThread = new Thread(HandleClient);
                    threads.Add(clientThread);

                    clientThread.Start(client);
                }
            } 
            catch (Exception)
            {

            }
        }


        private IPAddress serverIP()
        {
            try
            {
                // Get the local host name
                string hostName = Dns.GetHostName();

                // Get the IP addresses associated with the local host
                IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);

                // Print all the IP addresses associated with the local host
                Debug.WriteLine($"IP Addresses of the local host ({hostName}):");
                foreach (IPAddress address in hostAddresses)
                {
                    if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        return address;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }
            return null;
        }

        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                while (true)
                {
                    if (!isRunning)
                        break;
                    if (!stream.DataAvailable)
                        continue;

                    bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                        break;

                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Debug.WriteLine("[Server] Received: " + message);

                    Broadcast(client.Client.RemoteEndPoint.ToString() + ": " + message);
                }
            }
            finally 
            { 
                stream.Close();
                client.Close();  
                lock(clients)
                {
                    clients.Remove(client);
                }
            }
        }

        public void Broadcast(string message)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message);

            foreach (TcpClient client in clients)
            {
                NetworkStream stream = client.GetStream();
                Debug.WriteLine("[Server] Sent message to client");
                try
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("[Server] Exception: " + ex.Message);
                }
            }
        }

        private void DiscoveryServer()
        {
            try
            {
                using (var udpClient = new UdpClient(discoveryPort))
                {
                    while (isRunning)
                    {
                        if (udpClient.Available == 0)
                            continue;

                        IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);

                        byte[] requestData = udpClient.Receive(ref remoteEndpoint);

                        string requestString = Encoding.ASCII.GetString(requestData);

                        Debug.WriteLine("[Discovery Server] Received request: " + requestString);

                        if (requestString.Trim() == "DISCOVER_SERVER")
                        {
                            IPEndPoint localEndPoint = (IPEndPoint)listener.LocalEndpoint;
                            string response = $"{serverIP()}:{localEndPoint.Port}";
                            Debug.WriteLine("[Discovery Server] Sending response: " + response);
                            byte[] responseData = Encoding.ASCII.GetBytes(response);
                            udpClient.Send(responseData, responseData.Length, remoteEndpoint);
                        }
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"[Discovery Server] Stop, error occurred: {ex.Message}");
            }
        }

        public void Stop()
        {
            isRunning = false;
            listener.Stop();

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            discoverThread.Join();
        }
    }

}
