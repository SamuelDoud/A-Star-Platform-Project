using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockGarden
{
    class Garden
    {
        private int length, width;
        private Atom[,] grid;
        /// <summary>
        /// 
        /// </summary>
        public Garden(int length, int width)
        {
            this.length = length;
            this.width = width;
            this.grid = new Atom[length, width];
        }
    }
}
