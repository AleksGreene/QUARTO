using System;
using System.Windows.Forms;

namespace Quarto
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            textBox1.SetWatermark("Имя игрока");
            textBox2.SetWatermark("Пароль для входа");
            textBox3.SetWatermark("Повторите пароль");
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e) => Owner.Enabled = true;
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int)Keys.Space) e.Handled = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                errorProvider1.SetError(textBox1, "Введите имя игрока");
            else if (queriesTableAdapter.Login(textBox1.Text) > 0)
                errorProvider1.SetError(textBox1, "Игрок с таким именем уже есть");
            else if (textBox2.Text == "")
                errorProvider1.SetError(textBox2, "Введите пароль");
            else if (textBox3.Text == "" || textBox2.Text != textBox3.Text)
                errorProvider1.SetError(textBox3, "Повторите введеный пароль");
            else
            {
                queriesTableAdapter.InputPlayer(textBox1.Text, textBox2.Text);
                Close();
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e) => errorProvider1.Clear();
    }
}
