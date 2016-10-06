using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RockGarden
{
    class Garden
    {
        public static int n_directions = 8; //The four cardinal directions + NE, NW, SW, SE.
        public const int NORTH = 0, NORTHEAST = 1, EAST = 2, SOUTHEAST = 3, SOUTH = 4, SOUTHWEST = 5, WEST = 6, NORTHWEST = 7;
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
            foreach (Point coordinate in getCoordinates())
            {
                grid[coordinate.X, coordinate.Y] = new Atom();
            }
        }

        /// <summary>
        /// Return the Resident that occupies this coordinate pair.
        /// </summary>
        /// <param name="x">X portion of the coordinate pair.</param>
        /// <param name="y">Y portion of the coordinate pair.</param>
        /// <returns>The Resident that occupies this square</returns>
        public Resident getResident(Point spot)
        {
            return grid[spot.X, spot.Y].getResident();
        }
        /// <summary>
        /// Fills empty atoms in the garden with blank gravel.
        /// </summary>
        public void fillWithGravel()
        {
            foreach (Point coordinate in getCoordinates())
            {
                if (grid[coordinate.X, coordinate.Y].getResident() == null)
                {
                    grid[coordinate.X, coordinate.Y].setResident(new Gravel(),
                    coordinate);
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
        public bool addResident(Resident newNeighbor, Point spot, bool overwrite)
        {
            int width = newNeighbor.width;
            int length = newNeighbor.length;
            List<Point> pointsToAddTo = getCoordinates(spot, newNeighbor.width, newNeighbor.length);
            //check if we can add the resident to the grid
            //return false if user does not want to overwrite anything that isn't gravel
            //remove that object if it is.
            foreach (Point coordinate in pointsToAddTo)
            {
                //is there an item here? is it gravel?
                if (getResident(coordinate) != null &&
                    !getResident(coordinate).isGravel)
                {
                    if (!overwrite)
                    {
                        //Since there is a Resident here, we must throw false to show that the addition won't happen
                        return false;
                    }
                    else
                    {
                        //delete the object here at all locations it exists
                        removeResident(spot);
                    }
                }
            }
            //add the resident to the grid at each point it would occupy based on its size
            foreach (Point coordinate in pointsToAddTo)
            {
                grid[coordinate.X, coordinate.Y].setResident(newNeighbor, spot);
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
        public bool removeResident(Point spot)
        {
            Resident evictee = grid[spot.X, spot.Y].getResident();
            if (evictee == null)
            {
                //no Resident occupies this space
                return false;
            }
            //go through each Atom that contains this object
            foreach (Point coordinate in getCoordinates(this.grid[spot.X, spot.Y].getPoint(),
                evictee.width, evictee.length))
            {
                grid[coordinate.X, coordinate.Y].removeResident();
            }
            return true;
        }

        public void addRiver(List<Point> startPoints, List<Point> endPoints)
        {
            if (startPoints.Count != endPoints.Count)
            {
                throw new DataMisalignedException();
            }
            for (int index = 0; index < startPoints.Count; index++)
            {
                addStream(startPoints[index], endPoints[index]);
            }
        }

        /// <summary>
        /// Creates a stream between two points. Will take the direction between the two and
        /// move one-by-one until the points meet.
        /// </summary>
        /// <param name="start.X">The x-coordinate from the first point.</param>
        /// <param name="start.Y">The y-coordinate from the first point.</param>
        /// <param name="end.X">The x-coordinate from the second point.</param>
        /// <param name="end.Y">The y-coordinate from the second point.</param>
        public void addStream(Point start, Point end)
        {
            Resident originResident = grid[start.X, start.Y].getResident();
            Resident endResident = grid[end.X, end.Y].getResident();
            Point new_end = end, new_start = start, old_start;

            int direction;
            
            while (new_start.X != new_end.X && new_start.Y != new_end.Y)
            {
                direction = closestDirection(new_start, new_end);
                old_start = new Point(new_start.X, new_start.Y);
                //getting the new coordinate from the radians
                directionSwitch(ref new_start, direction, false, 0);
            }
        }
        private bool directionSwitch(ref Point new_start, int direction, bool positive, int delta)
        {
            delta = positive ? delta + 1 : delta * -1;
            direction += delta;
            Point old_start = new Point(new_start.X, new_start.Y);
            bool overrideLegal = direction < 0 || direction > n_directions;
            //recursion limit
            if (delta > 8)
            {
                return false;
            }
            //assigning movement based on direction only if direction is between 0 and 8
            if (!overrideLegal)
            {
                switch (direction)
                {
                    case NORTH:
                        new_start.X += 0;
                        new_start.Y += 1;
                        break;
                    case NORTHEAST:
                        new_start.X += 1;
                        new_start.Y += 1;
                        break;
                    case EAST:
                        new_start.X += 1;
                        new_start.Y += 0;
                        break;
                    case SOUTHEAST:
                        new_start.X += 1;
                        new_start.Y -= 1;
                        break;
                    case SOUTH:
                        new_start.X += 0;
                        new_start.Y -= 1;
                        break;
                    case SOUTHWEST:
                        new_start.X -= 1;
                        new_start.Y -= 1;
                        break;
                    case WEST:
                        new_start.X -= 1;
                        new_start.Y += 0;
                        break;
                    case NORTHWEST:
                        new_start.X -= 1;
                        new_start.Y += 1;
                        break;
                }
            }
            //try to add stream to this location and if it fails start trying to 'wiggle' around
            if (!addStreamSingular(new_start, direction) || overrideLegal)
            {
                new_start = new Point(old_start.X, old_start.Y);
                return directionSwitch(ref new_start, direction, positive ^= positive, delta);
            }
            //add a stream to the last spot in the opposite direction
            addStreamSingular(old_start, oppositeDirection(direction));
            return true;
        }
        private bool addStreamSingular(Point spot, int direction)
        {
            return this.grid[spot.X, spot.Y].getResident().addStream(direction);
        }

        private int closestDirection(Point start, Point end)
        {
            int endX_toOrigin = end.X - start.X;
            int endY_toOrigin = end.Y - start.Y;
            //arguments flipped to reflect 0 being north instead of east
            double angle_in_radians = Math.Atan2(endX_toOrigin, endY_toOrigin) * (4 / Math.PI);
            if (angle_in_radians < 0)
            {
                angle_in_radians = -1 * (angle_in_radians - 4);
            }
            return (int)Math.Round(angle_in_radians, MidpointRounding.AwayFromZero);
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
        private List<Point> getCoordinates(Point start, int width, int length)
        {
            var points = new List<Point>();
            for (int currentX = start.X; currentX < start.X + width; currentX++)
            {
                for (int currentY = start.Y; currentY < start.Y + length; currentY++)
                {
                    points.Add(new Point(currentX, currentY));
                }
            }
            return points;
        }
        /// <summary>
        /// Get all coordinates of the grid
        /// </summary>
        /// <returns></returns>
        private List<Point> getCoordinates()
        {
            return getCoordinates(new Point(0, 0), width, length);
        }
        /// <summary>
        /// Calculates the opposite number on a clock of 8 hours (our 8 directions)
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        private int oppositeDirection(int direction)
        {
            return direction > SOUTH ? direction % SOUTH : direction + SOUTH;
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
