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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e) => gamersTableAdapter.Fill(dbDataSet.Gamers);

        private void Form5_FormClosed(object sender, FormClosedEventArgs e) => (Owner as Form3).AchivUnlock();

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0) gamersTableAdapter.Fill(dbDataSet.Gamers);
            else gamesTableAdapter.Fill(dbDataSet.Games);
        }

        private void TabControl1_SizeChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns[0].Width = 30;
            dataGridView2.Columns[0].Width = 30;
        }
    }
}
