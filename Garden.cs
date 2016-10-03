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
            grid = new Atom[width, length];
            foreach (Tuple<int, int> coordinate in getCoordinates())
            {
                grid[coordinate.Item1, coordinate.Item2] = new Atom();
            }
        }

        /// <summary>
        /// Return the Resident that occupies this coordinate pair.
        /// </summary>
        /// <param name="x">X portion of the coordinate pair.</param>
        /// <param name="y">Y portion of the coordinate pair.</param>
        /// <returns>The Resident that occupies this square</returns>
        public Resident getResident(int x, int y)
        {
            return grid[x, y].getResident();
        }
        /// <summary>
        /// Fills empty atoms in the garden with blank gravel.
        /// </summary>
        public void fillWithGravel()
        {
            foreach (Tuple<int, int> coordinate in getCoordinates())
            {
                if (grid[coordinate.Item1, coordinate.Item2].getResident() == null)
                {
                    grid[coordinate.Item1, coordinate.Item2].setResident(new Gravel(),
                    coordinate.Item1, coordinate.Item2);
                }
            }
        }
        /// <summary>
        /// Adds a Resident to the garden.
        /// </summary>
        /// <param name="newNeighbor">The Resident to be added to the garden.</param>
        /// <param name="x">The leftmost x coordinate where the Resident will be placed.</param>
        /// <param name="y">The bottommost y coordinate where the Resident will be placed.</param>
        /// <param name="overwrite">Boolean indicating if the object will remove any object that
        /// may be in its intended footprint.</param>
        /// <returns>A boolean indicating whether the addition was successful or not.</returns>
        public bool addResident(Resident newNeighbor, int x, int y, bool overwrite)
        {
            int width = newNeighbor.width;
            int length = newNeighbor.length;
            List<Tuple<int, int>> pointsToAddTo = getCoordinates(x, y, newNeighbor.width, newNeighbor.length);
            //check if we can add the resident to the grid
            //return false if user does not want to overwrite anything that isn't gravel
            //remove that object if it is.
            foreach (Tuple<int, int> coordinate in pointsToAddTo)
            {
                //is there an item here? is it gravel?
                if (getResident(coordinate.Item1, coordinate.Item2) != null &&
                    !getResident(coordinate.Item1, coordinate.Item2).isGravel)
                {
                    if (!overwrite)
                    {
                        //Since there is a Resident here, we must throw false to show that the addition won't happen
                        return false;
                    }
                    else
                    {
                        //delete the object here at all locations it exists
                        removeResident(x, y);
                    }
                }
            }
            //add the resident to the grid at each point it would occupy based on its size
            foreach (Tuple<int, int> coordinate in pointsToAddTo)
            {
                grid[coordinate.Item1, coordinate.Item2].setResident(newNeighbor, x, y);
            }
            return true;
        }

        /// <summary>
        /// Removes any object that occupies this point on the grid.
        /// Will remove all instances of this object.
        /// </summary>
        /// <param name="x">Any x coordinate of the object to be removed so long as the y
        /// coodinate that forms the pair (x, y) contains the Resident to be removed.
        /// </param>
        /// <param name="y">Any y coordinate of the object to be removed so long as the x
        /// coodinate that forms the pair (x, y) contains the Resident to be removed.</param>
        /// <returns>Boolean indicating whether any object existed on the passed set of
        /// coordinates.</returns>
        public bool removeResident(int x, int y)
        {
            Resident evictee = grid[x, y].getResident();
            if (evictee == null)
            {
                //no Resident occupies this space
                return false;
            }
            //go through each Atom that contains this object
            foreach (Tuple<int, int> coordinate in getCoordinates(this.grid[x,y].getBaseX(),
                grid[x, y].getBaseY(), evictee.width, evictee.length))
            {
                grid[coordinate.Item1, coordinate.Item2].removeResident();
            }
            return true;

        }

        /// <summary>
        /// Return a list of coordinates that would be found in a rectangle of width x length size
        /// based at the coordinate (startX, startY).
        /// </summary>
        /// <param name="startX">Base x coordinate</param>
        /// <param name="startY">Base y coordinate</param>
        /// <param name="width">Number of atoms wide the selection is</param>
        /// <param name="length">Number of atoms long the selection is </param>
        /// <returns></returns>
        private List<Tuple <int, int>> getCoordinates(int startX, int startY, int width, int length)
        {
            var points = new List<Tuple<int, int>>();
            for (int currentX = startX; currentX < startX + width; currentX++)
            {
                for (int currentY = startY; currentY < startY + length; currentY++)
                {
                    points.Add(Tuple.Create(currentX, currentY));
                }
            }
            return points;
        }

        private List<Tuple<int, int>> getCoordinates()
        {
            return getCoordinates(0, 0, width, length);
        }
        /// <summary>
        /// Get a string representation of the Rock Garden.
        /// </summary>
        /// <returns>A string which gives an ASCII art view of the rock garden</returns>
        public override string ToString()
        {
            string gardenString = "";
            for (int x = 0; x < width; x++)
            {
                //counting down as the (0, 0) is located in the bottom right corner
                for (int y = length - 1; y >= 0; y--)
                {
                    gardenString += grid[x, y].ToString();
                }
                //going to a new row so we will need a new line
                gardenString += "\n";
            }
            return gardenString;
        }
    }
}
