using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockGarden
{
    class Rock : Resident
    {
        
        public const int NORTH = 0, EAST = 1, SOUTH = 2, WEST = 3;
        public static bool isGravel = true;
        private int width, height, length;
        private int[] angleFaces = new int[4];
        //Need to define attributes of rocks here...
        /// <summary>
        /// 
        /// </summary>
        public Rock(int width, int length): base(width, length, 1, isGravel)
        {

        }

    }
}
