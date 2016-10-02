using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockGarden
{
    
    class Gravel : Resident
    {
        public static int n_directions = 8; //The four cardinal directions + NE, NW, SW, SE.
        public static int NORTH = 0, NORTHEAST = 1, EAST = 2, SOUTHEAST = 3, SOUTH = 4, SOUTHWEST = 5, WEST = 6, NORTHWEST = 7;
        public static bool isGravel = true;
        public bool isStream {get; set;}
        public List<int> connections = new List<int>();
        public Gravel(int width, int length) : base(width, length, 1, isGravel)
        {

        }
        //it would make sense if the gravel is a stream that it has two "connections"
        //connections being which direction the gravel's stream faces
        public int numberConnections()
        {
            return this.connections.Count;
        }
        public void addConnection(int direction)
        {
            this.connections.Add(direction);
        }
        public List<int> getConnections()
        {
            return this.connections;
        }

    }
}
