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
    public partial class Home_Form2 : Form
    {
        private string login_Form1;
        public Home_Form2(string log)
        {
            InitializeComponent();
            login_Form1 = log;
        }

        private void Home_Form2_Load(object sender, EventArgs e)
        {
            label1.Text= login_Form1;
        }
    }
}
