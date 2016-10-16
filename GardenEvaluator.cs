using System;
using System.Collections.Generic;
using System.Linq;

namespace RockGarden
{
    class GardenEvaluator
    {
        private static double defaultBalanceWeight = 1.0, defaultSymmetryWeight = 1.0;
        private static int defaultJigger = 5;
        private double balanceWeight { get; set; }
        private double symmetryWeight { get; set; }
        private int jigger { get; set; }
        public GardenEvaluator()
        {
            balanceWeight = defaultBalanceWeight;
            symmetryWeight = defaultSymmetryWeight;
            jigger = defaultJigger;
        }
        public double scoreGarden(Garden toScore)
        {
            double symmetry = toScore.symmetric(jigger) * symmetryWeight;
            double balance = balanceOfGarden(toScore) * balanceWeight;
            return symmetry * balance;
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
            return Math.Sqrt(sumOfVariance / (nRocksPerHalf.Count - 1)) / maxStDev(average, halves.Length);
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
    }
}
