using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship
{
    public partial class Form1 : Form
    {
        public int Tsize = 16;
        public int Tscale = 4;
        public int TWidth = 7;
        public int THeight = 7;

        stages stage = stages.None;

        //Set base ship info
        ShipData[] ships = { new ShipData("Carrier", 5),
                             new ShipData("Battleship", 4),
                             new ShipData("Destroyer", 3),
                             new ShipData("Submarine", 3),
                             new ShipData("Patrol Boat", 2) };

        Player[] players = { new Player(true), new Player(false) };
        int placing_ship = 0;
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
            bool[] ships_placed = { false, false, false, false, false };

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

                    for(int i=0; i<ships.Length; i++)
                    {
                        LabelInfo.Text += $"{ships[i].Name}\n";
                    }
                    break;
                case stages.InGame:
                    LabelObjective.Text = "Destroy the Enemy";
                    LabelInfo.Text = "";
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

        private void GuessMap_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            var relativePoint = PointToClient(Cursor.Position);

            int MouseX = relativePoint.X - GuessMap.Location.X;
            int MouseY = relativePoint.Y - GuessMap.Location.Y;

            int CellSize = Tsize * Tscale;

            int GridX = MouseX / CellSize;
            int GridY = MouseY / CellSize;

            Console.WriteLine($"MX: {MouseX}\tMY: {MouseY}");
            Console.WriteLine($"GX: {GridX}\tGY: {GridY}");

            if (me.Button == MouseButtons.Right)
            {
                string writetext = "";

                for (int y = 0; y < THeight; y++)
                {
                    for (int x = 0; x < TWidth; x++)
                    {
                        if (players[0].HasPart(x, y))
                        {
                            writetext += "O";
                        }
                        else
                        {
                            writetext += " ";
                        }
                    }
                    writetext += "\n";
                }
                Console.WriteLine(writetext);
            }

            switch (stage)
            {
                case stages.Place:
                    ShipData tempShip = ships[placing_ship];

                    if (players[turn].PlaceShip(new Ship(tempShip.Name, GridX, GridY, 0, tempShip.Size)))
                    {
                        placing_ship++;
                    }

                    if (placing_ship == 5)
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
        private void GuessMap_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int CellSize = Tsize * Tscale;

            //Draw player ships on main grid while placing
            if (stage == stages.Place)
            {
                Pen p_ship = new Pen(Color.Gray);

                for (int y = 0; y < THeight; y++)
                {
                    for (int x = 0; x < TWidth; x++)
                    {
                        if (players[0].HasPart(x, y))
                        {
                            Rectangle rect = new Rectangle((x * CellSize) + 8, (y * CellSize + 8), CellSize - 16, CellSize - 16);

                            g.DrawRectangle(p_ship, rect);
                        }
                    }
                }
            }

            //Draw map Grid
            Pen p_grid = new Pen(Color.Black);

            g.DrawRectangle(p_grid, 0, 0, GuessMap.Width-1, GuessMap.Height-1);

            for (int y = 1; y < TWidth; y++)
            {
                g.DrawLine(p_grid, 0, y * CellSize, GuessMap.Height, y * CellSize);
            }
            for (int x = 1; x < THeight; x++)
            {
                g.DrawLine(p_grid, x * CellSize, 0, x * CellSize, GuessMap.Width);
            }
        }

        private void ShipMap_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p = new Pen(Color.Black);

            int CellSize = (Tsize * Tscale)/2;

            g.DrawRectangle(p, 0, 0, ShipMap.Width - 1, ShipMap.Height - 1);

            for (int y = 1; y < TWidth; y++)
            {
                g.DrawLine(p, 0, y * CellSize, ShipMap.Height, y * CellSize);
            }
            for (int x = 1; x < THeight; x++)
            {
                g.DrawLine(p, x * CellSize, 0, x * CellSize, ShipMap.Width);
            }
        }
    }
}
