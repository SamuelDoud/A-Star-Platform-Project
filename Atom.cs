using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RockGarden
{
    class Atom
    {
        public static int defaultWeight = 1;
        private Resident occupant;
        //describe where the location of the base of this Resident is
        //can act as a key pair to compare objects
        private int baseX;
        private int baseY;
        private Point location;
        private int weight { get; set; }

        public Atom(Point location)
        {
            initalize(location, defaultWeight);   
        }
        public Atom(Point location, int weight)
        {
            initalize(location, weight);
        }
        public void initalize(Point location, int weight)
        {
            this.location = location;
            this.weight = weight;
            removeResident();
        }
        public void setResident(Resident member, Point spot)
        {
            occupant = member;
            baseX = spot.X;
            baseY = spot.Y;
        }
        public Point getLocation()
        {
            return location;
        }
        public void removeResident()
        {
            occupant = null;
            baseX = -1;
            baseY = -1;
        }
        public Resident getResident()
        {
            return occupant;
        }
        public int getBaseX()
        {
            return baseX;
        }
        public int getBaseY()
        {
            return baseY;
        }
        public Point getPoint()
        {
            return new Point(baseX, baseY);
        }
        public override string ToString()
        {
            if (this.occupant == null)
            {
                return " ";
            }
            return occupant.ToString();
        }
    }
}
