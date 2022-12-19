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
    static class Globals
    {
        public static string working_directory = Environment.CurrentDirectory;
        public static string asset_directory = Directory.GetParent(working_directory).Parent.FullName+"\\Assets\\";
    }
    public partial class Form1 : Form
    {
        public int Tbase = 16;
        public int Tscale = 2;
        public int Tsize = 0;

        static public int TWidth = 10;
        static public int THeight = 10;

        public int placing_dir = 0;

        stages stage = stages.None;

        //Set base ship info
        static ShipData[] allships = { new ShipData("Carrier", 5, "spr_carrier.png"),
                             new ShipData("Battleship", 4, "spr_battleship.png"),
                             new ShipData("Destroyer", 3, "spr_destroyer.png"),
                             new ShipData("Submarine", 3, "spr_submarine.png"),
                             new ShipData("Patrol Boat", 2, "spr_patrol.png") };

        Player[] players = { new Human(TWidth, THeight), new Bot(TWidth, THeight, ref allships) };
        int turn = 0;

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

            Tsize = Tbase * Tscale;
            NextStage();

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

                    LabelInfo.Text = "";

                    for(int i=0; i<allships.Length; i++)
                    {
                        LabelInfo.Text += $"{allships[i].Name}\n";
                    }
                    break;
                case stages.InGame:
                    LabelObjective.Text = "Destroy the Enemy";
                    LabelInfo.Text = "";

                    Bot bot = (Bot)players[1];
                    bot.PlaceShips();
                    Refresh();
                    break;
                case stages.Finished:
                    break;
            }
        }
        private void EndTurn()
        {
            turn++;

            if (turn == players.Length)
            {
                turn = 0;
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

            int GridX = MouseX / Tsize;
            int GridY = MouseY / Tsize;

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

            Player tempplayer = players[0];

            for (int s = 0; s < 5; s++)
                {
                Ship tempship = tempplayer.ships[s];

                if (tempship != null)
                {
                    g.DrawImage(tempship.DrawShip(), tempship.X*Tsize, tempship.Y*Tsize);

                    for (int part = 0; part < tempship.Size; part++)
                    {
                        ShipPart temppart = tempship.GetPart(part);
                        int pX = temppart.X;
                        int pY = temppart.Y;

                        Rectangle rect = new Rectangle((pX * Tsize) + 4, (pY * Tsize + 4), Tsize - 8, Tsize - 8);

                        g.DrawRectangle(p_ship, rect);  
                    }
                }                        
            }

            //Draw map Grid
            Pen p_grid = new Pen(Color.Black);
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

            g.DrawRectangle(p_grid, 0, 0, MainMap.Width-1, MainMap.Height-1);

            for (int y = 1; y < TWidth; y++)
            {
                g.DrawLine(p_grid, 0, y * Tsize, MainMap.Height, y * Tsize);
            }
            for (int x = 1; x < THeight; x++)
            {
                g.DrawLine(p_grid, x * Tsize, 0, x * Tsize, MainMap.Width);
            }
        }

        private void EnemyMap_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            //Draw enemy ships on enemy grid
                Pen p_ship = new Pen(Color.Gray);

                Player tempplayer = players[1];

                for (int s = 0; s < 5; s++)
                {
                Ship tempship = tempplayer.ships[s];

                if (tempship != null)
                {
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                    //g.DrawImage(tempship.Sprite, tempship.X * Tsize, tempship.Y * Tsize);

                    for (int part = 0; part < tempship.Size; part++)
                    {
                        ShipPart temppart = tempship.GetPart(part);
                        int pX = temppart.X;
                        int pY = temppart.Y;

                        Rectangle rect = new Rectangle((pX * Tsize) + 4, (pY * Tsize + 4), Tsize - 8, Tsize - 8);

                        g.DrawRectangle(p_ship, rect);
                    }
                }
            }
            
            //Draw grid
            Pen p = new Pen(Color.Black);

            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            g.DrawRectangle(p, 0, 0, EnemyMap.Width - 1, EnemyMap.Height - 1);

            for (int y = 1; y < TWidth; y++)
            {
                g.DrawLine(p, 0, y * Tsize, EnemyMap.Height, y * Tsize);
            }
            for (int x = 1; x < THeight; x++)
            {
                g.DrawLine(p, x * Tsize, 0, x * Tsize, EnemyMap.Width);
            }
        }
    }
}
