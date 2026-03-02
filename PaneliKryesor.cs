using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SuperMarketManagment
{
    public partial class PaneliKryesor : Form
    {
        public PaneliKryesor()
        {
            InitializeComponent();
        }

        private void PaneliKryesor_Load(object sender, EventArgs e)
        {
            display();
            display1();
            display2();
        }
        private void display()
        {
            SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=marketdb;Integrated Security=True");
            con.Open();
            SqlCommand cnn = new SqlCommand("select count (*) from producttab", con);
            Int32 count = Convert.ToInt32(cnn.ExecuteScalar());
            if (count > 0)
            {
                lblCount.Text = Convert.ToString(count.ToString());
            }
            else
            {
                lblCount.Text = "0";
            }

            con.Close();

        }

        private void display1()
        {
            SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=marketdb;Integrated Security=True");
            con.Open();
            SqlCommand cnn = new SqlCommand("select count (*) from klienttab", con);
            Int32 count = Convert.ToInt32(cnn.ExecuteScalar());
            if (count > 0)
            {
                lblCount1.Text = Convert.ToString(count.ToString());
            }
            else
            {
                lblCount1.Text = "0";
            }

            con.Close();

        }
        private void display2()
        {
            SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=marketdb;Integrated Security=True");
            con.Open();
            SqlCommand cnn = new SqlCommand("select count (*) from porosi", con);
            Int32 count = Convert.ToInt32(cnn.ExecuteScalar());
            if (count > 0)
            {
                lblCount2.Text = Convert.ToString(count.ToString());
            }
            else
            {
                lblCount2.Text = "0";
            }

            con.Close();
        }
    }
}
