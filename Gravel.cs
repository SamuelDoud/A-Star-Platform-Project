﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RockGarden
{
    
    class Gravel : Resident
    {
        public List<int> connections = new List<int>();
        public static string terminus = "#", blank = ".";
        public Gravel() : base(1, 1, 1, true)
        {
            stringRepresentation = blank;
            hasStream = false;
        }
        //it would make sense if the gravel is a stream that it has two "connections"
        //connections being which direction the gravel's stream faces
        public int numberConnections()
        {
            return connections.Count;
        }
        public override bool addStream(int direction)
        {
            hasStream = true;
            connections.Add(direction);
            newStringRepresentation();
            return true;
        }
        public List<int> getConnections()
        {
            return connections;
        }
        private void newStringRepresentation()
        {
            int n_connections = numberConnections();
            switch (n_connections)
            {
                case 0:
                    stringRepresentation = blank;
                    break;
                case 1:
                    stringRepresentation = terminus;
                    break;
                case 2:
                    stringRepresentation = directional(this.connections[0], this.connections[1]);
                    break;
                default:
                    //non-sense number of connections, for now.
                    stringRepresentation = "?";
                    break;
            }

        }
        private static string directional(int origin, int destination)
        {
            if (origin + destination == 4 && (origin == 0 || destination == 0))
            {
                return "|";
            }
            if(origin + destination == 8 && (origin == 2 || destination == 2))
            {
                return "-";
            }
            if (origin + destination == 6 && (origin == 1 || destination == 1))
            {
                return "/";
            }
            if (origin + destination == 10 && (origin == 3 || destination == 3))
            {
                return "\\";
            }
            if (origin + destination == 4 && (origin == 1 || destination == 1))
            {
                return "<";
            }
            if (origin + destination == 12 && (origin == 5 || destination == 5))
            {
                return ">";
            }
            if ((origin + destination == 3 && (origin == 0 || destination == 0)) || (origin + destination == 1 && (origin == 0 || destination == 0)))
            {
                return "(";
            }
            if ((origin + destination == 11 && (origin == 4 || destination == 4)) || (origin + destination == 5 && (origin == 0 || destination == 0)))
            {
                return ")";
            }
            return "~";
        }

    }
}
