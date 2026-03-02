using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SuperMarketManagment
{
    public partial class Klient : Form
    {
        public Klient()
        {
            InitializeComponent();
        }

        // Metodë për të rifreskuar DataGridView
        private void RefreshDataGridView()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=marketdb;Integrated Security=True"))
                {
                   
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM klienttab", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gabim gjatë rifreskimit: " + ex.Message, "Gabim",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Metodë për të kontrolluar nëse tekstbox-at janë të vlefshëm
        private bool ValidoniFushat()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Ju lutem plotësoni të gjitha fushat!", "Paralajmërim",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(textBox1.Text, out _))
            {
                MessageBox.Show("KlientID duhet të jetë numër i vlefshëm!", "Gabim",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        // SAVE BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidoniFushat()) return;

            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=marketdb;Integrated Security=True"))
                {
                    con.Open();

                    SqlCommand cnn = new SqlCommand("INSERT INTO klienttab VALUES(@klientid, @emriklientit, @phone, @adresa)", con);
                    cnn.Parameters.AddWithValue("@klientid", int.Parse(textBox1.Text));
                    cnn.Parameters.AddWithValue("@emriklientit", textBox2.Text);
                    cnn.Parameters.AddWithValue("@phone", textBox3.Text);
                    cnn.Parameters.AddWithValue("@adresa", textBox4.Text);

                    cnn.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Klienti u shtua me sukses!", "Sukses",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);

                    RefreshDataGridView();
                    button4_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gabim gjatë shtimit: " + ex.Message, "Gabim",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // UPDATE BUTTON 
        private void button2_Click(object sender, EventArgs e)
        {
            if (!ValidoniFushat()) return;

            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=marketdb;Integrated Security=True"))
                {
                    con.Open();

                   
                    SqlCommand cnn = new SqlCommand("UPDATE klienttab SET emriklientit=@emriklientit, phone=@phone, adresa=@adresa WHERE klientid=@klientid", con);

                    cnn.Parameters.AddWithValue("@klientid", int.Parse(textBox1.Text));
                    cnn.Parameters.AddWithValue("@emriklientit", textBox2.Text);
                    cnn.Parameters.AddWithValue("@phone", textBox3.Text);
                    cnn.Parameters.AddWithValue("@adresa", textBox4.Text);

                    int rowsAffected = cnn.ExecuteNonQuery();
                    con.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Klienti u ndryshua me sukses!", "Sukses",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGridView();
                        button4_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show($"Nuk u gjet asnjë klient me ID={textBox1.Text}", "Informacion",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gabim gjatë ndryshimit: " + ex.Message, "Gabim",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // DELETE BUTTON 
        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Ju lutem shkruani KlientID për të fshirë!", "Paralajmërim",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBox1.Text, out int id))
            {
                MessageBox.Show("KlientID duhet të jetë numër i vlefshëm!", "Gabim",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show($"Jeni të sigurt që doni të fshini klientin me ID={id}?",
                                                  "Konfirmo fshirjen",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=marketdb;Integrated Security=True"))
                    {
                        con.Open();

                        
                        SqlCommand cnn = new SqlCommand("DELETE FROM klienttab WHERE klientid = @id", con);
                        cnn.Parameters.AddWithValue("@id", id);

                        int rowsAffected = cnn.ExecuteNonQuery();
                        con.Close();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Klienti u fshi me sukses!", "Sukses",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshDataGridView();
                            button4_Click(sender, e);
                        }
                        else
                        {
                            MessageBox.Show($"Nuk u gjet asnjë klient me ID={id}", "Informacion",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gabim gjatë fshirjes: " + ex.Message, "Gabim",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // NEW BUTTON
        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        // LOAD FORM
        private void Klient_Load(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        // Klikim në DataGridView për të mbushur fushat
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["klientid"].Value.ToString();
                textBox2.Text = row.Cells["emriklientit"].Value.ToString();
                textBox3.Text = row.Cells["phone"].Value.ToString();
                textBox4.Text = row.Cells["adresa"].Value.ToString();
            }
        }
    }
}