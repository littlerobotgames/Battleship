using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class ShipData
    {
        private bool[] hits;
        public ShipData(string name, int size)
        {
            Name = name;
            Size = size;
        }
        public string Name { get; set; }
       
        public int Size { get; set; }
    }
    class Ship
    {
        private ShipPart[] parts;
        private int hits;
        public Ship(string name, int x, int y, int angle, int size)
        {
            Name = name;
            X = x;
            Y = y;
            hits = size;
            Size = size;

            parts = new ShipPart[size];

            for (int i=0; i<size; i++)
            {
                parts[i] = new ShipPart(Name, x+i, y);
            }
        }
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public void CalcHits()
        {
            hits = 0;

            for (int i = 0; i < parts.Length; i++)
            {
                if (!parts[i].Hit)
                {
                    hits++;
                }
            }
        }
        public ShipPart GetPart(int index)
        {
            return parts[index];
        }
    }
    class ShipPart
    {
        public ShipPart(string name, int x, int y)
        {
            Name = name;
            X = x;
            Y = y;
            Hit = false;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public bool Hit { get; set; }
    }
}
