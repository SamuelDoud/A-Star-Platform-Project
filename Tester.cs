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
            fillWithGravel();
            addResident(new Rock(10, 10), 3, 4, true);
            fillWithGravel();
            //should be the only rock that is in the garden
            removeResident(3, 5);
            fillWithGravel();
            addStream(1, 2, 5, 9);
            
        }
        public bool addResident(Resident resident, int x, int y, bool overwrite)
        {
            return testGarden.addResident(resident, x, y, overwrite);
        }
        public bool removeResident(int x, int y)
        {
            return testGarden.removeResident(x, y);
        }
        public void fillWithGravel()
        {
            testGarden.fillWithGravel();
        }
        public void addStream(int x_0, int y_0, int x_1, int y_1)
        {
            testGarden.addStream(x_0, y_0, x_1, y_1);
        }

        public override string ToString()
        {
            return testGarden.ToString();
        }
    }
}
