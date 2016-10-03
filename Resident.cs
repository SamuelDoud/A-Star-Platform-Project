using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockGarden
{
    class Resident
    {
        public bool isGravel;
        public int width, height, length;
        public string stringRepresentation = " ";
        public Resident(int width, int length, int height, bool isGravel)
        {
            this.width = width;
            this.length = length;
            this.height = height;
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
        public override string ToString()
        {
            return stringRepresentation;
        }
    }
}
