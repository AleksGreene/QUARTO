using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Quarto
{
    public partial class Form1 : Form
    {
        public class Figurine
        {
            public bool Dark;
            public bool Big;
            public bool Square;
            public bool Point;
            public Figurine(bool dark, bool big, bool square, bool point)
            {
                Dark = dark;
                Big = big;
                Square = square;
                Point = point;
            }
            public Figurine(Figurine copy)
            {
                Dark = copy.Dark;
                Big = copy.Big;
                Square = copy.Square;
                Point = copy.Point;
            }
        }

        private Form menu;
        private Random rnd = new Random();
        private Figurine[,] storage;
        private Figurine[,] board;
        private readonly int size = 60;

        public bool turn; //первый ход игрока
        private DateTime time;
        private int step;

        private PictureBox[,] pictureBox;
        private PictureBox[,] borderBox;

        public Form1()
        {
            InitializeComponent();

            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;

            pictureBox = new PictureBox[4, 4];
            borderBox = new PictureBox[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    double right = (j < 2 ? 0 : 4.1);
                    pictureBox[i, j] = new PictureBox()
                    {
                        Size = new Size(size - 8, size - 8),
                        Location = new Point(4 + (int)((j + right) * size), 4 + i * size),
                        BackColor = Color.White
                    };
                    pictureBox[i, j].MouseDown += new MouseEventHandler(PictureBoxIJ_MouseDown);

                    borderBox[i, j] = new PictureBox()
                    {
                        Size = new Size(size - 2, size - 2),
                        Location = new Point(1 + (int)((j + right) * size), 1 + i * size),
                        Enabled = false
                    };

                    panel1.Controls.Add(pictureBox[i, j]);
                    panel1.Controls.Add(borderBox[i, j]);
                }

            menu = new Form3 { Owner = this };
            menu.Show();
            //menu.Size = new Size(Width - 16, 296);
        }

        private void Form1_LocationChanged(object sender, EventArgs e) => menu.Location = new Point(Location.X + 8, Location.Y + 51);

        private void Form1_Shown(object sender, EventArgs e) => menu.Activate();

        public void Button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            for (; menu.Height < 296; menu.Height += 5) ;
            menu.Activate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            board = new Figurine[4, 4];
            storage = new Figurine[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    storage[i, j] = new Figurine(i < 2, j % 2 == 0, j < 2, i % 2 == 1);
                    borderBox[i, j].BackColor = Color.White;
                    pictureBox[i, j].Enabled = false;
                }
            Paint_Board();
            pictureBox1.Enabled = false;
            Paint_Storage();
            button2.Enabled = false;
        }

        private void Paint_Board()
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    g.FillEllipse(Brushes.OldLace, (j + 0.05f) * size, (i + 0.05f) * size, size * 0.9f, size * 0.9f);
                    if (board[i, j] != null)
                    {
                        Brush color;
                        if (board[i, j].Dark)
                            color = Brushes.Sienna;
                        else color = Brushes.BurlyWood;

                        float resize;
                        if (board[i, j].Big)
                            resize = 1;
                        else resize = 0.7f;

                        float param1 = (1 - resize * 0.9f / (float)Math.Sqrt(2.0)),
                            param2 = size * resize,
                            xy = size * (param1 / 2),
                            wh = param2 * 0.9f / (float)Math.Sqrt(2.0);
                        if (board[i,j].Square) g.FillRectangle(color, xy + j * size, xy + i * size, wh, wh);
                        else g.FillEllipse(color, xy + j * size, xy + i * size, wh, wh);

                        param1 = (1 - resize * 0.4f / (float)Math.Sqrt(2.0));
                        xy = size * (param1 / 2);
                        wh = param2 * 0.4f / (float)Math.Sqrt(2.0);
                        if (board[i, j].Point)
                            g.FillEllipse(new SolidBrush(Color.FromArgb(180, Color.Peru)), xy + j * size, xy + i * size, wh, wh);
                    }
                }
            pictureBox1.Refresh();
        }

        private void Paint_Storage()
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (storage[i, j] != null)
                    {
                        pictureBox[i, j].Image = new Bitmap(pictureBox[i, j].Width, pictureBox[i, j].Height);
                        Graphics g = Graphics.FromImage(pictureBox[i, j].Image);
                        Brush color;
                        if (storage[i, j].Dark)
                            color = Brushes.Sienna;
                        else color = Brushes.BurlyWood;

                        float resize;
                        if (storage[i, j].Big)
                            resize = 1;
                        else resize = 0.7f;

                        float param1 = (1 - resize * 0.9f / (float)Math.Sqrt(2.0)),
                            param2 = size * resize,
                            xy = size * (param1 / 2) - 4,
                            wh = param2 * 0.9f / (float)Math.Sqrt(2.0);
                        if (j < 2) g.FillRectangle(color, xy, xy, wh, wh);
                        else g.FillEllipse(color, xy, xy, wh, wh);

                        param1 = (1 - resize * 0.4f / (float)Math.Sqrt(2.0));
                        xy = size * (param1 / 2) - 4;
                        wh = param2 * 0.4f / (float)Math.Sqrt(2.0);
                        if (storage[i, j].Point)
                            g.FillEllipse(new SolidBrush(Color.FromArgb(180, Color.Peru)), xy, xy, wh, wh);
                        pictureBox[i, j].Refresh();
                    }   
        }

        public void StartGame()
        {
            Form1_Load(this, new EventArgs());
            queriesTableAdapter.IncGames(label6.Text);

            if (turn) // если первый ход
            {
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                        pictureBox[i, j].Enabled = true;
                richTextBox1.Text = "Ваш ход. Выберете фигуру, которой будет играть противник.";
                step = 0;
            }
            else
            {
                int i = rnd.Next(4), j = rnd.Next(4);
                borderBox[i, j].BackColor = Color.Blue;
                pictureBox1.Enabled = true;
                richTextBox1.Text = "Противник выбрал фигуру. Выберете поле на доске для установки этой фигуры.";
                step = 1;
            }

            label2.Text = "Ход: " + step.ToString();

            time = new DateTime(0, 0);
            timer1.Enabled = true;
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            time = time.AddSeconds(1);
            label1.Text = "Время: "+ time.ToString("HH:mm:ss");
        }

        private void PictureBoxIJ_MouseDown(object sender, MouseEventArgs e)
        {
            int i = (sender as PictureBox).Location.Y / size,
                j = (sender as PictureBox).Location.X / size;
            if (j > 1) j -= 4;
            if (storage[i, j] != null && ((Bitmap)pictureBox[i, j].Image).GetPixel(e.X, e.Y) != Color.FromArgb(0, 0, 0, 0))
            {
                for (int r = 0; r < 4; r++)
                    for (int c = 0; c < 4; c++)
                        borderBox[r, c].BackColor = Color.White;

                borderBox[i, j].BackColor = Color.Red;
                button2.Enabled = true;
                richTextBox1.Text = "Именно эта фигура? Если да - нажмите \"Подтвердить\".";
            }
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int x = e.X / size, y = e.Y / size;
            if (((Bitmap)pictureBox1.Image).GetPixel(e.X, e.Y) != Color.FromArgb(0, 0, 0, 0) && board[y, x] == null)
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                        if (borderBox[i, j].BackColor == Color.Blue)
                        {
                            Paint_Board();
                            Graphics g = Graphics.FromImage(pictureBox1.Image);
                            g.DrawEllipse(new Pen(Color.Red, 3), (x + 0.05f) * size, (y + 0.05f) * size, size * 0.9f, size * 0.9f);
                            g.DrawImage(pictureBox[i, j].Image, x * size + 4, y * size + 4, new Rectangle(0, 0, size, size), GraphicsUnit.Pixel);
                            g.Dispose();
                            pictureBox1.Refresh();
                            button2.Enabled = true;
                            richTextBox1.Text = "Уверены? Для выполнения данного хода нажмите \"Подтвердить\".";
                            return;
                        }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            if (pictureBox1.Enabled)
            {
                pictureBox1.Enabled = false;
                int istrg = 0, jstrg = 0, ibrd = 0, jbrd = 0;
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                    {
                        if (borderBox[i, j].BackColor == Color.Blue)
                        {
                            istrg = i;
                            jstrg = j;
                            borderBox[i, j].BackColor = Color.White;
                            borderBox[i, j].Refresh();
                            pictureBox[i, j].Image = null;
                            pictureBox[i, j].Refresh();
                        }
                        if (((Bitmap)pictureBox1.Image).GetPixel(j * size + size / 2, (int)((i + 0.05f) * size)) == Color.FromArgb(255, 255, 0, 0))
                        {
                            ibrd = i;
                            jbrd = j;
                        }
                    }

                board[ibrd, jbrd] = new Figurine(storage[istrg, jstrg]);
                storage[istrg, jstrg] = null;
                Paint_Board();

                if (!Stop(board) && !Standoff(board))
                {
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                            pictureBox[i, j].Enabled = true;
                    richTextBox1.Text = "Выберете фигуру, которой будет играть противник.";
                }
                else if (Standoff(board))
                {
                    richTextBox1.Text = "Ничья!";
                    timer1.Stop();
                    (menu as Form3).ContinueLock();
                }
                else
                {
                    richTextBox1.Text = "QUARTO! Вы выиграли!";
                    queriesTableAdapter.IncWins(label6.Text, time - new DateTime(0, 0));
                    queriesTableAdapter.SaveGame(label6.Text, time - new DateTime(0, 0), step, turn);
                    timer1.Stop();
                    (menu as Form3).ContinueLock();
                }
            }
            else
            {
                step++;
                label2.Text = "Ход: " + step.ToString();
                label2.Refresh();

                richTextBox1.Text = "Подождите, компьютер делает свой ход.";
                richTextBox1.Refresh();

                int istrg = 0, jstrg = 0;
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                    {
                        pictureBox[i, j].Enabled = false;
                        if (borderBox[i, j].BackColor == Color.Red)
                        {
                            borderBox[i, j].BackColor = Color.LimeGreen;
                            borderBox[i, j].Refresh();
                            istrg = i;
                            jstrg = j;
                        }
                    }

                Thread.Sleep(1000);

                Graphics g = Graphics.FromImage(pictureBox1.Image);
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                        if (board[i, j] == null)
                        {
                            Figurine[,] tmp = (Figurine[,])board.Clone();
                            tmp[i, j] = new Figurine(storage[istrg, jstrg]);
                            if (Stop(tmp) || Standoff(tmp))
                            {
                                g.DrawEllipse(new Pen(Color.LimeGreen, 3), (j + 0.05f) * size, (i + 0.05f) * size, size * 0.9f, size * 0.9f);
                                g.DrawImage(pictureBox[istrg, jstrg].Image, j * size + 4, i * size + 4, new Rectangle(0, 0, size, size), GraphicsUnit.Pixel);
                                pictureBox1.Refresh();
                                Thread.Sleep(200);

                                borderBox[istrg, jstrg].BackColor = Color.White;
                                borderBox[istrg, jstrg].Refresh();
                                pictureBox[istrg, jstrg].Image = null;
                                pictureBox[istrg, jstrg].Refresh();

                                board[i, j] = new Figurine(storage[istrg, jstrg]);
                                storage[istrg, jstrg] = null;

                                if (Standoff(board)) richTextBox1.Text = "Ничья!";
                                else richTextBox1.Text = "QUARTO! Компьютер выиграл!";

                                timer1.Stop();
                                (menu as Form3).ContinueLock();

                                return;
                            }
                        }

                int ibrd, jbrd, ifig, jfig, cnt = 0;
                bool flag;
                do
                {
                    cnt++;
                    flag = false;

                    do
                    {
                        ibrd = rnd.Next(4);
                        jbrd = rnd.Next(4);
                    } while (board[ibrd, jbrd] != null);

                    do
                    {
                        ifig = rnd.Next(4);
                        jfig = rnd.Next(4);

                    } while (storage[ifig, jfig] == null || ifig == istrg && jfig == jstrg);

                    Figurine[,] tmp1 = (Figurine[,])board.Clone();
                    tmp1[ibrd, jbrd] = new Figurine(storage[istrg, jstrg]);
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                            if (tmp1[i, j] == null)
                            {
                                Figurine[,] tmp2 = (Figurine[,])tmp1.Clone();
                                tmp2[i, j] = new Figurine(storage[ifig, jfig]);
                                if (Stop(tmp2)) flag = true;
                            }
                } while (flag || cnt < 256);

                g.DrawEllipse(new Pen(Color.LimeGreen, 3), (jbrd + 0.05f) * size, (ibrd + 0.05f) * size, size * 0.9f, size * 0.9f);
                g.DrawImage(pictureBox[istrg, jstrg].Image, jbrd * size + 4, ibrd * size + 4, new Rectangle(0, 0, size, size), GraphicsUnit.Pixel);
                pictureBox1.Refresh();
                Thread.Sleep(200);

                borderBox[istrg, jstrg].BackColor = Color.White;
                borderBox[istrg, jstrg].Refresh();
                pictureBox[istrg, jstrg].Image = null;
                pictureBox[istrg, jstrg].Refresh();

                board[ibrd, jbrd] = new Figurine(storage[istrg, jstrg]);
                storage[istrg, jstrg] = null;

                borderBox[ifig, jfig].BackColor = Color.Blue;
                borderBox[ifig, jfig].Refresh();

                richTextBox1.Text = "Противник сделал ход (зелёный). Выберете поле на доске для выбронной противником фигуры (синий).";
                pictureBox1.Enabled = true;

                step++;
                label2.Text = "Ход: " + step.ToString();
            }
        }

        bool Stop(Figurine[,] mem)
        {
            bool result = false;
            for (int i = 0; i < 4; i++)
            {
                if ((mem[i, 0] != null && mem[i, 1] != null && mem[i, 2] != null && mem[i, 3] != null &&
                    ((mem[i, 0].Big == mem[i, 1].Big && mem[i, 1].Big == mem[i, 2].Big && mem[i, 2].Big == mem[i, 3].Big) ||
                    (mem[i, 0].Dark == mem[i, 1].Dark && mem[i, 1].Dark == mem[i, 2].Dark && mem[i, 2].Dark == mem[i, 3].Dark) ||
                    (mem[i, 0].Point == mem[i, 1].Point && mem[i, 1].Point == mem[i, 2].Point && mem[i, 2].Point == mem[i, 3].Point) ||
                    (mem[i, 0].Square == mem[i, 1].Square && mem[i, 1].Square == mem[i, 2].Square && mem[i, 2].Square == mem[i, 3].Square)))
                    ||
                    (mem[0, i] != null && mem[1, i] != null && mem[2, i] != null && mem[3, i] != null &&
                    ((mem[0, i].Big == mem[1, i].Big && mem[1, i].Big == mem[2, i].Big && mem[2, i].Big == mem[3, i].Big) ||
                    (mem[0, i].Dark == mem[1, i].Dark && mem[1, i].Dark == mem[2, i].Dark && mem[2, i].Dark == mem[3, i].Dark) ||
                    (mem[0, i].Point == mem[1, i].Point && mem[1, i].Point == mem[2, i].Point && mem[2, i].Point == mem[3, i].Point) ||
                    (mem[0, i].Square == mem[1, i].Square && mem[1, i].Square == mem[2, i].Square && mem[2, i].Square == mem[3, i].Square))))
                    result = true;
            }
            if ((mem[0, 0] != null && mem[1, 1] != null && mem[2, 2] != null && mem[3, 3] != null &&
                ((mem[0, 0].Big == mem[1, 1].Big && mem[1, 1].Big == mem[2, 2].Big && mem[2, 2].Big == mem[3, 3].Big) ||
                (mem[0, 0].Dark == mem[1, 1].Dark && mem[1, 1].Dark == mem[2, 2].Dark && mem[2, 2].Dark == mem[3, 3].Dark) ||
                (mem[0, 0].Point == mem[1, 1].Point && mem[1, 1].Point == mem[2, 2].Point && mem[2, 2].Point == mem[3, 3].Point) ||
                (mem[0, 0].Square == mem[1, 1].Square && mem[1, 1].Square == mem[2, 2].Square && mem[2, 2].Square == mem[3, 3].Square)))
                ||
                (mem[0, 3] != null && mem[1, 2] != null && mem[2, 1] != null && mem[3, 0] != null &&
                ((mem[0, 3].Big == mem[1, 2].Big && mem[1, 2].Big == mem[2, 1].Big && mem[2, 1].Big == mem[3, 0].Big) ||
                (mem[0, 3].Dark == mem[1, 2].Dark && mem[1, 2].Dark == mem[2, 1].Dark && mem[2, 1].Dark == mem[3, 0].Dark) ||
                (mem[0, 3].Point == mem[1, 2].Point && mem[1, 2].Point == mem[2, 1].Point && mem[2, 1].Point == mem[3, 0].Point) ||
                (mem[0, 3].Square == mem[1, 2].Square && mem[1, 2].Square == mem[2, 1].Square && mem[2, 1].Square == mem[3, 0].Square))))
                result = true;

            return result;
        }

        private bool Standoff(Figurine[,] mem)
        {
            bool result = true;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (mem[i, j] == null) result = false;
            return result;
        }

        public void SetName(string str)
        {
            label5.Text = "Игрок: ";
            label6.Text = str;
            label5.Left = (Width - (label5.Width + label6.Width)) / 2;
            label6.Left = label5.Location.X + label5.Width;
            label1.Text = "";
            label2.Text = "";
        }
    }
}
