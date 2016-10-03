using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RockGarden
{
    class Tester
    {
        private Garden testGarden;
        private int width, length;
        public Tester(int length, int width)
        {
            this.width = width;
            this.length = length;
            testGarden = new Garden(width, length);
            fillWithGravel();
            addResident(new Rock(2, 4), 2, 4, false);
            addResident(new Rock(3, 4), 2, 5, false);
            addResident(new Rock(10, 10), 3, 4, true);
            //should be the only rock that is in the garden
        }
        public bool addResident(Resident resident, int x, int y, bool overwrite)
        {
            return testGarden.addResident(resident, x, y, overwrite);
        }
        public void fillWithGravel()
        {
            testGarden.fillWithGravel();
        }
        public override string ToString()
        {
            return testGarden.ToString();
        }
    }
}
