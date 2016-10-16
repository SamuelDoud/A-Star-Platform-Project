﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RockGarden
{
    class Resident
    {
        public bool isGravel;
        public bool hasStream { get; set; }
        public int width, height, length;
        public Point origin, center;
        public string stringRepresentation = " ";
        public Resident(int width, int length, int height,  bool isGravel)
        {
            this.width = width;
            this.length = length;
            this.height = height;
            this.isGravel = isGravel;
            setCenter();
        }
        public bool gravel()
        {
            return isGravel;
        }
        public int getArea()
        {
            return width * length;
        }
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

        public double distanceToEdge(double slope)
        {
            double slopeFromCorners = length / width;
            double centerY = origin.Y + length / 2.0, centerX = origin.X + width / 2.0;
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
        public bool isInResident(Point check)
        {
            return (check.Y >= origin.Y && check.Y <= origin.Y + length &&
                    check.X >= origin.X && check.X <= origin.X + width);
        }
        public virtual bool addStream(int direction)
        {
            return false;
        }
        public override string ToString()
        {
            return stringRepresentation;
        }
    }
}
