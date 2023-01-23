using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Battleship
{
    
    class Player
    {
        public Ship[] ships;
        public guessStates[,] guesses;
        public Player()
        {
            ships = new Ship[5];
            guesses = new guessStates[Globals.MapTileWidth, Globals.MapTileHeight];
            PlacingShip = 0;
        }
        public int PlacingShip { get; set; }
        public int Lives { get; set; }
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
        public bool HitShipAt(int x, int y)
        {
            //Returns index of the hit ship
            for (int s = 0; s < ships.Length; s++)
            {
                Ship tempship = ships[s];

                if (tempship != null)
                {
                    if (tempship.AtPlace(x, y))
                    {
                        Console.WriteLine($"Ship at place ({x}, {y})");

                        tempship.HitPart(x, y);
                        
                        return true;
                    }
                }
            }
            Console.WriteLine($"No ship at place ({x}, {y})");
            return false;
        }
        public guessStates GetAttacked(int x, int y)
        {
            if (HitShipAt(x, y))
            {
                return guessStates.Hit;
            }
            else
            {
                return guessStates.Miss;
            }
        }
        public void SetGuess(int x, int y, guessStates guess)
        {
            guesses[x, y] = guess;
        }
        public guessStates GetValue(int x, int y)
        {
            return guesses[x, y];
        }
        public void calcLives()
        {
            int l = 0;

            for (int s = 0; s < ships.Length; s++)
            {
                Ship ship = ships[s];

                l += ship.Hits;
            }

            Lives = l;
        }
    }
    class Human : Player
    {
        public Human()
        {

        }
    }
    class Bot : Player
    {
        private ShipData[] allships;
        private Random ran = new Random();
        public Bot(ref ShipData[] all, int l)
        {
            allships = all;
            Level = l;
        }
        public int Level { get; set; }
        public Point GetDirPoints(int dir)
        {
            Point p;
            switch (dir)
            {
                case 0:
                    p = new Point(1, 0);
                    break;
                case 1:
                    p = new Point(0, -1);
                    break;
                case 2:
                    p = new Point(-1, 0);
                    break;
                case 3:
                    p = new Point(0, 1);
                    break;
                default:
                    p = new Point(0, 0);
                    break;
            }
            return p;
        }
        public int CountEmpty(int x, int y)
        {
            Console.WriteLine($"Counting empty spacing around ({x}, {y})");
            int count = 0;

            for (int i = 0; i<4; i++)
            {
                int tempX = x + GetDirPoints(i).X;
                int tempY = y + GetDirPoints(i).Y;

                if (tempX>=0 && tempX<10 && tempY>=0 && tempY < 10)
                {
                    if (guesses[tempX, tempY] == guessStates.None)
                    {
                        Console.WriteLine($"    Spot ({tempX}, {tempY}) is empty");
                        count++;
                    }                              
                }
            }
            return count;
        }
        public void PlaceShips()
        {
            while (PlacingShip < 5)
            {
                ShipData data = allships[PlacingShip];
                int ranX, ranY, ranA;

                do
                {
                    ranX = ran.Next(0, 10);
                    ranY = ran.Next(0, 10);
                    ranA = ran.Next(0, 3);
                } while (!PlaceShip(new Ship(data.Name, ranX, ranY, ranA, data.Size, data.SpritePath)));
                PlacingShip++;
            }
        }
        public void TakeTurn(Player enemy)
        {
            Globals.wait(1000);

            int ranX = 0, ranY = 0;
            guessStates tile;

            //1. Find tile to guess

            switch (Level)
            {
                case 0:
                    //Easy Bot

                    //Find random unpicked tile
                    do
                    {
                        ranX = ran.Next(0, 10);
                        ranY = ran.Next(0, 10);
                        tile = guesses[ranX, ranY];
                    } while (tile != guessStates.None);
                    break;
                case 1:
                    //Medium Bot

                    //Get list of hits on the map
                    List<Point> hit_points = new List<Point>();
                    for (int yy = 0; yy < Globals.MapTileHeight; yy++)
                    {
                        for (int xx = 0; xx<Globals.MapTileWidth; xx++)
                        {
                            guessStates temptile = guesses[xx, yy];

                            if (temptile == guessStates.Hit)
                            {
                                if (CountEmpty(xx, yy) > 0)
                                {
                                    //Add to list if there are guessable tiles around it
                                    Console.WriteLine($"Found an available hit at ({xx}, {yy})");
                                    hit_points.Add(new Point(xx, yy));
                                }                               
                            }
                        }
                    }

                    if (hit_points.Count > 0)
                    {
                        Console.WriteLine($"{hit_points.Count} hit points to choose from");
                        //Choose a random hit with unpicked tiles around it
                        bool picked = false;
                        do
                        {
                            //Pick random hit point
                            int ranPointIndex = ran.Next(hit_points.Count);
                            Point tempPoint = hit_points[ranPointIndex];

                            //Count guessable spots around hit
                            int empty = CountEmpty(tempPoint.X, tempPoint.Y);

                            if (empty > 0)
                            {
                                guessStates checkTile = guessStates.None;
                                int checkX = 0;
                                int checkY = 0;

                                do
                                {
                                    //Pick random spots around hit until one is unguessed
                                    int ranDir = ran.Next(4);

                                    Point addP = GetDirPoints(ranDir);
                                    checkX = tempPoint.X + addP.X;
                                    checkY = tempPoint.Y + addP.Y;

                                    if (checkX>=0 && checkX<10 && checkY>=0 && checkY < 10)
                                    {
                                        checkTile = guesses[checkX, checkY];
                                    }
                                    else
                                    {
                                        checkTile = guessStates.Miss;
                                    }

                                } while (checkTile != guessStates.None);

                                picked = true;
                                ranX = checkX;
                                ranY = checkY;
                            }

                        } while (picked == false);
                    }
                    else
                    {
                        //Find random unpicked tile
                        do
                        {
                            ranX = ran.Next(0, 10);
                            ranY = ran.Next(0, 10);
                            tile = guesses[ranX, ranY];
                        } while (tile != guessStates.None);
                    }

                    
                    break;
                case 2:
                    //Hard Bot
                    break;
            }
            
            //2. Attack that tile
            guessStates result = enemy.GetAttacked(ranX, ranY);
            SetGuess(ranX, ranY, result);

            Console.WriteLine($"Bot attacked at ({ranX}, {ranY}) and it was a {result}");

            if (result == guessStates.Hit)
            {
                Console.WriteLine($"Player 1 has {enemy.Lives} lives left");
            }
        }
    }
}
