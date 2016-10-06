﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

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
            testGarden = new Garden(length + 5, width);
            fillWithGravel();
            addResident(new Rock(2, 4), new Point(2, 4), false);
            addResident(new Rock(3, 4), new Point(2, 5), false);
            fillWithGravel();
            addResident(new Rock(10, 10), new Point(3, 4), true);
            fillWithGravel();
            //should be the only rock that is in the garden
            removeResident(new Point(3, 5));
            fillWithGravel();
            addStream(new Point(1, 2), new Point (5, 9));
            
        }
        public bool addResident(Resident resident, Point spot, bool overwrite)
        {
            return testGarden.addResident(resident, spot, overwrite);
        }
        public bool removeResident(Point spot)
        {
            return testGarden.removeResident(spot);
        }
        public void fillWithGravel()
        {
            testGarden.fillWithGravel();
        }
        public void addStream(Point start, Point end)
        {
            testGarden.addStream(start, end);
        }

        public override string ToString()
        {
            return testGarden.ToString();
        }
    }
}
