using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMarketManagment
{
    public partial class Market : Form
    {
        public Market()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Produkt pd = new Produkt();
            pd.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Klient kt = new Klient();
            kt.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Porosi ps = new Porosi();
            ps.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Inventar iv = new Inventar();
            iv.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PaneliKryesor pk = new PaneliKryesor();
            pk.Show();
        }
    }
}
