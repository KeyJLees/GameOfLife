using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifeSimulation
{
    public class Cell
    {
        public int x;
        public int y;
        public TypeOfCell Type;
        public List<Mob> mobs = new List<Mob>();
        public List<Zombie> zombies = new List<Zombie>();
        public enum TypeOfCell
        {
            Empty = 1,
            Mob = 2,
            Plant = 3,
            Zombie = 4
        }
        public Cell(int X, int Y, TypeOfCell TypeOfCell)
        {
            x = X;
            y = Y;
            Type = TypeOfCell;
        }
    }
}
