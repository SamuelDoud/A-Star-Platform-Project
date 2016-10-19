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
        
        public static double defaultHeuristic = 1.0, defaultRockHeuristic = int.MaxValue, defaultRockBorderHeuristic = defaultHeuristic / 2;
        private Resident occupant;
        //describe where the location of the base of this Resident is
        private Point location;
        public double heuristic { get; set; }
        private double backgroundRockHueristic;
        public double rockHeuristic
        {
            get
            {
                return backgroundRockHueristic;
            }
            set
            {
                setRockHeuristic();
            }
        }
        public double rockBorderHeuristic { get; set; }


        public Atom(Point location)
        {
            initalize(location, defaultHeuristic);   
        }
        public Atom(Point location, int heuristic)
        {
            initalize(location, heuristic);
        }
        public void initalize(Point location, double heuristic)
        {
            this.location = location;
            this.heuristic = heuristic;
            rockHeuristic = defaultRockHeuristic;
            rockBorderHeuristic = defaultRockBorderHeuristic;
            removeResident();
        }
        public void setResident(Resident member, Point spot)
        {
            occupant = member;
            setRockHeuristic();
        }
        public Point getLocation()
        {
            return location;
        }
        public void removeResident()
        {
            occupant = null;
            heuristic = defaultHeuristic;
        }
        public Resident getResident()
        {
            return occupant;
        }
        public void rockBorder()
        {
            heuristic = defaultRockBorderHeuristic;
        }
        private void setRockHeuristic()
        {
            if (ToString().Equals(Rock.rockString))
            {
                heuristic = backgroundRockHueristic;
            }
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
