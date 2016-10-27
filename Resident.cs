using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RockGarden
{
    class Resident
    {
        public bool isGravel { get; set; }
        public bool hasStream { get; set; }
        public int width, height, length;
        private Point backgroundOrigin;
        public Point origin { get { return backgroundOrigin; } set { backgroundOrigin = value; setCenter(); } }
        public Point center { get; set; }
        public string stringRepresentation = " ";
        public double hueristic { get; set; }
        /// <summary>
        /// Create a Resident.
        /// </summary>
        /// <param name="width">How many Atoms in the X direction the Resident will occupy.</param>
        /// <param name="length">How many Atoms in the Y direction the Resident will occupy.</param>
        /// <param name="height">Not active.</param>
        /// <param name="isGravel">Whether the Resident is a gravel subclass
        /// (special rules apply).</param>
        public Resident(int width, int length, int height, bool isGravel)
        {
            this.width = width;
            this.length = length;
            this.height = height;
            this.isGravel = isGravel;
            setCenter();
        }
        /// <summary>
        /// Gets the area that this Resident occupies.
        /// </summary>
        /// <returns>The area that this Resident occupies.</returns>
        public int getArea()
        {
            return width * length;
        }
        /// <summary>
        /// Internal method to set the center Point of this object.
        /// </summary>
        private void setCenter()
        {
            double thisCenterX = (origin.X + width / 2.0);
            double thisCenterY = (origin.Y + length / 2.0);
            center = new Point((int)Math.Round(thisCenterX, MidpointRounding.AwayFromZero),
                               (int)Math.Round(thisCenterY, MidpointRounding.AwayFromZero));
        }
        /// <summary>
        /// Finds the minimal distance from one rock to another.
        /// Straight line disregarding any obstacles.
        /// Assumed to be a line from the centers
        /// </summary>
        /// <param name="other">The resident to find the distance to.</param>
        /// <returns>A double representing the distance between this Resident and the passed
        /// Resident.</returns>
        public double getDistance(Resident other)
        {
            double otherCenterX = (other.origin.X + other.width / 2.0);
            double otherCenterY = (other.origin.X + other.length / 2.0);
            double slope = (center.Y - otherCenterY) / (center.X - otherCenterX);
            double rawDistance = Math.Sqrt(Math.Pow(center.X - otherCenterX, 2) + Math.Pow(center.Y - otherCenterY, 2));
            //negative tracking on this, positive on other
            //remove the space from centers to edges of both residents.
            double edgeDistance = rawDistance - distanceToEdge(-1 *slope) - other.distanceToEdge(slope);
            return edgeDistance; 
        }
        /// <summary>
        /// Distance to edge of the Resident from its center
        /// </summary>
        /// <param name="slope"></param>
        /// <returns></returns>
        public double distanceToEdge(double slope)
        {
            double slopeFromCorners = length / width;
            double centerY = center.Y, centerX = center.Y;
            double yEdge, xEdge;
            if (Math.Abs(slope) < slopeFromCorners)
            {
                //intersects on the length sides of the Resident
                xEdge = origin.X;
                yEdge = (width / 2.0) * slope;
            }
            else
            {
                //intersection on the width or corner
                yEdge = origin.Y;
                xEdge = slope / (length / 2.0);
            }
            return Math.Sqrt(Math.Pow(centerX - xEdge, 2) + Math.Pow(centerY - yEdge, 2));
        }
        /// <summary>
        /// Does the point is passed within the Resident?
        /// </summary>
        /// <param name="check">The Point to be checked if it is withi the Resident.</param>
        /// <returns>A bool wheteher the Point is within the Resident.</returns>
        public bool isInResident(Point check)
        {
            return (check.Y >= origin.Y && check.Y <= origin.Y + length &&
                    check.X >= origin.X && check.X <= origin.X + width);
        }
        /// <summary>
        /// By default, streams cannot be added to Residents. This can be overrode by subclasses.
        /// </summary>
        /// <param name="direction">The direction that the stream is going.</param>
        /// <returns>The success of adding the stream. Will always evaluate to false.</returns>
        public virtual bool addStream(int direction)
        {
            return false;
        }
        /// <summary>
        /// Returns a single character as a string that represents the Resident.
        /// </summary>
        /// <returns>A single character string.</returns>
        public override string ToString()
        {
            return stringRepresentation;
        }
    }
}
