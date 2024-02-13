using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace _01_02_P_datagridview_Dorofeeva
{
    public partial class Form1 : Form
    {
        NpgsqlConnection connection = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=123456789;Database=test_repo");
        DataTable dt = new DataTable();

        public Form1()
        {
            InitializeComponent();
            LoadData();
            dataGridView1.ReadOnly = true;
        }

        public void LoadData()
        {
            //Использование using для управления подключением к базе данных.
            //Это гарантирует, что подключение будет закрыто после использования.
            using (NpgsqlCommand requestOutput = new NpgsqlCommand("SELECT * FROM products ORDER BY id", connection))
            {
                using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(requestOutput))
                {
                    dt.Clear();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    MessageBox.Show("Fields should not be empty!");
                    return;
                }
                else
                {
                    using (NpgsqlCommand requestInsert = new NpgsqlCommand("INSERT INTO products(id, name, describe) VALUES (@id, @name, @describe)", connection))
                    {
                        requestInsert.Parameters.AddWithValue("@id", textBox1.Text);
                        requestInsert.Parameters.AddWithValue("@name", textBox2.Text);
                        requestInsert.Parameters.AddWithValue("@describe", textBox3.Text);

                        connection.Open();
                        requestInsert.ExecuteNonQuery();
                    }

                    MessageBox.Show("Successfully added!");

                    // Обновить таблицу
                    LoadData();

                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();    
                }

            
                
            }
            catch
            {
                MessageBox.Show("Unkown error!");
                return;
            }
            finally
            {
                connection.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Field should not be empty!");
                    return;
                }
                else
                {
                    using (NpgsqlCommand commandGetId = new NpgsqlCommand("SELECT name FROM products WHERE id = @id", connection))
                    {
                        commandGetId.Parameters.AddWithValue("@id", textBox4.Text);
                        connection.Open();
                        commandGetId.ExecuteNonQuery();

                        if (commandGetId.ExecuteScalar() == null)
                        {
                            MessageBox.Show("ID not found");
                            return;
                        }
                        else
                        {
                            using (NpgsqlCommand requestDelete = new NpgsqlCommand("DELETE FROM products WHERE id = @id", connection))
                            {
                                requestDelete.Parameters.AddWithValue("@id", textBox4.Text);
                                requestDelete.ExecuteNonQuery();
                            }
                            MessageBox.Show("Successfully deleted!");
                            // Обновить таблицу
                            LoadData();
                            textBox4.Clear();
                        }
                    }
                }                
            }
            catch
            {
                MessageBox.Show("Unkown error!");
            }
            finally
            {
                connection.Close();
            }
        }

        private void Update_button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox6.Text) || string.IsNullOrWhiteSpace(textBox7.Text) || string.IsNullOrWhiteSpace(textBox5.Text))
                {
                    MessageBox.Show("Fields should not be empty!");
                    return;
                }
                else
                {
                    // Проверить уникальность id
                    using (NpgsqlCommand commandGetId = new NpgsqlCommand("SELECT id FROM products WHERE id = @id AND id <> @currentId", connection))
                    {
                        commandGetId.Parameters.AddWithValue("@id", textBox6.Text);
                        commandGetId.Parameters.AddWithValue("@currentId", textBox6.Text);
                        connection.Open();
                        if (commandGetId.ExecuteScalar() != null)
                        {
                            MessageBox.Show("ID should be unique!");
                            return;
                        }
                    }
                }
                using (NpgsqlCommand requestUpdate = new NpgsqlCommand("UPDATE products SET id = @id, name = @name, describe = @describe WHERE id = @id", connection))
                {
                     requestUpdate.Parameters.AddWithValue("@id", textBox6.Text);
                     requestUpdate.Parameters.AddWithValue("@name", textBox7.Text);
                     requestUpdate.Parameters.AddWithValue("@describe", textBox5.Text);
                     connection.Open();
                     requestUpdate.ExecuteNonQuery();
                }

                MessageBox.Show("Successfully updaded!");
                // Обновить таблицу
                LoadData();
                textBox6.Clear();
                textBox7.Clear();
                textBox5.Clear();
                              
            }
            catch
            {
                MessageBox.Show("Unkown error!");
            }
            finally
            {
                connection.Close();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Извлечение данных из выбранной строки
                string id = row.Cells["id"].Value.ToString();
                string name = row.Cells["name"].Value.ToString();
                string describe = row.Cells["describe"].Value.ToString();

                // Вывод данных в текстовые поля
                textBox6.Text = id;
                textBox7.Text = name;
                textBox5.Text = describe;
            }
        }
    }     
}



