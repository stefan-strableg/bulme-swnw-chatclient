using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    public class Client
    {
        public delegate void OnReceiveDelegate(string message);
        public delegate void OnDisconnectDelegate();

        public OnReceiveDelegate? onReceive;
        public OnDisconnectDelegate? onDisconnect;

        public bool isConnected;
        private TcpClient client;
        private NetworkStream stream;

        public bool Connect(IPAddress serverIP, int port)
        {
            client = new TcpClient();
            try
            {
                client.Connect(serverIP, port);
            }
            catch (SocketException ex) 
            {
                return false;
            }
            stream = client.GetStream();
            isConnected = true;

            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();
            return true;
        }

        public void SendMessage(string message)
        {
            if (!isConnected)
            {
                Debug.WriteLine("Not connected to server.");
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
            Debug.WriteLine("Message sent.");
        }

        private void ReceiveMessages()
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while (isConnected)
            {
                try
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                        Debug.WriteLine("Received: " + receivedMessage);

                        if (receivedMessage[0] == '\\')
                        {
                            if (receivedMessage.StartsWith("\\disc"))
                            {
                                if (onDisconnect != null)
                                    onDisconnect();
                            }
                        }
                        else if (onReceive != null)
                            onReceive(receivedMessage);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error receiving message: " + ex.Message);
                    isConnected = false;
                }
            }
        }

        public void Disconnect()
        {
            if (isConnected)
            {
                isConnected = false;
                stream.Close();
                client.Close();
            }
        }
    }


}
