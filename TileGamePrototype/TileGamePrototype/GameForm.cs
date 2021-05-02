using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TileGamePrototype
{
    public partial class GameForm : Form
    {
        //Menu bar sizes
        public static int headerHeight = 30;
        public static int footerHeight = 70;

        //Other Windows Except help window
        private static CraftingWindow craftingWindow;
        private static InventoryWindow inventoryWindow;

        //Add level tiles to form
        public void AddAllControls()
        {
            foreach (Tile tile in levelHandler.currentLevelSurfaceArray)
            {
                this.Controls.Add(tile);
                tile.Click += new System.EventHandler(tile.ClickedOn);
                tile.SendToBack();
            }
            this.Controls.Add(levelHandler.player);
            levelHandler.player.Click += new System.EventHandler(levelHandler.player.ClickedOn);
            levelHandler.player.BringToFront();
            levelHandler.player.Parent = levelHandler.player.hiddenControl;

            //Reset counters
            this.lblLevelNo.Text = "Level: " + (Program.level + 1).ToString();
            this.lblMoveCounter.Text = "Move: 0";
        }

        //Remove tiles
        public void RemoveAllControls()
        {
            this.Controls.Remove(levelHandler.player);
            foreach (Control tile in levelHandler.currentLevelSurfaceArray)
            {
                this.Controls.Remove(tile);
            }
        }

        //Add single tile
        public void AddControl(Tile tile)
        {
            this.Controls.Add(tile);
            tile.Click += new System.EventHandler(tile.ClickedOn);
            tile.SendToBack();
        }

        //Remove single tile
        public void RemoveControl(Tile tile)
        {
            this.Controls.Remove(tile);
        }

        //Constructor
        public GameForm()
        {
            InitializeComponent();
            craftingWindow = new CraftingWindow();
            inventoryWindow = new InventoryWindow();
            AddAllControls();
        }

        //Adds footer button to form
        private void addFooterButton(string name, int distanceAlong, Image image, System.EventHandler eventHandler)
        {
            //Add footer button
            Utils.nonAntialiasingPictureBox naPicRef = new Utils.nonAntialiasingPictureBox();
            naPicRef.Name = name;

            this.Controls.Add(naPicRef);
            naPicRef.Parent = this.lblFooter;
            naPicRef.Location = new Point(distanceAlong, 5);
            naPicRef.Size = new Size(footerHeight - 10, footerHeight - 10);
            naPicRef.Image = image;
            naPicRef.SizeMode = PictureBoxSizeMode.Zoom;
            naPicRef.BackColor = Color.Transparent;
            naPicRef.Click += eventHandler;
            naPicRef.Show();
        }

        //Form load event
        private void GameForm_Load(object sender, EventArgs e)
        {
            this.ClientSize = new System.Drawing.Size(Program.formWidth, Program.formHeight + headerHeight + footerHeight);
            lblFooter.Size = new Size(Program.formWidth, footerHeight);
            lblHeader.Size = new Size(Program.formWidth, headerHeight);
            lblFooter.Location = new Point(0, Program.formHeight + headerHeight);

            //Add buttons to the footer, did this manually as they are my custom nonAntialiasing picture boxes
            addFooterButton("Inventory", 60, global::TileGamePrototype.Properties.Resources.InventoryButtonIcon, new System.EventHandler(Inventory));
            addFooterButton("Combine", (60 + (footerHeight - 10 + 80)), global::TileGamePrototype.Properties.Resources.CraftButtonIcon, new System.EventHandler(Combine));
            addFooterButton("Menu", (60 + (footerHeight - 10 + 80) * 2), global::TileGamePrototype.Properties.Resources.MenuButtonIcon, new System.EventHandler(ReturnToMenu));
            addFooterButton("Help", (60 + (footerHeight - 10 + 80) * 3), global::TileGamePrototype.Properties.Resources.HelpButtonIcon, new System.EventHandler(Help));
        }

        //Used for disabling keypresses
        public bool move = true;
        public bool disableKeyPress = false;

        //Key down event
        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            //If message box is shown this will be disabled
            if (disableKeyPress)
                return;
            //Gets players location
            Point loc = TileOperators.getGridPosition(levelHandler.player.hiddenControl);
            //Up
            if (e.KeyCode == Keys.Up)
            {
                if (loc.Y > 0 && move)
                    levelHandler.currentLevelSurfaceArray[loc.Y - 1, loc.X].ClickedOn(this, EventArgs.Empty);
                move = false;
            }
            //Down
            else if (e.KeyCode == Keys.Down)
            {
                if (loc.Y < levelHandler.currentLevelSurfaceArray.GetLength(0) - 1 && move)
                    levelHandler.currentLevelSurfaceArray[loc.Y + 1, loc.X].ClickedOn(this, EventArgs.Empty);
                move = false;
            }
            //Left
            else if (e.KeyCode == Keys.Left)
            {
                if(loc.X > 0 && move)
                    levelHandler.currentLevelSurfaceArray[loc.Y, loc.X - 1].ClickedOn(this, EventArgs.Empty);
                move = false;
            }
            //Right
            else if (e.KeyCode == Keys.Right)
            {
                if (loc.X < levelHandler.currentLevelSurfaceArray.GetLength(1) - 1 && move)
                    levelHandler.currentLevelSurfaceArray[loc.Y, loc.X + 1].ClickedOn(this, EventArgs.Empty);
                move = false;
            }
            //Inventory Shortcut
            else if(e.KeyCode == Keys.I)
            {
                Inventory(null, EventArgs.Empty);
            }
            //Crafting shortcut
            else if (e.KeyCode == Keys.C)
            {
                Combine(null, EventArgs.Empty);
            }
            //Menu shortcut
            else if (e.KeyCode == Keys.M || e.KeyCode == Keys.Escape)
            {
                ReturnToMenu(null, EventArgs.Empty);
            }
            //Help shortcut
            else if (e.KeyCode == Keys.H)
            {
                Help(null, EventArgs.Empty);
            }
            //Restart level
            else if (e.KeyCode == Keys.R)
            {
                levelHandler.restartLevel();
            }
            //Developer hack
            /*else
            {
                Program.NextLevel();
            }*/
        }

        //enable moving again when key is released
        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            move = true;
        }

        //Open inventory
        private void Inventory(object sender, EventArgs e)
        {
            this.Hide();
            inventoryWindow.Show();
            inventoryWindow.FormClosed += inventoryWindowClosed;
        }

        //Inventory closed
        void inventoryWindowClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
            inventoryWindow = new InventoryWindow();
        }

        //Open crafting window
        private void Combine(object sender, EventArgs e)
        {
            this.Hide();
            craftingWindow.Show();
            craftingWindow.FormClosed += craftingWindowClosed;
        }

        //Crafting closed
        void craftingWindowClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
            craftingWindow = new CraftingWindow();
        }

        //Return to menu
        public void ReturnToMenu(object sender, EventArgs e)
        {
            this.Close();
        }

        //Help window
        private void Help(object sender, EventArgs e)
        {
            this.Hide();
            levelHandler.levelInfoWindow.Show();
            levelHandler.levelInfoWindow.FormClosing += helpWindowClosed;
            levelHandler.levelInfoWindow.BringToFront();
        }

        //Help closed
        void helpWindowClosed(object sender, FormClosingEventArgs e)
        {
            this.Show();
            e.Cancel = true;
            levelHandler.levelInfoWindow.Hide();
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            Program.mapRef.Show();
        }
    }
}
