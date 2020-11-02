using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LifeSimulation.Cell;

namespace LifeSimulation
{
    public class Mob
    {
        public int x;
        public int y;
        public int xp;
        public int yp;
        private int fullness = 100;
        private int timer = 10;
        private int vision = 5;
        public TypeOfGender gender;
        TypeOfGender needGender;
        TypeOfStrategy strategy;
        static Random rnd = new Random();

        public enum TypeOfGender
        {
            Male = 1,
            Female = 2,
            Trans = 3
        }

        public enum TypeOfStrategy
        {
            Standart = 1,
            Partner = 2,
            Border = 3
        }
        public Mob(int X, int Y, int FULLNESS)
        {
            x = X;
            y = Y;
            fullness = FULLNESS;
            int value = rnd.Next(3);
            if (value == 1)
            {
                gender = TypeOfGender.Male;
                needGender = TypeOfGender.Female;
            }
            else if (value == 2)
            {
                gender = TypeOfGender.Female;
                needGender = TypeOfGender.Male;
            }
            else
                gender = TypeOfGender.Trans;

            value = rnd.Next(3);
            if (value == 0)
                strategy = TypeOfStrategy.Standart;
            else if (value == 1)
                strategy = TypeOfStrategy.Partner;
            else
                strategy = TypeOfStrategy.Border;
        }

        public void MoveMob(int WIDTH, int HEIGHT, Cell[,] NewField, int RESOLUTION, List<Mob> mobs)
        {
            if (fullness > 60)
            {
                Reproduction(WIDTH, HEIGHT, NewField, RESOLUTION, mobs);
            }
            else if (fullness > 0)
            {
                if (strategy == TypeOfStrategy.Standart)
                    StandartStrategy(WIDTH, HEIGHT, NewField, RESOLUTION, mobs);
                else if (strategy == TypeOfStrategy.Partner)
                    PartnerStrategy(WIDTH, HEIGHT, NewField, RESOLUTION, mobs);
                else
                    BorderStrategy(WIDTH, HEIGHT, NewField, RESOLUTION, mobs);
            }
        }

        private void Reproduction(int WIDTH, int HEIGHT, Cell[,] NewField, int RESOLUTION, List<Mob> mobs)
        {
            if (gender == TypeOfGender.Trans)
            {
                RandomMove(WIDTH, HEIGHT, NewField, RESOLUTION, mobs);
            }
            else 
            {
                SearchPartnerMove(WIDTH, HEIGHT, NewField, RESOLUTION, mobs);
            }
        }

        private void RandomMove(int WIDTH, int HEIGHT, Cell[,] NewField, int RESOLUTION, List<Mob> mobs)
        {
            int value = rnd.Next(1, 4);
            if (value == 1)
                Move(WIDTH, HEIGHT, NewField, RESOLUTION, 1, 0, mobs);
            else if (value == 2)
                Move(WIDTH, HEIGHT, NewField, RESOLUTION, -1, 0, mobs);
            else if (value == 3)
                Move(WIDTH, HEIGHT, NewField, RESOLUTION, 0, 1, mobs);
            else if (value == 4)
                Move(WIDTH, HEIGHT, NewField, RESOLUTION, 0, -1, mobs);
        }

        private void SearchPartnerMove(int WIDTH, int HEIGHT, Cell[,] NewField, int RESOLUTION, List<Mob> mobs)
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
                        NewField[i, j].Type == TypeOfCell.Mob &&
                        NewField[i, j].mobs.Count == 1 &&
                        NewField[i, j].mobs[0].gender == needGender &&
                        NewField[i, j].mobs[0].timer == 0 &&
                        NewField[i, j].mobs[0].fullness > 60 &&
                        timer == 0 &&
                        fullness > 60 &&
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
                if ((Math.Abs(x - xf) + Math.Abs(y - yf)) > 1 || gender == TypeOfGender.Male)
                {
                    if (x - xf != 0)
                    {
                        if (x > xf)
                            Move(WIDTH, HEIGHT, NewField, RESOLUTION, -1, 0, mobs);
                        else
                            Move(WIDTH, HEIGHT, NewField, RESOLUTION, 1, 0, mobs);
                    }
                    else if (y - yf != 0)
                    {
                        if (y > yf)
                            Move(WIDTH, HEIGHT, NewField, RESOLUTION, 0, -1, mobs);
                        else
                            Move(WIDTH, HEIGHT, NewField, RESOLUTION, 0, 1, mobs);
                    }
                }
            }
            else
                RandomMove(WIDTH, HEIGHT, NewField, RESOLUTION, mobs);
        }

        private bool SimpleMove(int WIDTH, int HEIGHT, Cell[,] NewField, int RESOLUTION, List<Mob> mobs, TypeOfCell type)
        {
            bool flag = false;
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
                        NewField[i, j].Type == type && lenght < minlength)
                    {
                        minlength = lenght;
                        xf = i;
                        yf = j;
                    }
                }
            }
            if (minlength != 51)
            {
                flag = true;
                if (x - xf != 0)
                {
                    if (x > xf)
                        Move(WIDTH, HEIGHT, NewField, RESOLUTION, -1, 0, mobs);
                    else
                        Move(WIDTH, HEIGHT, NewField, RESOLUTION, 1, 0, mobs);
                }
                else if (y - yf != 0)
                {
                    if (y > yf)
                        Move(WIDTH, HEIGHT, NewField, RESOLUTION, 0, -1, mobs);
                    else
                        Move(WIDTH, HEIGHT, NewField, RESOLUTION, 0, 1, mobs);
                }
            }
            return flag;
        }

        private void StandartStrategy(int WIDTH, int HEIGHT, Cell[,] NewField, int RESOLUTION, List<Mob> mobs)
        {
            if (!SimpleMove(WIDTH, HEIGHT, NewField, RESOLUTION, mobs, TypeOfCell.Plant))
            {
                RandomMove(WIDTH, HEIGHT, NewField, RESOLUTION, mobs);
            }
        }

        private void PartnerStrategy(int WIDTH, int HEIGHT, Cell[,] NewField, int RESOLUTION, List<Mob> mobs)
        {
            if (!SimpleMove(WIDTH, HEIGHT, NewField, RESOLUTION, mobs, TypeOfCell.Plant))
                if (!SimpleMove(WIDTH, HEIGHT, NewField, RESOLUTION, mobs, TypeOfCell.Mob))
                    RandomMove(WIDTH, HEIGHT, NewField, RESOLUTION, mobs);
        }
        private void BorderStrategy(int WIDTH, int HEIGHT, Cell[,] NewField, int RESOLUTION, List<Mob> mobs)
        {
            if (!SimpleMove(WIDTH, HEIGHT, NewField, RESOLUTION, mobs, TypeOfCell.Plant))
            {
                int valx = Math.Min(x, 1000 - x);
                int valy = Math.Min(y, 1000 - y);
                if (valx < valy)
                {
                    if (x < 1000 - x)
                        Move(WIDTH, HEIGHT, NewField, RESOLUTION, -1, 0, mobs);
                    else
                        Move(WIDTH, HEIGHT, NewField, RESOLUTION, 1, 0, mobs);
                }
                else
                {
                    if (y < 1000 - y)
                        Move(WIDTH, HEIGHT, NewField, RESOLUTION, 0, -1, mobs);
                    else
                        Move(WIDTH, HEIGHT, NewField, RESOLUTION, 0, 1, mobs);
                }

            }

        }

        private void Move(int WIDTH, int HEIGHT, Cell[,] NewField, int RESOLUTION, int difx, int dify, List<Mob> mobs)
        {
            if (x + difx < WIDTH - 5 &&
                x + difx > 5 &&
                y + difx < HEIGHT - 5 &&
                y + difx > 5)
            {
                if (NewField[x + difx, y + dify].mobs.Count == 1)
                {
                    if (NewField[x + difx, y + dify].mobs[0].gender == needGender || NewField[x + difx, y + dify].mobs[0].gender == TypeOfGender.Trans &&
                        NewField[x + difx, y + dify].mobs[0].timer == 0 &&
                        NewField[x + difx, y + dify].mobs[0].fullness > 60 &&
                        fullness > 60 &&
                        timer == 0)
                    {
                        Mob mob = new Mob(x, y, 50);
                        NewField[x, y].mobs.Add(mob);
                        mobs.Add(mob);
                        timer = 10;
                        NewField[x, y].mobs[0].timer = 10;
                    }
                }
                else if (NewField[x + difx, y + dify].Type == TypeOfCell.Plant)
                {
                    if (fullness < 80)
                        fullness = fullness + 25;
                    else
                        fullness = 100;
                }
                xp = x;
                yp = y;
                x = x + difx;
                y = y + dify;
                NewField[xp, yp].mobs.Remove(this);
                NewField[x, y].mobs.Add(this);
            }
        }
    }
}
