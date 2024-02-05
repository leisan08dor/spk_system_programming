using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20._01_pdf_dorofeeva_
{
    public partial class Form1 : Form
    {
        // создали экземпляры объектов, с к-ми дальше взаимодействуем
        PrintDocument document = new PrintDocument();
        PrintDialog dialog = new PrintDialog();

        public Form1()
        {
            InitializeComponent();
            // явно указали в обработчике событий печати,что печатать необходимо метод document_PrintPage
            printDocument1.PrintPage += new PrintPageEventHandler(document_PrintPage);
        }

        void document_PrintPage(object sender, PrintPageEventArgs e)
        {
            // метод Graphics.DrawString создает заданную текстовую строку в указанном месте на странице и формате 
            e.Graphics.DrawString(textBox1.Text, new Font("Times New Roman", 14, FontStyle.Regular), Brushes.Black, 20, 20);
        }

        // обработчик событий кнопки
        private void button1_Click(object sender, EventArgs e)
        {
            // вызвали диалог печати документа
            dialog.Document = document;
            // при подтверждении, файл отправляется в печать
            if (printDialog1.ShowDialog() == DialogResult.OK)
                printDocument1.Print();
        }
    }
}
