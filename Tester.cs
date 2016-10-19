using System;
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
            GardenEvaluator ge = new GardenEvaluator();
            
            this.width = width;
            this.length = length;
            testGarden = new Garden(length + 5, width);
            fillWithGravel();
            addResident(new Rock(2, 2), new Point(12, 12), false);
            addResident(new Rock(2, 2), new Point(2, 8), false);
            addResident(new Rock(2, 2), new Point(15, 3), false);
            fillWithGravel();
            //addResident(new Rock(10, 10), new Point(3, 4), true);
            fillWithGravel();
            //should be the only rock that is in the garden
            //removeResident(new Point(3, 5));
            fillWithGravel();
            addStream(new Point(12, 2), new Point (2, 19));
            List<Point> starts = new List<Point>();
            List<Point> ends = new List<Point>();
            starts.Add(new Point(0, 0));
            starts.Add(new Point(0, 1));
            ends.Add(new Point(4, 4));
            ends.Add(new Point(4, 5));
            addRiver(starts, ends);
            double score = ge.scoreGarden(testGarden);
            Console.WriteLine(ge.scoreGarden(testGarden));
            List<int[]> combos = new List<int[]>();
            GardenEvaluator.combinations(ref combos, 0, 0, 3, 5, new int[3]);
            
        }
        
        public void addRiver(List<Point> starts, List<Point> ends)
        {
            testGarden.addRiver(starts, ends);
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
