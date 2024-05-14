using System;
using System.Windows.Forms;
using System.Drawing;

namespace ChatApplication
{
    public partial class Form2 : Form
    {
        private Client client;

        public Form2()
        {
            InitializeComponent();
            this.Text = "Client";

            client = new Client(listBox1);  
            client.LoopConnect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.MessageToSend(textBox2, listBox1);
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
            client.Disconnect();  
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
