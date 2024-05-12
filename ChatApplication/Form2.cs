﻿using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing;
using System.Collections.Generic;

namespace ChatApplication
{
    public partial class Form2 : Form
    {
        private static byte[] buffer = new byte[1024];
        private static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private string message = "";
        private List<ClientInfo> connectedClients = new List<ClientInfo>();

        public Form2()
        {
            InitializeComponent();
            this.Text = "Client";
            LoopConnect();
        }

        public class ClientInfo
        {
            public Socket Socket { get; set; }
            public string Username { get; set; }
        }

        private void SetText(string text)
        {
            if (listBox1.InvokeRequired)
            {
                Action<string> d = new Action<string>(SetText);
                this.Invoke(d, text);
            }
            else
                listBox1.Items.Add(text);
        }

        private void UpdateConnectedClientsListView()
        {
            listView1.Items.Clear();
            foreach (var client in connectedClients)
            {
                ListViewItem item = new ListViewItem(client.Username);
                item.SubItems.Add(((IPEndPoint)client.Socket.RemoteEndPoint).Address.ToString());
                listView1.Items.Add(item);
            }
        }



        private void LoopConnect()
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
                    // Handle connection errors
                }
            }

            // Add clients to list
            connectedClients.Add(new ClientInfo { Socket = clientSocket, Username = Form1.username });

            // Start receiving messages asynchronously
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), clientSocket);
        }


        private void ReceiveCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            int received = socket.EndReceive(AR);
            byte[] dataBuf = new byte[received];
            Array.Copy(buffer, dataBuf, received);
            string message = Encoding.ASCII.GetString(dataBuf);

            SetText(message);

            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Send an empty buffer to signal the start of a new message
            clientSocket.Send(Encoding.ASCII.GetBytes(Environment.NewLine));

            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                string messageToSend = "[" + Form1.username + "] " + textBox2.Text;
                byte[] buffer = Encoding.ASCII.GetBytes(messageToSend);
                byte[] emptyBuffer = new byte[0];
                clientSocket.Send(emptyBuffer);
                clientSocket.Send(buffer);
                textBox2.Clear();
                listBox1.Items.Add(messageToSend);
            }
        }



        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.ForeColor = Color.White;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (clientSocket.Connected)
                clientSocket.Close();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.Text = Form1.username;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}