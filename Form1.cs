using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        public Graphics graphics;
        Field field = new Field();
        public Form1()
        {
            InitializeComponent();
        }

        public void StartGame()
        {
            if (timer1.Enabled)
            {
                return;
            }
            nudResolution.Enabled = false;
            nudDensity.Enabled = false;
            field.resolution = (int)nudResolution.Value;
            field.rows = 1000;
            field.cols = 1000;
            field.field = new bool[field.cols, field.rows];
            Random random = new Random();
            for (int x = 0; x < field.cols; x++)
            {
                for (int y = 0; y < field.rows; y++)
                {
                    field.field[x, y] = random.Next((int)nudDensity.Value) == 0;

                }
            }
            pictureBox1.Image = new Bitmap(1000, 1000);
            graphics = Graphics.FromImage(pictureBox1.Image);
            graphics.Clear(Color.Black);
            field.graphics = graphics;
            timer1.Start();
        }

        public void StopGame()
        {
            timer1.Stop();
            nudResolution.Enabled = true;
            nudDensity.Enabled = true;

        }

        private void bStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            field.NextGeneration();
            pictureBox1.Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
    public class Field
    {
        public Graphics graphics;
        public int resolution;
        public bool[,] field;
        public int cols;
        public int rows;

        public void NextGeneration()
        {
            Random rnd = new Random();
            var newfield = new bool[cols, rows];

            for (int x = 1; x < cols - 1; x++)
            {
                for (int y = 1; y < rows - 1; y++)
                {
                    newfield[x, y] = field[x, y];
                }
            }
            for (int x = 1; x < cols - 1; x++)
            {
                for (int y = 1; y < rows - 1; y++)
                {
                    int value = rnd.Next(1, 4);
                    if (value == 1)
                    {
                        if (newfield[x + 1, y] == false)
                        {
                            newfield[x, y] = false;
                            newfield[x + 1, y] = true;
                        }
                    }
                    else if (value == 2)
                    {
                        if (newfield[x - 1, y] == false)
                        {
                            newfield[x, y] = false;
                            newfield[x - 1, y] = true;
                        }
                    }
                    else if (value == 3)
                    {
                        if (newfield[x, y + 1] == false)
                        {
                            newfield[x, y] = false;
                            newfield[x, y + 1] = true;
                        }
                    }
                    else if (value == 4)
                    {
                        if (newfield[x, y - 1] == false)
                        {
                            newfield[x, y] = false;
                            newfield[x, y - 1] = true;
                        }
                    }
                }
            }
            for (int x = 1; x < cols - 1; x++)
            {
                for (int y = 1; y < rows - 1; y++)
                {
                    if (newfield[x, y] == true)
                    {
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution, resolution);
                    }
                    else
                    {
                        graphics.FillRectangle(Brushes.Green, x * resolution, y * resolution, resolution, resolution);
                    }
                }
            }
        }

    }
}
