using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quarto
{
    public partial class Form2 : Form
    {
        private Random rnd = new Random();
        bool flag = true;

        public Form2()
        {
            InitializeComponent();

            label1.Text = "Введите любое число от 0 до 9 включительно.\n" + 
                "Ваш соперник уже загадал число (N) из этого\n" +
                "же диапазона. Если сумма ваших чисел будет\n" +
                "четным числом, то первым ход делаете Вы,\n" +
                "иначе - ваш соперник Компьютер!";
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e) => Owner.Enabled = true;

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) ) e.Handled = true;
            else textBox1.Text = "";

            if (e.KeyChar == (char)Keys.Enter) Button1_Click(this, new EventArgs());
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (flag)
                {
                    int a = Int32.Parse(textBox1.Text),
                        b = rnd.Next(10);
                    label2.Text = "+" + b.ToString() + "=" + (a + b).ToString();
                    if ((a + b) % 2 == 0)
                    {
                        label3.Text = "Поздравляем! Ваш ход первый.";
                        (Owner.Owner as Form1).turn = true;
                    }
                    else
                    {
                        label3.Text = "Первым ход делает Компьютер.";
                        (Owner.Owner as Form1).turn = false;
                    }
                    flag = false;
                }
                else
                {
                    while (--Owner.Height > 1) ;
                    (Owner as Form3).Button_Continue();
                    Owner.Owner.Activate();
                    (Owner.Owner as Form1).StartGame();
                    Close();
                }
            }
        }
    }
}
