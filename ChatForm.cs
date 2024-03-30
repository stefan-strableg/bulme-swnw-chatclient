using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatClient
{

    public partial class ChatForm : Form
    {
        Server? server;
        Client client = new Client();

        IPAddress serverIp = IPAddress.Parse("127.0.0.1");
        int port = 42069;

        public ChatForm()
        {
            InitializeComponent();
            ReevaluateUI();
        }

        public void printMessage(String message)
        {
            listView_messages.Items.Add(message);
        }


        public static Tuple<IPAddress, int> CheckForServer()
        {
            const int port = 25777;
            using (UdpClient udpClient = new UdpClient())
            {
                udpClient.EnableBroadcast = true;
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpClient.Client.ReceiveTimeout = 500;

                byte[] broadcastMessage = Encoding.ASCII.GetBytes("DISCOVER_SERVER");

                udpClient.Send(broadcastMessage, broadcastMessage.Length, new IPEndPoint(IPAddress.Broadcast, port));
                Debug.WriteLine("Sent Discover Packet");

                byte[] responseData;

                IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
                try
                {
                    responseData = udpClient.Receive(ref remoteEndpoint);
                }
                catch (Exception ex)
                {
                    return null;
                }

                Debug.WriteLine("Received Response");

                string responseString = Encoding.ASCII.GetString(responseData);
                string[] responseParts = responseString.Split(':');

                if (responseParts.Length == 2 && IPAddress.TryParse(responseParts[0], out IPAddress ipAddress) && int.TryParse(responseParts[1], out int discoveredPort))
                {
                    return Tuple.Create(ipAddress, discoveredPort);
                }
            }

            return null;
        }

        private void Disconnect()
        {
            if (server != null)
            {
                server.Broadcast("\\disc");
                server.Stop();
                server = null;
            }

            client.Disconnect();

            ReevaluateUI();

            printMessage("Disconnected");
        }

        private void Connect()
        {
            listView_messages.Clear();

            if (checkBox_autohost.Checked)
            {
                Tuple<IPAddress, int> ip = CheckForServer();
                if (ip == null)
                {
                    printMessage("No hosts discovered, starting server...");

                    server = new Server(port);

                    serverIp = IPAddress.Parse("127.0.0.1");

                    server.Start();
                }
                else
                {
                    serverIp = ip.Item1;
                    port = ip.Item2;
                }
            }
            else if (checkBox_host.Checked)
            {
                server = new Server(port);

                serverIp = IPAddress.Parse("127.0.0.1");

                server.Start();
            }
            else if (!IPAddress.TryParse(textBox_ip.Text, out serverIp))
            {
                Disconnect();
                printMessage("Invalid ip address: " + textBox_ip.Text);
                return;
            }


            if (!client.Connect(serverIp, port))
            {
                Disconnect();
                printMessage("Failed to connect to " + serverIp.ToString() + ":" + port);
                return;
            }

            client.onReceive = (message) => { lock (listView_messages) { Invoke(new Action<string>(printMessage), message); } };
            client.onDisconnect = () => { Disconnect(); };
            printMessage("Connected to " + serverIp.ToString() + ":" + port + ((server != null && server.isRunning) ? " - You are hosting" : ""));
        }


        private void button_connect_Click(object sender, EventArgs e)
        {
            if (!client.isConnected)
            {
                Connect();
            }
            else
            {
                Disconnect();
            }
            ReevaluateUI();
        }

        private void sendMessage()
        {
            client.SendMessage(textBox_message.Text);
            textBox_message.Clear();
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            sendMessage();
        }

        private void textBox_message_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sendMessage();
            }
        }

        private void checkBox_host_CheckedChanged(object sender, EventArgs e)
        {
            ReevaluateUI();
        }

        private void textBox_ip_TextChanged(object sender, EventArgs e)
        {
            IPAddress.TryParse(textBox_ip.Text, out serverIp);
        }

        private void textBox_port_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(textBox_port.Text, out port);
        }

        private void checkBox_autohost_CheckedChanged(object sender, EventArgs e)
        {
            ReevaluateUI();
        }

        private void ReevaluateUI()
        {
            button_send.Enabled = client.isConnected;
            label_status.Enabled = client.isConnected;

            if (!client.isConnected)
                button_connect.Text = checkBox_autohost.Checked ? "Connect / Host" : (checkBox_host.Checked ? "Host" : "Connect");
            else
                button_connect.Text = (server != null && server.isRunning) ? "Stop hosting" : "Disconnect";

            textBox_ip.Enabled = !client.isConnected && !checkBox_autohost.Checked && !checkBox_host.Checked;
            textBox_port.Enabled = !client.isConnected;
            checkBox_host.Enabled = !client.isConnected && !checkBox_autohost.Checked;
            checkBox_autohost.Enabled = !client.isConnected;

            if (server != null && server.isRunning)
                label_status.Text = "Hosting on port " + port;
            else if (client.isConnected)
                label_status.Text = "Connected to " + serverIp.ToString() + ":" + port;
            else
                label_status.Text = "Not connected";
        }
    }
}