using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private ShipPart[,] shipparts = new ShipPart[7, 7];
        private guessStates[,] guesses = new guessStates[7, 7];
        public Player(bool human)
        {
            IsHuman = human;
        }
        public bool IsHuman { get; set; }

        public bool PlaceShip(Ship ship)
        {
            if (PlaceClear(ship))
            {
                for(int i=0; i<ship.Size; i++)
                {
                    ShipPart temppart = ship.GetPart(i);
                    int partX = temppart.X;
                    int partY = temppart.Y;

                    shipparts[partX, partY] = temppart;
                }
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

                if (!HasPart(partX, partY))
                {
                    clear++;
                }
            }

            if (clear == aim_clear)
            {
                Console.WriteLine("Place is clear to place a ship");
                return true;
            }
            else
            {
                Console.WriteLine("Place is not clear to place");
                return false;
            }
        }
        public bool HasPart(int x, int y)
        {
            return shipparts[x, y] != null;
        }
    }
}
