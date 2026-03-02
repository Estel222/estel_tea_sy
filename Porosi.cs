using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SuperMarketManagment
{
    public partial class Porosi : Form
    {
        public Porosi()
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
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM porosi", con);
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

        // SAVE BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=marketdb;Integrated Security=True"))
                {
                    con.Open();

                    SqlCommand cnn = new SqlCommand("INSERT INTO porosi VALUES(@orderid, @customerid, @productid, @quantity, @amount)", con);
                    cnn.Parameters.AddWithValue("@orderid", int.Parse(textBox1.Text));
                    cnn.Parameters.AddWithValue("@customerid", int.Parse(textBox2.Text));
                    cnn.Parameters.AddWithValue("@productid", int.Parse(textBox3.Text));
                    cnn.Parameters.AddWithValue("@quantity", textBox4.Text);
                    cnn.Parameters.AddWithValue("@amount", textBox5.Text);

                    cnn.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Porosia u shtua me sukses!", "Sukses",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // RIFRESKO DataGridView
                    RefreshDataGridView();

                    // Pastro fushat
                    button4_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gabim gjatë shtimit: " + ex.Message, "Gabim",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // UPDATE BUTTON - KORRIGJUAR
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=marketdb;Integrated Security=True"))
                {
                    con.Open();

                    // KORRIGJUAR: Shtuar WHERE dhe presjet e duhura
                    SqlCommand cnn = new SqlCommand("UPDATE porosi SET customerid=@customerid, productid=@productid, quantity=@quantity, amount=@amount WHERE orderid=@orderid", con);

                    cnn.Parameters.AddWithValue("@orderid", int.Parse(textBox1.Text));
                    cnn.Parameters.AddWithValue("@customerid", int.Parse(textBox2.Text));
                    cnn.Parameters.AddWithValue("@productid", int.Parse(textBox3.Text));
                    cnn.Parameters.AddWithValue("@quantity", textBox4.Text);
                    cnn.Parameters.AddWithValue("@amount", textBox5.Text);

                    int rowsAffected = cnn.ExecuteNonQuery();
                    con.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Porosia u ndryshua me sukses!", "Sukses",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // RIFRESKO DataGridView
                        RefreshDataGridView();

                        // Pastro fushat
                        button4_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show($"Nuk u gjet asnjë porosi me ID={textBox1.Text}", "Informacion",
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
                MessageBox.Show("Ju lutem shkruani PorosiID për të fshirë!", "Paralajmërim",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBox1.Text, out int id))
            {
                MessageBox.Show("PorosiID duhet të jetë numër i vlefshëm!", "Gabim",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show($"Jeni të sigurt që doni të fshini porosinë me ID={id}?",
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
                        SqlCommand cnn = new SqlCommand("DELETE FROM porosi WHERE orderid=@id", con);
                        cnn.Parameters.AddWithValue("@id", id);

                        int rowsAffected = cnn.ExecuteNonQuery();
                        con.Close();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Porosia u fshi me sukses!", "Sukses",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // RIFRESKO DataGridView
                            RefreshDataGridView();

                            // Pastro fushat
                            button4_Click(sender, e);
                        }
                        else
                        {
                            MessageBox.Show($"Nuk u gjet asnjë porosi me ID={id}", "Informacion",
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

        // NEW BUTTON (Pastro fushat)
        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        // LOAD FORM
        private void Porosi_Load(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        // Klikim në DataGridView për të mbushur fushat
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["orderid"].Value.ToString();
                textBox2.Text = row.Cells["customerid"].Value.ToString();
                textBox3.Text = row.Cells["productid"].Value.ToString();
                textBox4.Text = row.Cells["quantity"].Value.ToString();
                textBox5.Text = row.Cells["amount"].Value.ToString();
            }
        }
    }
}