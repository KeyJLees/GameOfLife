using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LifeSimulation.Cell;

namespace LifeSimulation
{
    public partial class Form1 : Form
    {
        public int RESOLUTION = 1;
        const int C = 1000;
        private Graphics graphics;
        Field field;
        public Form1()
        {
            InitializeComponent();
        }

        private void InitMap()
        {
            pictureBox1.Image = new Bitmap(1000 * (int)nudResolution.Value, 1000 * (int)nudResolution.Value);
            graphics = Graphics.FromImage(pictureBox1.Image);
            field = new Field(C, C);
            field.InitField((int)countOfMobs.Value, (int)countOfFood.Value, C);
            DrawPixels();
        }

        private void UpdateMap()
        {
            pictureBox1.Image = new Bitmap(1000 * (int)nudResolution.Value, 1000 * (int)nudResolution.Value);
            graphics = Graphics.FromImage(pictureBox1.Image);
            DrawPixels();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
                return;
            countOfMobs.Enabled = false;
            countOfFood.Enabled = false;
            nudPlantsPerSceonds.Enabled = false;
            RESOLUTION = (int)nudResolution.Value;
            InitMap();
            timer1.Start();
            timer2.Start();
            timer3.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            timer3.Stop();
            countOfMobs.Enabled = true;
            countOfFood.Enabled = true;
            nudPlantsPerSceonds.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            field.NextMove(RESOLUTION);
            DrawPixels();
            pictureBox1.Refresh();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            field.GrowFood((int)nudPlantsPerSceonds.Value);
            pictureBox1.Refresh();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            field.NextMoveZ(RESOLUTION);
            DrawPixels();
            pictureBox1.Refresh();
        }

        private void DrawPixels()
        {
            graphics.Clear(Color.Green);
            for (int i = 0; i < field.plants.Count; i++)
            {
                graphics.FillRectangle(Brushes.Yellow, field.plants[i].x * RESOLUTION, field.plants[i].y * RESOLUTION, RESOLUTION, RESOLUTION);
            }
            for (int i = 0; i < field.mobs.Count; i++)
            {
                graphics.FillRectangle(Brushes.Crimson, field.mobs[i].x * RESOLUTION, field.mobs[i].y * RESOLUTION, RESOLUTION, RESOLUTION);
                if (field.NewField[field.mobs[i].xp, field.mobs[i].yp].Type != TypeOfCell.Mob &&
                    field.NewField[field.mobs[i].xp, field.mobs[i].yp].Type != TypeOfCell.Zombie)
                    graphics.FillRectangle(Brushes.Green, field.mobs[i].xp * RESOLUTION, field.mobs[i].yp * RESOLUTION, RESOLUTION, RESOLUTION);
            }
            for (int i = 0; i < field.zombies.Count; i++)
            {
                graphics.FillRectangle(Brushes.Black, field.zombies[i].x * RESOLUTION, field.zombies[i].y * RESOLUTION, RESOLUTION, RESOLUTION);
                if (field.NewField[field.zombies[i].xp, field.zombies[i].yp].Type != TypeOfCell.Zombie)
                    graphics.FillRectangle(Brushes.Green, field.zombies[i].xp * RESOLUTION, field.zombies[i].yp * RESOLUTION, RESOLUTION, RESOLUTION);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e) 
        {
            Point Cordinate = ((MouseEventArgs)e).Location;
            int X = Cordinate.X / RESOLUTION;
            int Y = Cordinate.Y / RESOLUTION;
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked)
            {
                if (field.NewField[X, Y].Type == TypeOfCell.Mob)
                {
                    for (int i = 0; i < field.NewField[X, Y].mobs.Count; i++)
                        MessageBox.Show(String.Format("CountOfMobs: {4} \n\nX: {0}\nY: {1}\nFullness: {2} / {3}\nGender: {5}",
                            X,
                            Y,
                            field.NewField[X, Y].mobs[i].fullness,
                            100,
                            field.NewField[X, Y].mobs.Count,
                            field.NewField[X, Y].mobs[i].gender));
                }
            }
            else if(checkBox1.Checked)
            {
                field.CreateMob(X, Y);
                graphics.FillRectangle(Brushes.Crimson, X * RESOLUTION, Y * RESOLUTION, RESOLUTION, RESOLUTION);
                pictureBox1.Refresh();
            }
            else if(checkBox2.Checked)
            {
                field.CreatePlant(X, Y);
                graphics.FillRectangle(Brushes.Yellow, X * RESOLUTION, Y * RESOLUTION, RESOLUTION, RESOLUTION);
                pictureBox1.Refresh();
            }
            else
            {
                field.CreateZombie(X, Y);
                graphics.FillRectangle(Brushes.Black, X * RESOLUTION, Y * RESOLUTION, RESOLUTION, RESOLUTION);
                pictureBox1.Refresh();
            }

        }

        private void countOfFood_ValueChanged(object sender, EventArgs e) { }

        private void label3_Click(object sender, EventArgs e) { }

        private void nudResolution_ValueChanged(object sender, EventArgs e)
        {
            if (field != null)
            {
                RESOLUTION = (int)nudResolution.Value;
                UpdateMap();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
            }
            else
            {
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Enabled = false;
                checkBox3.Enabled = false;
            }
            else
            {
                checkBox1.Enabled = true;
                checkBox3.Enabled = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
            }
            else
            {
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
            }
        }
    }
}
