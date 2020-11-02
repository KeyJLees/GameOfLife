using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LifeSimulation.Cell;

namespace LifeSimulation
{
    public class Zombie
    {
        public int x;
        public int y;
        public int xp;
        public int yp;
        public int vision = 10;

        public Zombie(int X, int Y)
        {
            x = X;
            y = Y;
        }

        public void SimpleMove(int WIDTH, int HEIGHT, Cell[,] NewField, int RESOLUTION, List<Mob> mobs, TypeOfCell type, List<Zombie> zombies)
        {
            int xf = 0;
            int yf = 0;
            int minlength = 51;
            int lenght;
            for (int i = x - vision; i < x + vision; i++)
            {
                for (int j = y - vision; j < y + vision; j++)
                {
                    lenght = Math.Abs(x - i) + Math.Abs(y - j);
                    if (i < WIDTH - RESOLUTION &&
                        j < HEIGHT - RESOLUTION &&
                        i > 0 &&
                        j > 0 &&
                        NewField[i, j].Type == type &&
                        lenght < minlength)
                    {
                        minlength = lenght;
                        xf = i;
                        yf = j;
                    }
                }
            }
            if (minlength != 51)
            {
                if (x - xf != 0)
                {
                    if (x > xf)
                        Move(WIDTH, HEIGHT, NewField, RESOLUTION, -1, 0, mobs, zombies);
                    else
                        Move(WIDTH, HEIGHT, NewField, RESOLUTION, 1, 0, mobs, zombies);
                }
                else if (y - yf != 0)
                {
                    if (y > yf)
                        Move(WIDTH, HEIGHT, NewField, RESOLUTION, 0, -1, mobs, zombies);
                    else
                        Move(WIDTH, HEIGHT, NewField, RESOLUTION, 0, 1, mobs, zombies);
                }
            }
        }
        private void Move(int WIDTH, int HEIGHT, Cell[,] NewField, int RESOLUTION, int difx, int dify, List<Mob> mobs, List<Zombie> zombies)
        {
            if (x + difx < WIDTH - 5 &&
                x + difx > 5 &&
                y + difx < HEIGHT - 5 &&
                y + difx > 5)
            {
                if (NewField[x + difx, y + dify].Type == TypeOfCell.Mob)
                {
                    NewField[x + difx, y + dify].Type = TypeOfCell.Zombie;
                    NewField[x, y].zombies.Add(this);
                    NewField[x, y].mobs.Clear();
                }
                xp = x;
                yp = y;
                x = x + difx;
                y = y + dify;
                NewField[xp, yp].zombies.Remove(this);
                NewField[x, y].zombies.Add(this);
            }
        }

    }
}
