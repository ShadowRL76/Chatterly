using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ChatApplication
{
    internal class Client
    {
        private byte[] buffer = new byte[1024];
        private Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static List<ClientInfo> connectedClients = new List<ClientInfo>();
        private ListBox listBox1;

        public Client(ListBox listBox)
        {
            listBox1 = listBox;
        }

        private class ClientInfo
        {
            public Socket Socket { get; set; }
            public string Username { get; set; }
        }

        public void SetText(string text, ListBox listBox)
        {
            if (listBox.InvokeRequired)
            {
                listBox.Invoke(new Action<string, ListBox>(SetText), text, listBox);
            }
            else
            {
                listBox.Items.Add(text);
            }
        }

        public void LoopConnect()
        {
            int attempts = 0;
            string serverIpAddress = "192.168.1.241";

            while (!clientSocket.Connected)
            {
                try
                {
                    attempts++;
                    clientSocket.Connect(serverIpAddress, 8080);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"Connection attempt {attempts} failed: {ex.Message}");
                }
            }

            connectedClients.Add(new ClientInfo { Socket = clientSocket, Username = Form1.username });
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), clientSocket);
        }

        public void ReceiveCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            int received = socket.EndReceive(AR);
            byte[] dataBuf = new byte[received];
            Array.Copy(buffer, dataBuf, received);
            string message = Encoding.ASCII.GetString(dataBuf);

            SetText(message, listBox1);

            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
        }

        public void MessageToSend(TextBox textBox2, ListBox listBox)
        {
            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                string messageToSend = $"[{Form1.username}] {textBox2.Text}";
                byte[] buffer = Encoding.ASCII.GetBytes(messageToSend);

                try
                {
                    clientSocket.Send(buffer);
                    textBox2.Clear();
                    listBox.Items.Add(messageToSend);
                }
                catch (SocketException ex)
                {
                    MessageBox.Show($"Failed to send message: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Message can't be empty!");
            }
        }

        public void Disconnect()
        {
            if (clientSocket.Connected)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }
    }
}
