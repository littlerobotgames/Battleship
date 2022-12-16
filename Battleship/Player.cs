using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship
{
    enum guessStates
    {
        None = 0,
        Miss,
        Hit
    }
    class Player
    {
        public Ship[] ships;
        private guessStates[,] guesses;
        public Player(int mapW, int mapH)
        {
            ships = new Ship[5];
            guesses = new guessStates[mapW, mapH];
            PlacingShip = 0;
        }
        public int PlacingShip { get; set; }
        public bool PlaceShip(Ship ship)
        {
            if (PlaceClear(ship))
            {
                ships[PlacingShip] = ship;
                Console.WriteLine($"Placing a {ship.Name} at ({ship.X}, {ship.Y})");
                return true;
            }

            Console.WriteLine($"Failed to place a {ship.Name} at ({ship.X}, {ship.Y})");
            return false;
        }
        public bool PlaceClear(Ship ship)
        {
            int clear = 0;
            int aim_clear = ship.Size;

            for (int i = 0; i < ship.Size; i++)
            {
                ShipPart temppart = ship.GetPart(i);
                int partX = temppart.X;
                int partY = temppart.Y;

                if (partX>=10 || partX < 0 || partY >= 10 || partY < 0)
                {
                    return false;
                }

                if (PlacingShip == 0)
                {
                    clear = aim_clear;
                }
                else
                {
                    if (!ShipAt(partX, partY))
                    {
                        clear++;
                    }
                }              
            }

            if (clear == aim_clear)
            {
                Console.WriteLine($"{GetType().Name}: Place is clear to place a ship");
                return true;
            }
            else
            {
                Console.WriteLine("Place is not clear to place a ship");
                return false;
            }
        }
        public bool ShipAt(int x, int y)
        {
            for(int s = 0;s<ships.Length; s++)
            {
                Ship tempship = ships[s];

                if (tempship != null)
                {
                    if (tempship.AtPlace(x, y))
                    {
                        Console.WriteLine($"Ship at place ({x}, {y})");
                        return true;
                    }
                }               
            }
            Console.WriteLine($"No ship at place ({x}, {y})");
            return false;
        }
    }
    class Human : Player
    {
        public Human(int mapW, int mapH) : base(mapW, mapH)
        {

        }
    }
    class Bot : Player
    {
        private ShipData[] allships;
        private Random ran = new Random();
        public Bot(int mapW, int mapH, ref ShipData[] all) : base(mapW, mapH)
        {
            allships = all;
        }
        public void PlaceShips()
        {
            while (PlacingShip < 5)
            {
                ShipData data = allships[PlacingShip];
                int ranX, ranY, ranA;

                do
                {
                    ranX = ran.Next(0, 9);
                    ranY = ran.Next(0, 9);
                    ranA = ran.Next(0, 3);
                } while (!PlaceShip(new Ship(data.Name, ranX, ranY, ranA, data.Size, data.SpritePath)));
                PlacingShip++;
            }
        }
    }
}
