using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace RockGarden
{
    class GardenEvaluator
    {
        private static double defaultBalanceWeight = 1.0, defaultSymmetryWeight = 1.0, defaultLinearityWeight = 1.0;
        private static double defaultStartScaleDownDegrees = 15.0, defaultZeroScoreDegrees = 5.0; 
        private static int defaultJigger = 5;
        public double balanceWeight { get; set; }
        public double symmetryWeight { get; set; }
        public double linearityRootWeight { get; set; }
        public double startScaleDownDegrees { get; set; }
        public double zeroScoreDegrees { get; set; }
        private int jigger { get; set; }
        public GardenEvaluator()
        {
            balanceWeight = defaultBalanceWeight;
            symmetryWeight = defaultSymmetryWeight;
            linearityRootWeight = defaultLinearityWeight;
            startScaleDownDegrees = defaultStartScaleDownDegrees;
            zeroScoreDegrees = defaultZeroScoreDegrees;
            jigger = defaultJigger;
        }
        /// <summary>
        /// Score the Garden passed based on a variety of methods.
        /// </summary>
        /// <param name="toScore">The Garden to score.</param>
        /// <returns>The score of a Garden, the higher the better.</returns>
        public double scoreGarden(Garden toScore)
        {
            //should pull this method into this class
            double symmetry = toScore.symmetric(jigger, symmetryWeight);
            double balance = balanceOfGarden(toScore);
            double nonLinearity = scoreLinearity(toScore);
            return symmetry * balance * nonLinearity;
        }
        /// <summary>
        /// Find how much any half the Garden dominates the other half.
        /// Finds the standard deviation of nRocks from each half.
        /// We seek to avoid one half having 50 rocks while the other has 0
        /// </summary>
        /// <param name="toScore"></param>
        /// <returns></returns>
        private double balanceOfGarden(Garden toScore)
        {
            Neighborhood[] halves = toScore.allHalves();
            List<double> nRocksPerHalf = new List<double>();
            double sumOfVariance = 0, average;
            foreach (Neighborhood half in halves)
            {
                nRocksPerHalf.Add(half.numberOfRocks());
            }
            average = nRocksPerHalf.Average();
            foreach (double nRocks in nRocksPerHalf)
            {
                sumOfVariance += Math.Pow(nRocks - average, 2);
            }
            return Math.Sqrt(sumOfVariance / (nRocksPerHalf.Count - 1)) / maxStDev(average, halves.Length) * balanceWeight;
        }

        /// <summary>
        /// Finds the maximum standard eviation given an average and an N.
        /// </summary>
        /// <param name="average"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private double maxStDev(double average, int n)
        {
            //max is if aveage is (n+m / 2) where n or m = 0
            //since we are squaring all variances, assume 0 is what compprises the entire list.
            double sumOfVariance = n * Math.Pow(average, 2);
            return Math.Sqrt(sumOfVariance / (n -1));
        }
        /// <summary>
        /// Takes two preferably disjoint Neighborhoods and measures if one has
        /// a dominant amount of Rocks over the other.
        /// This score should be very weak in the case where there are few rocks as it may not
        /// be feasible to cover all distinct halves. This can be controlled with the
        /// "balanceOfNeighborhoods" property.
        /// </summary>
        /// <param name="upper">One neighborhood to measure.</param>
        /// <param name="lower">The other neighborhood to measure.</param>
        /// <returns></returns>
        private double balanceOfNeighborhoods(Neighborhood upper, Neighborhood lower)
        {
            int nRocksUpper = upper.numberOfRocks();
            int nRocksLower = upper.numberOfRocks();
            double resultRatio;
            if (nRocksUpper >= nRocksLower)
            {
                //adding 1.0 to cast to double and prevent division by zero
                resultRatio = (nRocksLower + 1.0) / (nRocksUpper + 1.0);
            }
            else
            {
                resultRatio = (nRocksUpper + 1.0) / (nRocksLower + 1.0);
            }
            return resultRatio;
        }
        /// <summary>
        /// Determine all combinations of k Residents from residentsList.
        /// </summary>
        /// <param name="residentsList">A List of Residents to be selected from.</param>
        /// <param name="k">How many Residents each selection should have.</param>
        /// <returns>All combinations of k Residents from residents List as a 2d array
        /// of Residents.</returns>
        private Resident[,] NChooseK(List<Resident> residentsList, int k)
        {
            int n = residentsList.Count;
            if (k > n)
            {
                return new Resident[0, k];
            }
            int selectionsNum = factorial(n) / (factorial(k) * factorial(n - k));
            Resident[,] combinatorics = new Resident[selectionsNum, k];
            List<int[]> combinationIndecies = new List<int[]>();
            combinations(ref combinationIndecies, 0, 0, k, n, new int[k]);
            for(int i = 0; i < combinationIndecies.Count; i++)
            {
                for (int q = 0; q < k; q++)
                {
                    combinatorics[i, q] = residentsList[combinationIndecies[i][q]];
                }
            }
            return combinatorics;
        }
        /// <summary>
        /// Calculate all the combinations of n of size k.
        /// Inputs combinations into combinationsList via reference.
        /// </summary>
        /// <param name="combinationIndecies">This is the list that will have combinations added to
        /// it via reference. Must be empty on passing in.</param>
        /// <param name="startIndex">What number is the minimum number to draw from. Pass zero.</param>
        /// <param name="level">Internal parameter. Pass zero.</param>
        /// <param name="k">How many indecies to be selected.</param>
        /// <param name="n">How many indecies to select from.</param>
        /// <param name="combo">Internal parameter, pass "new int[k]"</param>
        public void combinations(ref List<int[]> combinationIndecies, int startIndex, int level, int k, int n, int[] combo)
        {
            if (level == k)
            {
                int[] copyArr = new int[combo.Length];
                combo.CopyTo(copyArr, 0);
                combinationIndecies.Add(copyArr);
                return;
            }
            for (int i = startIndex; n - i + 1 >= k - level && i < n; i++)
            {
                combo[level] = i;
                combinations(ref combinationIndecies, i + 1, level + 1, k, n, combo);
            }
        }
        /// <summary>
        /// Score the Garden based on the linearity of the Rocks in it.
        /// If Rocks are determined to be linear (or close to linear), the number returned will
        /// be less than 1, allowing the evaluator to reduce the score of the Garden. If no Rocks
        /// are linear, then this method will return a value 
        /// </summary>
        /// <param name="garden"></param>
        /// <returns></returns>
        private double scoreLinearity(Garden garden)
        {
            double score = 1;
            int triangle = 3;
            List<double> scores = new List<double>();
            List<Resident> allRocksInGarden = getAllResidents(garden, Rock.rockString);
            List<Resident[]> AllRockTriangles = new List<Resident[]>();
            List<int[]> pickThree = new List<int[]>();
            Resident[] tempResidents = new Resident[triangle], tempResidentsCopy = new Resident[triangle];
            
            combinations(ref pickThree, 0, 0, triangle, allRocksInGarden.Count, new int[triangle]);
            foreach (int[] combo in pickThree)
            {
                for(int i = 0; i < triangle; i++)
                {
                    tempResidents[i] = allRocksInGarden[combo[i]];
                }
                tempResidents.CopyTo(tempResidentsCopy, 0);
                AllRockTriangles.Add(tempResidentsCopy);
            }
            foreach(Resident[] triangleArrrangement in AllRockTriangles)
            {
                scores.Add(Math.Pow(triangleLinearity(triangleArrrangement), 1 / linearityRootWeight));
            }
            if (scores.Count > 0)
            {
                return scores.Average();
            }
            return score;
        }

        /// <summary>
        /// Scores the linearity of the a set of three residents based on the smallest angle
        /// as determined by creating a triangle from their centers.
        /// Scales from 0 to 1 based on a linear scale as determined by the properties
        /// "startScaleDownDegrees" and "zeroScoreDegrees". If the smallest angle is greater than
        /// "startScaleDownDegrees" then return 1. If less than "zeroScoreDegrees", return 0.
        /// If between, return angle - "zeroScoreDegrees" divided by range of the two properties.
        /// This value will be taken the the "linearityRootScore"-th root 
        /// (a property defaulted to 1).
        /// </summary>
        /// <param name="threeResidents">the Residents from which a triangle will be created.</param>
        /// <returns>The linearity score. Double value from 0 to 1.</returns>
        private double triangleLinearity(Resident[] threeResidents)
        {
            double radians, degrees, score = 1;
            int minIndex = 0, otherIndex1, otherIndex2;
            double firstSecondDistance = distance(threeResidents[0].center, threeResidents[1].center);
            double secondThirdDistance = distance(threeResidents[1].center, threeResidents[2].center);
            double thirdFirstDistance = distance(threeResidents[2].center, threeResidents[0].center);
            double[] lengths = new double[]{firstSecondDistance, secondThirdDistance, thirdFirstDistance};
            if (lengths[minIndex] > lengths[1])
            {
                minIndex = 1;
            }
            if (lengths[minIndex] > lengths[2])
            {
                minIndex = 2;
            }
            otherIndex1 = (minIndex + 1) % 3;
            otherIndex2 = (minIndex + 2) % 3;
            //Law of cosines to find the smallest angle (opposite the smallest side)
            // c^2 = a^2 + b^2 - 2abCos(C)
            // Cos(C) = (a^2 + b^2 - c^2) / (2ab)
            // C = cos^-1 [(a^2 + b^2 - c^2) / (2ab)]
            radians = Math.Acos((Math.Pow(lengths[otherIndex1], 2) + Math.Pow(lengths[otherIndex2], 2) - Math.Pow(lengths[minIndex], 2)) / (2 * lengths[otherIndex1] * lengths[otherIndex2]));
            degrees = toDegrees(radians);
            //only modify the score if it is close to zero, and if its below zeroScore, set to 0.
            if (degrees < startScaleDownDegrees)
            {
                //slope = 1 / (startScore - zeroScore)
                //y
                //0 = (slope)(zeroScore)
                score = Math.Max((degrees - zeroScoreDegrees) / (startScaleDownDegrees - zeroScoreDegrees), 0);
            }
            return score;
        }

        //the search type probably should be an integer since thos comparisons are faster than equals.
        //future refactor.
        /// <summary>
        /// Gets all the residents in the Garden of a certain type denoted by the string type.
        /// </summary>
        /// <param name="garden">The Garden to be searched.</param>
        /// <param name="type">The string of the object being searched.</param>
        /// <returns>A List of the Residents that match the type passed.</returns>
        private List<Resident> getAllResidents(Garden garden, string type)
        {
            List<Resident> allResidents = garden.residentsList;
            List<Resident> specificResidents = new List<Resident>();
            foreach (Resident resident in allResidents)
            {
                if (resident.ToString().Equals(type))
                {
                    specificResidents.Add(resident);
                }
            }
            return specificResidents;
        }
        /// <summary>
        /// Calculates the factorial of n. n!
        /// </summary>
        /// <param name="n">The integer to have its factorial calculated. Due to the return
        /// type of an integer, the range is |n| leq 12</param>
        /// <returns>The factorial of the number as an integer.</returns>
        private static int factorial(int n)
        {
            int product = 1;
            for (int i = 1; i <= n; i++)
            {
                product *= i;
            }
            return product;
        }

        /// <summary>
        /// Converts a radian measure to degrees.
        /// </summary>
        /// <param name="radians">The radian measure to be converted.</param>
        /// <returns>The equivelent degree measure.</returns>
        public static double toDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }
        /// <summary>
        /// Calculate the Euclidean distance between two points.
        /// </summary>
        /// <param name="start">The Point the measure will start from.</param>
        /// <param name="end">The Point the measure will end from.</param>
        /// <returns>The Euclidean distance between the two Points.</returns>
        private static double distance(Point start, Point end)
        {
            return Math.Sqrt(Math.Pow(start.X - end.X, 2) + Math.Pow(start.Y - end.Y, 2));
        }

    }
}
