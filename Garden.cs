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
        /// Create a Garden of length and width.
        /// </summary>
        public Garden(int length, int width)
        {
            this.length = length;
            this.width = width;
            grid = new Atom[width, length];
            foreach (Point coordinate in getCoordinates())
            {
                grid[coordinate.X, coordinate.Y] = new Atom(coordinate);
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
            return atomAt(spot).getResident();
        }
        /// <summary>
        /// Fills empty atoms in the garden with blank gravel.
        /// </summary>
        public void fillWithGravel()
        {
            Atom tempAtom;
            foreach (Point coordinate in getCoordinates())
            {
                tempAtom = atomAt(coordinate);
                if (tempAtom.getResident() == null)
                {
                    tempAtom.setResident(new Gravel(),
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
            newNeighbor.origin = spot;
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
                atomAt(coordinate).setResident(newNeighbor, spot);
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
            Resident evictee = atomAt(spot).getResident();
            if (evictee == null)
            {
                //no Resident occupies this space
                return false;
            }
            //go through each Atom that contains this object
            foreach (Point coordinate in getCoordinates(atomAt(spot).getPoint(),
                evictee.width, evictee.length))
            {
                atomAt(coordinate).removeResident();
            }
            return true;
        }
        /// <summary>
        /// A river is a multiple Atom wide stream.
        /// The list of Points must have the same counts.
        /// </summary>
        /// <param name="startPoints">A list of Points for the river to start at.</param>
        /// <param name="endPoints">A list of Points for the river to end at.</param>
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
            return atomAt(spot).getResident().addStream(direction);
        }

        private static int closestDirection(Point start, Point end)
        {
            int endX_toOrigin = end.X - start.X;
            int endY_toOrigin = end.Y - start.Y;
            //arguments flipped to reflect 0 being north instead of east
            double angle_in_radians = Math.Atan2(endX_toOrigin, endY_toOrigin) * (4 / Math.PI);
            if (angle_in_radians < 0)
            {
                angle_in_radians = (n_directions + angle_in_radians);
            }
            return ((int)Math.Round(angle_in_radians, MidpointRounding.AwayFromZero)) % n_directions;
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
        private static List<Point> getCoordinates(Point start, int width, int length)
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
        private static int oppositeDirection(int direction)
        {
            return direction > SOUTH ? direction % SOUTH : direction + SOUTH;
        }
        /// <summary>
        /// Gets the Atoms in a specified area.
        /// </summary>
        /// <param name="origin">The lower left corner of the neighborhood.</param>
        /// <param name="x">How many x Atoms the neighborhood spans.</param>
        /// <param name="y">How many y Atoms the neighborhood spans.</param>
        /// <returns>A list of the Atoms in this neighborhood in the form of a Neiughborhood
        /// object.</returns>
        public Neighborhood getNeighborhood(Point origin, int x, int y)
        {
            List<Point> addresses = getCoordinates(origin, x, y);
            //the neighborhood listserv...
            List<Atom> neighbors = new List<Atom>();
            foreach (Point coordinate in addresses)
            {
                neighbors.Add(atomAt(coordinate));
            }
            return new Neighborhood(neighbors);
        }
        /// <summary>
        /// Gets the Atom located at this Point in the Garden.
        /// Used to eliminate clunky use of grid[p.X, p.Y].
        /// </summary>
        /// <param name="location">The location of the Atom desired.</param>
        /// <returns>The Atom at the passed location.</returns>
        private Atom atomAt(Point location)
        {
            try
            {
                return grid[location.X, location.Y];
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("This location: (" + location.X + ", " + location.Y + " is not in the Garden.");
                throw;
            }
        }

        /// <summary>
        /// Evaluates the garden for symmetry by checking for relative symmetry on 4 axis
        /// N-S, E-W, NE-SW, and NW-SE.
        ///
        /// </summary>
        /// <returns>A double from 0 (unique) to 1 (full symmetry) describing the symmetry</returns>
        public double symmetric(int jigger)
        {
            //Gravel matches aren't that big of a deal.
            //More interested in Rocks
            //We are going to take the Hamming distance of the strings by "Jiggering" them by some integer (call it jigger) in each direction
            //Unlike Hamming we are only going to take the count of the Rs that do match up to the other neighborhood.
            //Then we will divide this number by the min number of Rs in each comparsion section times J^2 (so the number of Rs in N for E-W and NE in NW-SE)
            //If perfectly symmetrical, this number will be 1.
            double slope = length / (width + 0.0), product = 1;
            
            Point tempPoint;
            Atom tempAtom;
            //select the minimum number of rocks

            Neighborhood north, south, east, west, northeast, southwest, northwest, southeast;
            north = getNeighborhood(new Point(0, length / 2), width, length - (length / 2));
            south = getNeighborhood(new Point(0, 0), width, (length / 2));
            east = getNeighborhood(new Point(0, 0), (width / 2), length);
            west = getNeighborhood(new Point(0, (width / 2)), width - (width / 2), length);
            northeast = new Neighborhood();
            southeast = new Neighborhood();
            northwest = new Neighborhood();
            southwest = new Neighborhood();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < length; y++)
                {
                    tempPoint = new Point(x, y);
                    tempAtom = atomAt(tempPoint);
                    if (y > slope * x)
                    {
                        //in NW
                        northwest.addAtom(tempAtom);
                    }
                    else
                    {
                        //in SE
                        southeast.addAtom(tempAtom);
                    }
                    if (y > (-1 * slope) * x + length)
                    {
                        //in NE
                        northeast.addAtom(tempAtom);
                    }
                    else
                    {
                        //in SW
                        southwest.addAtom(tempAtom);
                    }
                }
            }
            //do all comparisons
            product *= symmetric(north, south, jigger);
            product *= symmetric(east, west, jigger);
            product *= symmetric(northwest, southeast, jigger);
            product *= symmetric(northeast, southwest, jigger);
            return product;
        }
        /// <summary>
        /// Go through every point in the upper neighborhood and compare it to the lower.
        /// Need to implement how to refernce points (Offsetting)
        /// </summary>
        /// <param name="upper"></param>
        /// <param name="lower"></param>
        /// <param name="jigger"></param>
        /// <returns></returns>
        private double symmetric(Neighborhood upper, Neighborhood lower, int jigger)
        {
            int totalRocks;
            int upperRocks = upper.numberOfRocks();
            int lowerRocks = lower.numberOfRocks();
            totalRocks = upperRocks < lowerRocks ? upperRocks : lowerRocks;
            double product = 1;
            
            foreach(Atom a in upper.getAtoms())
            {
                product *= singleSymmetry(a, lower, jigger, a.getLocation());
            }
            return product;
        }
        /// <summary>
        /// Counts the number of hits and return how many hits occured against how many
        /// were possible.
        /// </summary>
        /// <param name="atomCompare"></param>
        /// <param name="neighboorhoodCompare"></param>
        /// <param name="jigger"></param>
        /// <param name="atomLocation"></param>
        /// <returns></returns>
        private double singleSymmetry(Atom atomCompare, Neighborhood neighboorhoodCompare, int jigger, Point atomLocation)
        {

            string atomCompareString = atomCompare.ToString();
            //temp. Only care about rocks
            if (!atomCompareString.Equals(Rock.rockString))
            {
                return 0;
            }
            Point comparePoint;
            int countHits = 0, countAll = 0;
            for (int xDelta = -1 * jigger; xDelta <= jigger; xDelta++)
            {
                for (int yDelta = -1 * jigger; yDelta <= jigger; yDelta++)
                {
                    comparePoint = new Point(atomLocation.X + xDelta, atomLocation.Y + yDelta);
                    try
                    {
                        if (atomCompareString.Equals(neighboorhoodCompare.getAtom(comparePoint).ToString()))
                        {
                            countHits++;
                        }
                        countAll++;
                    }
                    finally
                    {

                    }
                }
            }
            //prevent division by zero errors
            return countAll != 0? countHits / (countAll + 0.0): 0;
        }

        /// <summary>
        /// Get a string representation of the Rock Garden.
        /// </summary>
        /// <returns>A string which gives an ASCII art view of the rock garden</returns>
        public override string ToString()
        {
            
            string gardenString = "   ";
            string bottom = "   ";
            for (int i = 0; i < width; i++)
            {

                gardenString += i.ToString();
                if (i < 10)
                {
                    gardenString += " ";
                }
                bottom += "__";
            }
            gardenString += "\n" + bottom + "\n" + length + ( length > 10 ?"| ": " | ");
            for (int y = length - 1; y >= 0; y-- )
            {
                //counting down as the (0, 0) is located in the bottom right corner
                for (int x = 0; x < width; x++)
                {
                    gardenString += grid[x, y].ToString() + " ";
                }
                //going to a new row so we will need a new line
                gardenString += "\n" + y;
                if (y < 10)
                {
                    gardenString += " ";
                }
                gardenString += "| ";
            }
            return gardenString;
        }
    }
}
