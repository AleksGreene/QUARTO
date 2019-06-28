using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quarto
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            textBox1.SetWatermark("Имя игрока");
            textBox2.SetWatermark("Пароль для входа");
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            NullButton.Focus();
            errorProvider1.Clear();
            Form form4 = new Form4 { TopMost = true, Owner = this };
            form4.Show();
            form4.Location = new Point(Location.X + (Width - form4.Width) / 2, Location.Y + (Height - form4.Height) / 2 + 70);
            Enabled = false; 
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            NullButton.Focus();
            Form form2 = new Form2 { TopMost = true, Owner = this };
            form2.Show();
            form2.Location = new Point(Owner.Location.X + (Owner.Width - form2.Width) / 2, Owner.Location.Y + (Owner.Height - form2.Height) / 2);
            Enabled = false;
        }

        public void Button_Continue() => button5.Enabled = true;

        private void Button5_Click(object sender, EventArgs e)
        {
            while (--Height > 1) ;
            Owner.Activate();
            (Owner as Form1).timer1.Start();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (queriesTableAdapter.Login(textBox1.Text) == 0)
                errorProvider1.SetError(textBox1, "Неверный логин");
            else if (queriesTableAdapter.Enter(textBox1.Text, textBox2.Text) == 0)
                errorProvider1.SetError(textBox2, "Неверныйпароль");
            else
            {
                (Owner as Form1).SetName(textBox1.Text);
                button4.Enabled = true;
                textBox1.Text = "";
                textBox2.Text = "";            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e) => errorProvider1.Clear();

        public void ContinueLock() => button5.Enabled = false;
        public void AchivUnlock() => button7.Enabled = true;

        private void Button7_Click(object sender, EventArgs e)
        {
            NullButton.Focus();
            Form form5 = new Form5 { TopMost = true, Owner = this };
            form5.Show();
            form5.Location = new Point(Owner.Location.X + Owner.Width - 8, Owner.Location.Y);
            form5.Height = Owner.Height;
            button7.Enabled = false;
        }
    }
}
