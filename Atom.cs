using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockGarden
{
    class Atom
    {
        private Resident occupant;
        //describe where the location of the base of this Resident is
        //can act as a key pair to compare objects
        private int baseX;
        private int baseY;
        public Atom()
        {
            this.removeResident();
        }

        public void setResident(Resident member, int baseX, int baseY)
        {
            this.occupant = member;
            this.baseX = baseX;
            this.baseY = baseY;
        }
        public void removeResident()
        {
            this.occupant = null;
            this.baseX = -1;
            this.baseY = -1;
        }
        public Resident getResident()
        {
            return this.occupant;
        }
        public int getBaseX()
        {
            return this.baseX;
        }
        public int getBaseY()
        {
            return this.baseX;
        }
    }
}
