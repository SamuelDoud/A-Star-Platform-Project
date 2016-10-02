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
        public Atom()
        {
            this.occupant = new Resident();
        }

        public void setResident(Resident member)
        {
            this.occupant = member;
        }

        public Resident getResident()
        {
            return this.occupant;
        }
    }
}
