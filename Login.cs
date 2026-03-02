using System;
using System.Windows.Forms;

namespace SuperMarketManagment
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Ju lutem plotësoni të gjitha fushat!", "Gabim", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string correctUsername = "admin";
            string correctPassword = "123";

            if (textBox1.Text == correctUsername && textBox2.Text == correctPassword)
            {
                MessageBox.Show("Login i suksesshëm!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Market m = new Market();
                this.Hide();
                m.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Username ose password gabim!", "Gabim", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

