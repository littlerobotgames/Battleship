using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship
{
    public partial class Form1 : Form
    {
        public int placing_dir = 0;

        stages stage = stages.None;

        //Set base ship info
        static ShipData[] allships = { new ShipData("Carrier", 5, "spr_carrier.png"),
                             new ShipData("Battleship", 4, "spr_battleship.png"),
                             new ShipData("Destroyer", 3, "spr_destroyer.png"),
                             new ShipData("Submarine", 3, "spr_submarine.png"),
                             new ShipData("Patrol Boat", 2, "spr_patrol.png") };

        Player[] players = { new Human(), new Bot(ref allships, 1) };
        int turn = 0;

        Bitmap[] icons = { null, 
                           new Bitmap(new Bitmap(Globals.asset_directory + "spr_miss.png"), 32, 32),
                           new Bitmap(new Bitmap(Globals.asset_directory + "spr_hit.png"), 32, 32)};

        public Form1()
        {
            InitializeComponent();
        }
        enum stages
        {
            None = 0,
            Place,
            InGame,
            Finished
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //Print Globals
            Console.WriteLine("Base Directory = "+Globals.working_directory);
            Console.WriteLine("Asset Directory = " + Globals.asset_directory);
            NextStage();

            outcome_text.Text = "";

            /*Stages:
             * 0 = Base State
             * 1 = Placing Ships
             * 2 = InGame
             * 3 = Finished
             */
            
        }
        private void NextStage()
        {
            stage++;

            Console.WriteLine("New Stage is: " + stage);

            switch (stage)
            {
                case stages.Place:
                    LabelObjective.Text = "Place Your Ships";

                    P1TurnLabel.Visible = false;
                    P2TurnLabel.Visible = false;
                    break;
                case stages.InGame:
                    LabelObjective.Text = "Destroy the Enemy";

                    Bot bot = (Bot)players[1];
                    bot.PlaceShips();

                    P1TurnLabel.Visible = true;
                    P2TurnLabel.Visible = false;

                    Refresh();
                    break;
                case stages.Finished:
                    LabelObjective.Text = "";

                    if (players[0].Lives > 0 && players[1].Lives == 0)
                    {
                        outcome_text.Text = "Victory!";
                    }
                    else
                    {
                        outcome_text.Text = "Defeat";
                    }

                    break;
            }
        }       
        public void EndTurn()
        {
            turn++;

            if (turn == players.Length)
            {
                turn = 0;
            }

            foreach(Player p in players)
            {
                p.calcLives();
            }
            Console.WriteLine($"P1: {players[0].Lives}      P2: {players[1].Lives}");

            if (players[0].Lives == 0 || players[1].Lives == 0)
            {
                NextStage();
            }
            else
            {
                Console.WriteLine($"~~~~~~~~ Player {turn}'s Turn ~~~~~~~~");

                if (turn == 0)
                {
                    P1TurnLabel.Visible = true;
                    P2TurnLabel.Visible = false;
                }
                else
                {
                    P1TurnLabel.Visible = false;
                    P2TurnLabel.Visible = true;

                    Bot botplayer = (Bot)players[1];
                    botplayer.TakeTurn(players[0]);

                    MainMap.Refresh();

                    EndTurn();
                }
            }           
        }
        static public Point GetDirPoints(int dir)
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
        private void MainMap_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            var relativePoint = PointToClient(Cursor.Position);

            int MouseX = relativePoint.X - MainMap.Location.X;
            int MouseY = relativePoint.Y - MainMap.Location.Y;

            int GridX = MouseX / Globals.TileSize;
            int GridY = MouseY / Globals.TileSize;

            Console.WriteLine($"MX: {MouseX}\tMY: {MouseY}");
            Console.WriteLine($"GX: {GridX}\tGY: {GridY}");

            if (me.Button == MouseButtons.Left)
            {
                switch (stage)
                {
                    case stages.Place:
                        Player tempplayer = players[turn];
                        ShipData tempShip = allships[tempplayer.PlacingShip];

                        if (tempplayer.PlaceShip(new Ship(tempShip.Name, GridX, GridY, placing_dir, tempShip.Size, tempShip.SpritePath)))
                        {
                            Refresh();
                            tempplayer.PlacingShip++;
                            placing_dir = 0;
                        }

                        if (tempplayer.PlacingShip == 5)
                        {
                            NextStage();
                        }
                        break;
                    case stages.InGame:

                        break;
                    case stages.Finished:
                        break;
                }
            }
            else if (me.Button == MouseButtons.Right)
            {
                if (placing_dir == 3)
                {
                    placing_dir = 0;
                }
                else
                {
                    placing_dir++;
                }
                Console.WriteLine($"Angle is now {placing_dir}");
            }
        }
        private void MainMap_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            //Draw player ships on main grid while placing
            Pen p_ship = new Pen(Color.Gray);
            Pen p_center = new Pen(Color.Red);

            Player tempplayer = players[0];

            for (int s = 0; s < 5; s++)
                {
                Ship tempship = tempplayer.ships[s];

                if (tempship != null)
                {
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                    //g.DrawImage(tempship.Sprite, tempship.TopX, tempship.TopY);

                    g.DrawEllipse(p_center, tempship.TopX-3, tempship.TopY-3, 6, 6);

                    for (int part = 0; part < tempship.Size; part++)
                    {
                        ShipPart temppart = tempship.GetPart(part);
                        int pX = temppart.X;
                        int pY = temppart.Y;
                        bool hit = temppart.Hit;

                        Rectangle rect = new Rectangle((pX * Globals.TileSize) + 4, (pY * Globals.TileSize + 4), Globals.TileSize - 8, Globals.TileSize - 8);
                        g.DrawImage(temppart.Sprite, pX * Globals.TileSize, temppart.Y * Globals.TileSize);

                        g.DrawRectangle(p_ship, rect);
                    }
                }                        
            }

            //Draw map Grid
            Pen p_grid = new Pen(Color.Black);
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

            g.DrawRectangle(p_grid, 0, 0, MainMap.Width-1, MainMap.Height-1);

            for (int y = 1; y < Globals.MapTileWidth; y++)
            {
                g.DrawLine(p_grid, 0, y * Globals.TileSize, MainMap.Height, y * Globals.TileSize);
            }
            for (int x = 1; x < Globals.MapTileHeight; x++)
            {
                g.DrawLine(p_grid, x * Globals.TileSize, 0, x * Globals.TileSize, MainMap.Width);
            }

            //Draw hits and misses
            Player mainPlayer = players[1];
            for (int yy = 0; yy < Globals.MapTileHeight; yy++)
            {
                for (int xx = 0; xx < Globals.MapTileWidth; xx++)
                {
                    guessStates tilevalue = mainPlayer.GetValue(xx, yy);
                    Font font1 = new Font("Times New Roman", 6);

                    if (tilevalue != guessStates.None)
                    {
                        g.DrawImage(icons[(int)tilevalue], xx * Globals.TileSize, yy * Globals.TileSize);
                        if (Globals.DevMode)
                        {                            
                            g.DrawString(tilevalue.ToString(), font1, Brushes.Black, new PointF(xx * Globals.TileSize, yy * Globals.TileSize));                            
                        }
                    }                    
                }
            }
        }

        private void EnemyMap_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

                //Draw enemy ships on enemy grid
                Pen p_ship = new Pen(Color.Gray);
                Pen p_hit = new Pen(Color.Red);

                Player tempplayer = players[1];

                for (int s = 0; s < 5; s++)
                {
                    Ship tempship = tempplayer.ships[s];

                    if (tempship != null)
                    {
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                        //g.DrawImage(tempship.Sprite, tempship.X * Globals.TileSize, tempship.Y * Globals.TileSize);

                        for (int part = 0; part < tempship.Size; part++)
                        {
                            ShipPart temppart = tempship.GetPart(part);
                            int pX = temppart.X;
                            int pY = temppart.Y;
                            bool hit = temppart.Hit;

                            Rectangle rect = new Rectangle((pX * Globals.TileSize) + 4, (pY * Globals.TileSize + 4), Globals.TileSize - 8, Globals.TileSize - 8);

                            if (!hit && Globals.DevMode)
                            {
                                g.DrawRectangle(p_ship, rect);
                            }                           
                        }
                    }
                }
                       
            
            //Draw grid
            Pen p = new Pen(Color.Black);

            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            g.DrawRectangle(p, 0, 0, EnemyMap.Width - 1, EnemyMap.Height - 1);

            for (int y = 1; y < Globals.MapTileWidth; y++)
            {
                g.DrawLine(p, 0, y * Globals.TileSize, EnemyMap.Height, y * Globals.TileSize);
            }
            for (int x = 1; x < Globals.MapTileHeight; x++)
            {
                g.DrawLine(p, x * Globals.TileSize, 0, x * Globals.TileSize, EnemyMap.Width);
            }

            //Draw hits and misses
            Player mainPlayer = players[0];
            for (int yy = 0; yy < Globals.MapTileHeight; yy++)
            {
                for (int xx = 0; xx < Globals.MapTileWidth; xx++)
                {
                    guessStates tilevalue = mainPlayer.GetValue(xx, yy);
                    Font font1 = new Font("Times New Roman", 6);

                    if (tilevalue != guessStates.None)
                    {
                        g.DrawImage(icons[(int)tilevalue], xx * Globals.TileSize, yy * Globals.TileSize);
                        if (Globals.DevMode)
                        {
                            g.DrawString(tilevalue.ToString(), font1, Brushes.Black, new PointF(xx * Globals.TileSize, yy * Globals.TileSize));                           
                        }                       
                    }                    
                }
            }
        }

        private void EnemyMap_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            var relativePoint = PointToClient(Cursor.Position);

            int RawMouseX = relativePoint.X;
            int RawMouseY = relativePoint.Y;

            int MouseX = RawMouseX - EnemyMap.Location.X;
            int MouseY = RawMouseY - EnemyMap.Location.Y;

            int GridX = MouseX / Globals.TileSize;
            int GridY = MouseY / Globals.TileSize;

            Console.WriteLine($"RX: {RawMouseX}\tRY: {RawMouseY}");
            Console.WriteLine($"MX: {MouseX}\tMY: {MouseY}");
            Console.WriteLine($"GX: {GridX}\tGY: {GridY}");

            if (stage == stages.InGame)
            {
                if (turn == 0)
                {
                    if (me.Button == MouseButtons.Left)
                    {
                        guessStates tile = players[0].GetValue(GridX, GridY);

                        if (tile == guessStates.None)
                        {
                            guessStates result = players[1].GetAttacked(GridX, GridY);

                            players[0].SetGuess(GridX, GridY, result);

                            EnemyMap.Refresh();

                            EndTurn();
                        }                      
                    }
                }
            }
        }
    }
    static class Globals
    {
        public static string working_directory = Environment.CurrentDirectory;
        public static string asset_directory = Directory.GetParent(working_directory).Parent.FullName + "\\Assets\\";
        public static int TileSize = 32;
        public static int MapTileWidth = 10;
        public static int MapTileHeight = 10;
        public static bool DevMode = false;

        public static void wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            // Console.WriteLine("start wait timer");
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
                // Console.WriteLine("stop wait timer");
            };

            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }
    }
    public enum guessStates
    {
        None = 0,
        Miss,
        Hit
    }
}
