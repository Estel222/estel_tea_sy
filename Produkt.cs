using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SuperMarketManagment
{
    public partial class Produkt : Form
    {
        public Produkt()
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
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM producttab", con);
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
                MessageBox.Show("ProductID duhet të jetë numër i vlefshëm!", "Gabim",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Kontrollo nëse çmimi është numër
            if (!decimal.TryParse(textBox4.Text, out _))
            {
                MessageBox.Show("Çmimi duhet të jetë numër i vlefshëm!", "Gabim",
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

                    SqlCommand cnn = new SqlCommand("INSERT INTO producttab VALUES(@productid, @productname, @category, @price)", con);
                    cnn.Parameters.AddWithValue("@productid", int.Parse(textBox1.Text));
                    cnn.Parameters.AddWithValue("@productname", textBox2.Text);
                    cnn.Parameters.AddWithValue("@category", textBox3.Text);
                    cnn.Parameters.AddWithValue("@price", decimal.Parse(textBox4.Text));

                    cnn.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Produkti u shtua me sukses!", "Sukses",
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

                    // KORRIGJUAR: Shtuar WHERE dhe përdorur emrat e saktë të kolonave
                    SqlCommand cnn = new SqlCommand("UPDATE producttab SET productname=@productname, category=@category, price=@price WHERE productid=@productid", con);

                    cnn.Parameters.AddWithValue("@productid", int.Parse(textBox1.Text));
                    cnn.Parameters.AddWithValue("@productname", textBox2.Text);
                    cnn.Parameters.AddWithValue("@category", textBox3.Text);
                    cnn.Parameters.AddWithValue("@price", decimal.Parse(textBox4.Text));

                    int rowsAffected = cnn.ExecuteNonQuery();
                    con.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Produkti u ndryshua me sukses!", "Sukses",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGridView();
                        button4_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show($"Nuk u gjet asnjë produkt me ID={textBox1.Text}", "Informacion",
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
                MessageBox.Show("Ju lutem shkruani ProductID për të fshirë!", "Paralajmërim",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBox1.Text, out int id))
            {
                MessageBox.Show("ProductID duhet të jetë numër i vlefshëm!", "Gabim",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show($"Jeni të sigurt që doni të fshini produktin me ID={id}?",
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

                        // Fshij fillimisht nga tabelat e tjera (nëse ka foreign key)
                        SqlCommand cmdInventar = new SqlCommand("DELETE FROM inventar WHERE productid = @id", con);
                        cmdInventar.Parameters.AddWithValue("@id", id);
                        cmdInventar.ExecuteNonQuery();

                        SqlCommand cmdPorosi = new SqlCommand("DELETE FROM porosi WHERE productid = @id", con);
                        cmdPorosi.Parameters.AddWithValue("@id", id);
                        cmdPorosi.ExecuteNonQuery();

                        // Tani fshij produktin
                        SqlCommand cnn = new SqlCommand("DELETE FROM producttab WHERE productid = @id", con);
                        cnn.Parameters.AddWithValue("@id", id);

                        int rowsAffected = cnn.ExecuteNonQuery();
                        con.Close();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Produkti u fshi me sukses!", "Sukses",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshDataGridView();
                            button4_Click(sender, e);
                        }
                        else
                        {
                            MessageBox.Show($"Nuk u gjet asnjë produkt me ID={id}", "Informacion",
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
        private void Produkt_Load(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        // Shto event për klikim në DataGridView
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["productid"].Value.ToString();
                textBox2.Text = row.Cells["productname"].Value.ToString();
                textBox3.Text = row.Cells["category"].Value.ToString();
                textBox4.Text = row.Cells["price"].Value.ToString();
            }
        }
    }
}