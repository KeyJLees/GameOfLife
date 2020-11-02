using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LifeSimulation.Cell;

namespace LifeSimulation
{
    public class Field
    {
        static Random rnd = new Random();
        private int WIDTH;
        private int HEIGHT;
        public Cell[,] NewField;
        public List<Mob> mobs = new List<Mob>();
        public List<Plant> plants = new List<Plant>();
        public List<Zombie> zombies = new List<Zombie>();

        public Field(int v1, int v2)
        {
            WIDTH = v1;
            HEIGHT = v2;
            NewField = new Cell[WIDTH, HEIGHT];
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    NewField[x, y] = new Cell(x, y, TypeOfCell.Empty);
                }
            }
        }

        public void CreateMob(int x, int y)
        {
            if (NewField[x, y].Type == TypeOfCell.Empty)
            {
                Mob mob = new Mob(x, y, 100);
                NewField[x, y].mobs.Add(mob);
                mobs.Add(mob);
            }
        }

        public void CreatePlant(int x, int y)
        {
            if (NewField[x, y].Type == TypeOfCell.Empty)
            {
                Plant plant = new Plant(x, y, 25);
                plants.Add(plant);
            }
        }

        public void CreateZombie(int x, int y)
        {
            if (NewField[x, y].Type == TypeOfCell.Empty)
            {
                Zombie zombie = new Zombie(x, y);
                zombies.Add(zombie);
            }
        }

        private void UpdateField()
        {
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    NewField[x, y].Type = TypeOfCell.Empty;
                }
            }
        }

        private void SetMobs()
        {
            for (int i = 0; i < plants.Count; i++)
            {
                if (NewField[plants[i].x, plants[i].y].Type == TypeOfCell.Mob ||
                    NewField[plants[i].x, plants[i].y].Type == TypeOfCell.Zombie)
                {
                    plants.RemoveAt(i);
                }
            }

            for (int i = 0; i < mobs.Count; i++)
            {
                if (NewField[mobs[i].x, mobs[i].y].Type == TypeOfCell.Zombie)
                {
                    mobs.RemoveAt(i);
                }
            }
            UpdateField();
            for (int i = 0; i < plants.Count; i++)
            {
                NewField[plants[i].x, plants[i].y].Type = TypeOfCell.Plant;
            }
            for (int i = 0; i < mobs.Count; i++)
            {
                NewField[mobs[i].x, mobs[i].y].Type = TypeOfCell.Mob;
                mobs[i].fullness = mobs[i].fullness - 5;
                mobs[i].timer--;
            }
            for (int i = 0; i < zombies.Count; i++)
            {
                NewField[zombies[i].x, zombies[i].y].Type = TypeOfCell.Zombie;
            }
        }

        public void InitField(int countOfMobs, int countOfPlants, int C)
        {
            UpdateField();
            InitMobs(countOfMobs, C);
            InitPlants(countOfPlants, C);
            SetMobs();
        }

        private void InitMobs(int countOfMobs, int C)
        {
            bool flag;
            for (int i = 0; i < countOfMobs; i++)
            {
                flag = true;
                while (flag)
                {
                    int X = rnd.Next(5, C - 5);
                    int Y = rnd.Next(5, C - 5);
                    if (NewField[X, Y].Type == TypeOfCell.Empty)
                    {
                        Mob mob = new Mob(X, Y, 100);
                        NewField[X, Y].mobs.Add(mob);
                        mobs.Add(mob);
                        flag = false;
                    }
                }
            }
        }

        private void InitPlants(int countOfPlants, int C)
        {
            bool flag;
            for (int i = 0; i < countOfPlants; i++)
            {
                flag = true;
                while (flag)
                {
                    int X = rnd.Next(5, C - 5);
                    int Y = rnd.Next(5, C - 5);
                    if (NewField[X, Y].Type == TypeOfCell.Empty)
                    {
                        plants.Add(new Plant(X, Y, 20));
                        flag = false;
                    }
                }
            }
        }

        public void NextMove(int resolution)
        {
            for (int i = 0; i < mobs.Count; i++)
            {
                if (mobs[i].fullness > 0)
                    mobs[i].MoveMob(WIDTH, HEIGHT, NewField, resolution, mobs);
                else
                {
                    NewField[mobs[i].x, mobs[i].y].mobs.Remove(mobs[i]);
                    mobs.RemoveAt(i);
                }
            }
            SetMobs();
        }

        public void NextMoveZ(int resolution)
        {
            for (int i = 0; i < zombies.Count; i++)
            {
                zombies[i].SimpleMove(WIDTH, HEIGHT, NewField, resolution, mobs, TypeOfCell.Mob, zombies);
            }
            SetMobs();
        }

        public void GrowFood(int c)
        {
            for (int j = 0; j < c; j++)
            {
                bool flag = true;
                while (flag)
                {
                    int i = rnd.Next(plants.Count);
                    int rx = rnd.Next(6) - 3;
                    int ry = rnd.Next(6) - 3;
                    if (plants[i].x < WIDTH - 8 &&
                        plants[i].x < HEIGHT - 8 &&
                        plants[i].x > 8 &&
                        plants[i].y > 8)
                    {
                        if (NewField[plants[i].x + rx, plants[i].y + ry].Type == TypeOfCell.Empty)
                        {
                            plants.Add(new Plant(plants[i].x + rx, plants[i].y + ry, 20));
                            flag = false;
                        }
                    }
                }
            }
        }
    }
}
