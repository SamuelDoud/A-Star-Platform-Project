using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RockGarden
{
    class Neighborhood
    {
        public static int defaultScore = 1;
        private List<Atom> atoms;
        private double score { get; set; }
        /// <summary>
        /// Constructs an empty Neighborhood with a default score.
        /// </summary>
        public Neighborhood()
        {
            initialize(new List<Atom>(), defaultScore);
        }
        /// <summary>
        /// Constructs a Neighborhood with members and a default score.
        /// </summary>
        /// <param name="atoms">The atoms that will comprise the Neighborhood.</param>
        public Neighborhood(List<Atom> atoms)
        {
            initialize(atoms, defaultScore);
        }
        /// <summary>
        /// Constructs a Neighborhood of atoms with score weight.
        /// </summary>
        /// <param name="atoms">The atoms that will comprise the Neighborhood.</param>
        /// <param name="score">The score or weight of the Neighborhood</param>
        public Neighborhood(List<Atom> atoms, double score)
        {
            initialize(atoms, score);
        }
        /// <summary>
        /// Constructs the Neighborhood on the contract model.
        /// </summary>
        /// <param name="atoms">The atoms that will comprise the Neighborhood.</param>
        /// <param name="score">The score or weight of the Neighborhood</param>
        private void initialize(List<Atom> atoms, double score)
        {
            this.atoms = atoms;
            this.score = score;
        }

        /// <summary>
        /// Find the Atom located at the Point location.
        /// If the Point does not exist, throw a KeyNotFoundException.
        /// Unintelligent algorithm. Could improve since neighborhood Atoms must share borders.
        /// </summary>
        /// <param name="location">The Point to search the Neighborhood for.</param>
        /// <returns></returns>
        public Atom getAtom(Point location)
        {
            foreach(Atom atom in atoms)
            {
                if (location.Equals(atom.getPoint()))
                {
                    return atom;
                }
            }
            throw new KeyNotFoundException("This location is not in this Neighborhood.");
        }

        /// <summary>
        /// Add an Atom to the List of Atoms.
        /// </summary>
        /// <param name="a">The atom to be added to the List of Atoms.</param>
        public void addAtom(Atom a)
        {
            atoms.Add(a);
        }

        /// <summary>
        /// Accessor for the Atoms in this Neighborhood.
        /// </summary>
        /// <returns>All the Atoms in this Neighborhood in the form of a List.</returns>
        public List<Atom> getAtoms()
        {
            return atoms;
        }

        /// <summary>
        /// Searches all Atoms and finds the number of Atoms that have Rocks.
        /// </summary>
        /// <returns>The number of Rocks in this Neighborhood</returns>
        public int numberOfRocks()
        {
            return numberOfString(Rock.rockString);
        }

        /// <summary>
        /// Searches all Atoms and checks if SearchType matches the Atom's toString()
        /// </summary>
        /// <param name="searchType"></param>
        /// <returns></returns>
        private int numberOfString(string searchType)
        {
            int n_string = 0;
            foreach (Atom a in atoms)
            {
                if (a.getResident().ToString().Equals(searchType))
                {
                    n_string++;
                }
            }
            return n_string;
        }
    }
}
