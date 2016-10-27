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
            String s = "sss";
            s.sub
            GardenEvaluator ge = new GardenEvaluator();
            this.width = width;
            this.length = length;
            testGarden = new Garden(length, width);
            fillWithGravel();
            addResident(new Rock(2, 2), new Point(10, 12), false);
            addResident(new Rock(2, 2), new Point(1, 12), false);
            addResident(new Rock(2, 2), new Point(16, 12), false);
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
            Garden best = geneticSelection(testGarden, 20, 3, 20);
            Console.WriteLine(ge.scoreGarden(best));
            Console.WriteLine(best.ToString());
            Console.ReadLine();
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
        public Garden geneticSelection(Garden baseGarden, int steps, int movementMax, int numberOfChildren)
        {
            GardenEvaluator ge = new GardenEvaluator();
            List<Garden> ancestors = new List<Garden>();
            ancestors.Add(baseGarden);
            for (int i = 0; i < steps; i++)
            {
                ancestors = nextStep(ancestors, numberOfChildren, movementMax - (i * movementMax / steps), ge);
            }
            return ancestors[0];
        }
        public List<Garden> nextStep(List<Garden> ancestor, int numberofChildren, int movementMax, GardenEvaluator ge)
        {
            List<Tuple<Garden, double>> allDecendants = new List<Tuple<Garden, double>>();
            List<Garden> tempGardens = new List<Garden>();
            Tuple<Garden, double> tempTuple;
            double tempScore;
            for (int i = 0; i < numberofChildren && i < ancestor.Count; i++)
            {
                tempGardens.AddRange(makeChildren(ancestor[i], movementMax, numberofChildren));
                foreach (Garden child in tempGardens)
                {
                    tempScore = ge.scoreGarden(child);
                    tempTuple = new Tuple<Garden, double>(child, tempScore);
                    allDecendants.Add(tempTuple);
                }
            }
            tempGardens = new List<Garden>();
            //Order by the top scoring gardens
            allDecendants = allDecendants.OrderByDescending(score => score.Item2).ToList();
            for(int i = 0; i < numberofChildren; i++)
            {
                tempGardens.Add(allDecendants[i].Item1);
            }
            return tempGardens;
        }
        public List<Garden> makeChildren(Garden parent, int movementMax, int numberOfChildren)
        {
            List<Resident> allRocksInParent = parent.getAllOfType(Rock.rockString);
            List<Garden> childrenOfTheParentGarden = new List<Garden>();
            for (int childNumber = 0; childNumber < numberOfChildren; childNumber++)
            {
                childrenOfTheParentGarden.Add(makeChild(parent.length, parent.width, allRocksInParent, movementMax));
            }
            return childrenOfTheParentGarden;
        }

        public Garden makeChild(int length, int width, List<Resident> rocks, int movementMax)
        {
            Garden child = new Garden(length, width);
            Random gen = new Random();
            Point tempLocation = new Point();
            foreach (Resident rock in rocks)
            {
                tempLocation = rock.origin;
                tempLocation.X += gen.Next(-1 * movementMax, movementMax);
                tempLocation.Y += gen.Next(-1 * movementMax, movementMax);
                tempLocation.X = (tempLocation.X + width) % width;
                tempLocation.Y = (tempLocation.Y + length) % length;
                
                child.addResident(new Rock(rock.length, rock.width), tempLocation, false);
            }
            child.fillWithGravel();
            if (child.getAllOfType(Rock.rockString).Count != rocks.Count)
            {
                child = makeChild(length, width, rocks, movementMax);
            }

            Console.WriteLine(child);

            return child;

        }
        public Garden getBestGarden()
        {
            return null;
        }
    }
}
