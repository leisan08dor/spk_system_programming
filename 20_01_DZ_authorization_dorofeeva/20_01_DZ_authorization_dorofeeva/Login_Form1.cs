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

namespace _20_01_DZ_authorization_dorofeeva
{
    public partial class Login_Form1 : Form
    {
        private string login = "";
        private string password = "";
        private int count;

        string connectionString = "Server=localhost;" +
        "Port=5432;Database=test_repo;User Id=postgres;Password=123456789;";

        public Login_Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            login = textBox1.Text;
            password = textBox2.Text;
            string getedPassword = "";
            
            if (login.Length == 0 || password.Length == 0)
            {
                MessageBox.Show("Поля не должны быть пустыми!");
                return;
            }

            Program.connection.Open();
            NpgsqlCommand commandGetLogin = new NpgsqlCommand($"SELECT passwords FROM users WHERE logins = '{login}'", Program.connection);
            try
            {
                if (commandGetLogin.ExecuteScalar() == null)
                {
                    MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Program.connection.Close();
                    return;
                }

                getedPassword = commandGetLogin.ExecuteScalar().ToString();
            }
            catch
            {
                MessageBox.Show("Неизвестная шибка");
                Program.connection.Close();
                return;
            }
            Program.connection.Close();
            if (getedPassword != password)
            {
                count++;
                if (count == 3)
                {
                    this.Enabled = false;
                    await Task.Delay(20000);
                    this.Enabled = true;

                }
                MessageBox.Show("Неправильный пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                string log = textBox1.Text;
                Home_Form2 fr2 = new Home_Form2(log);
                fr2.Show();
                MessageBox.Show("Успешный вход!");
                Hide();
            }

        }
    }
}
