using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSimulation
{
    public class Plant
    {
        public int x;
        public int y;
        public int satiety;
        public bool dead = false;
        public Plant(int X, int Y, int S)
        {
            x = X;
            y = Y;
            satiety = S;
        }
    }
}
