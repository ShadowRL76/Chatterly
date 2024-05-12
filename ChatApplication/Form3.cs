using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace ChatApplication
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }


        // Register account button
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            string repassword = textBox3.Text;
            string email = textBox4.Text;

            if (password != repassword)
            {
                MessageBox.Show("Passwords must be the same!");
            }
            else
            {
                try
                {
                    bool registrationResult = RegisterAccountForm(username, password, email);
                    
                }
                catch
                {
                    MessageBox.Show("Error creating account");
                }

            }
        }

        
        public bool RegisterAccountForm(string username, string password, string email)
        {
            try
            {
                RegisterAccount register = new RegisterAccount(username, password, email);
                this.Hide();
                Form1 form1 = new Form1();
                form1.Show();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating Account: " + ex.Message);
                return false;
            }
        }
        
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
