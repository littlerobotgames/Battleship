using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Battleship
{
    class ShipData
    {
        public ShipData(string name, int size, string imageName)
        {
            Name = name;
            Size = size;

            SpritePath = Globals.asset_directory + imageName;
        }
        public string Name { get; set; }
       
        public int Size { get; set; }
        public string SpritePath { get; set; } 
    }
    class Ship
    {
        private ShipPart[] parts;
        private int hits;
        public Ship(string name, int x, int y, int angle, int size, string ip)
        {
            Name = name;
            X = x;
            Y = y;
            hits = size;
            Size = size;
            Angle = angle;
            Bitmap original = new Bitmap(ip);
            Sprite = new Bitmap(original, 32*size, 32);
            

            Point dirpoint = Form1.GetDirPoints(angle);
            int addX = dirpoint.X;
            int addY = dirpoint.Y;

            parts = new ShipPart[size];

            for (int i=0; i<size; i++)
            {
                parts[i] = new ShipPart(Name, x+(addX * i), y+(addY * i));
            }
        }
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public Bitmap Sprite { get; set; }
        public int Angle { get; set; }
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
        public bool AtPlace(int x, int y)
        {
            for (int i = 0; i < parts.Length; i++)
            {
                ShipPart temppart = parts[i];

                if (temppart.X == x && temppart.Y == y)
                {
                    return true;
                }
            }

            return false;
        }
        public Bitmap DrawShip()
        {
            Bitmap blankbit = new Bitmap(Sprite.Width, Sprite.Height);

            using (Graphics g = Graphics.FromImage(blankbit))
            {
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

                g.TranslateTransform(16, 16);
                g.RotateTransform(Angle * 90);
                g.TranslateTransform(-16, -16);

                g.DrawImage(Sprite, new Point(0, 0));
            }

            return blankbit;
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
