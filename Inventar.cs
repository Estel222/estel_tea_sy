using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SuperMarketManagment
{
    public partial class Inventar : Form
    {
        public Inventar()
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
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM inventar", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gabim gjatë rifreskimit: " + ex.Message, "Gabim", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Metodë për të kontrolluar nëse tekstbox-at janë të vlefshëm
        private bool ValidoniFushat()
        {
            // Kontrollo nëse janë bosh
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Ju lutem plotësoni të gjitha fushat!", "Paralajmërim",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Kontrollo nëse ID është numër
            if (!int.TryParse(textBox1.Text, out _))
            {
                MessageBox.Show("ID duhet të jetë numër i vlefshëm!", "Gabim",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Kontrollo nëse ProductID është numër
            if (!int.TryParse(textBox2.Text, out _))
            {
                MessageBox.Show("ProductID duhet të jetë numër i vlefshëm!", "Gabim",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Kontrollo nëse StockAdded është numër
            if (!int.TryParse(textBox3.Text, out _))
            {
                MessageBox.Show("Sasia e shtuar duhet të jetë numër i vlefshëm!", "Gabim",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Kontrollo nëse StockRemoved është numër
            if (!int.TryParse(textBox4.Text, out _))
            {
                MessageBox.Show("Sasia e hequr duhet të jetë numër i vlefshëm!", "Gabim",
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

                    SqlCommand cnn = new SqlCommand("INSERT INTO inventar VALUES(@id, @productid, @stockadded, @stockremoved)", con);
                    cnn.Parameters.AddWithValue("@id", int.Parse(textBox1.Text));
                    cnn.Parameters.AddWithValue("@productid", int.Parse(textBox2.Text));
                    cnn.Parameters.AddWithValue("@stockadded", int.Parse(textBox3.Text));
                    cnn.Parameters.AddWithValue("@stockremoved", int.Parse(textBox4.Text));

                    cnn.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Rekordi u shtua me sukses!", "Sukses",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);

                    RefreshDataGridView();
                    button4_Click(sender, e); // Pastro fushat
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
            if (!ValidoniFushat()) return;

            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=marketdb;Integrated Security=True"))
                {
                    con.Open();

                    // KORRIGJUAR: Komanda e saktë UPDATE
                    SqlCommand cnn = new SqlCommand("UPDATE inventar SET productid=@productid, stockadded=@stockadded, stockremoved=@stockremoved WHERE id=@id", con);

                    cnn.Parameters.AddWithValue("@id", int.Parse(textBox1.Text));
                    cnn.Parameters.AddWithValue("@productid", int.Parse(textBox2.Text));
                    cnn.Parameters.AddWithValue("@stockadded", int.Parse(textBox3.Text));
                    cnn.Parameters.AddWithValue("@stockremoved", int.Parse(textBox4.Text));

                    int rowsAffected = cnn.ExecuteNonQuery();
                    con.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Rekordi u ndryshua me sukses!", "Sukses",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGridView();
                        button4_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("Nuk u gjet asnjë rekord me ID=" + textBox1.Text, "Informacion",
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
                MessageBox.Show("Ju lutem shkruani ID për të fshirë!", "Paralajmërim",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBox1.Text, out int id))
            {
                MessageBox.Show("ID duhet të jetë numër i vlefshëm!", "Gabim",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show($"Jeni të sigurt që doni të fshini rekordin me ID={id}?",
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
                        SqlCommand cnn = new SqlCommand("DELETE FROM inventar WHERE id=@id", con);
                        cnn.Parameters.AddWithValue("@id", id);

                        int rowsAffected = cnn.ExecuteNonQuery();
                        con.Close();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Rekordi u fshi me sukses!", "Sukses",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshDataGridView();
                            button4_Click(sender, e);
                        }
                        else
                        {
                            MessageBox.Show($"Nuk u gjet asnjë rekord me ID={id}", "Informacion",
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
        }

        // LOAD FORM
        private void Inventar_Load(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        // Shto event për klikim në DataGridView
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["id"].Value.ToString();
                textBox2.Text = row.Cells["productid"].Value.ToString();
                textBox3.Text = row.Cells["stockadded"].Value.ToString();
                textBox4.Text = row.Cells["stockremoved"].Value.ToString();
            }
        }
    }
}