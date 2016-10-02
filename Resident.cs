using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockGarden
{
    class Resident
    {
        private bool isGravel;
        private int width, depth, length;
        public Resident(int width, int length, int depth, bool isGravel)
        {
            this.width = width;
            this.length = length;
            this.depth = depth;
            this.isGravel = isGravel;
        }
        public bool gravel()
        {
            return this.isGravel;
        }
        public int getArea()
        {
            return this.width * this.length;
        }
    }
}
