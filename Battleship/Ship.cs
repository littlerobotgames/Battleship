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
        public Ship(string name, int x, int y, int angle, int size, string ip)
        {
            Name = name;
            X = x;
            Y = y;
            Hits = size;
            Size = size;
            Angle = angle;
            Bitmap original = new Bitmap(ip);
            Sprite = new Bitmap(original, size*Globals.TileSize, Globals.TileSize);
            
            Point dirpoint = Form1.GetDirPoints(angle);
            int addX = dirpoint.X;
            int addY = dirpoint.Y;

            //Create all the ship parts

            parts = new ShipPart[size];

            for (int i=0; i<size; i++)
            {
                parts[i] = new ShipPart(Name, x+(addX * i), y+(addY * i), Angle);
                parts[i].Sprite = RotateSprite(GetSpritePart(i, Sprite), Angle);

            }
            CalcCorner();
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int TopX { get; set; }
        public int TopY { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public Bitmap Sprite { get; set; }
        public int Angle { get; set; }
        public int Hits { get; set; }
        public void CalcHits()
        {
            Hits = 0;

            for (int i = 0; i < parts.Length; i++)
            {
                if (!parts[i].Hit)
                {
                    Hits++;
                }
            }
        }
        public ShipPart GetPart(int index)
        {
            return parts[index];
        }
        public bool HitPart(int x, int y)
        {
            for (int i = 0; i < parts.Length; i++)
            {
                ShipPart temppart = parts[i];

                if (temppart.X == x && temppart.Y == y)
                {                    
                    temppart.Hit = true;
                    CalcHits();

                    Console.WriteLine($"Ship {Name} has been hit! It has {Hits} lives left");

                    return true;
                }
            }
            return false;
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
        public void CalcCorner()
        {
            int lowestX = X;
            int lowestY = Y;

            for (int i = 0; i < Size; i++)
            {
                ShipPart temppart = GetPart(i);

                if (temppart.X < X)
                {
                    lowestX = temppart.X;
                }
                if (temppart.Y < Y)
                {
                    lowestY = temppart.Y;
                }
            }
            TopX = lowestX*Globals.TileSize;
            TopY = lowestY*Globals.TileSize;
        }
        public Bitmap RotateSprite(Bitmap ori, int angle)
        {
            Bitmap rotated = new Bitmap(ori.Width, ori.Height);

            using (Graphics g = Graphics.FromImage(rotated))
            {
                g.TranslateTransform(ori.Width/2, ori.Height/2);
                g.RotateTransform(angle * 90);
                g.TranslateTransform(- ori.Width/2, - ori.Height/2);

                g.DrawImage(ori, new Point(0, 0));
            }

            return rotated;
        }
        public Bitmap GetSpritePart(int part, Bitmap src)
        {
            Bitmap dest = new Bitmap(src.Width, src.Height);

            using(Graphics g = Graphics.FromImage(dest))
            {
                g.DrawImage(src, new Rectangle(0, 0, 32, 32), new Rectangle(part * 32, 0, 32, 32), GraphicsUnit.Pixel);
            }

            return dest;
        }
    }
    class ShipPart
    {
        public ShipPart(string name, int x, int y, int angle)
        {
            Name = name;
            X = x;
            Y = y;
            Hit = false;
            Angle = angle;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public bool Hit { get; set; }
        public int Angle { get; set; }
        public Bitmap Sprite { get; set; }
    }
}
