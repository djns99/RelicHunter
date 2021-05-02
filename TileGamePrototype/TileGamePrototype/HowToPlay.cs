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
    public partial class HowToPlay : Form
    {
        public HowToPlay()
        {
            InitializeComponent();
        }

        private void HowToPlay_Load(object sender, EventArgs e)
        {
            int counter = 0;
            int tileSize = 32;
            int labelLength = 132;
            Dictionary<TileTypes, string> descriptions = new Dictionary<TileTypes,string>();
            descriptions.Add(TileTypes.Grass, "Grass, You can walk on it");
            descriptions.Add(TileTypes.Tree, "Trees, Cut these down with an axe");
            descriptions.Add(TileTypes.Sticks, "Sticks, Collect these to craft tools");
            descriptions.Add(TileTypes.Rocks, "Rocks, Collect these to craft tools");
            descriptions.Add(TileTypes.Bush, "Bush, Collect berries for food from bushes");
            descriptions.Add(TileTypes.Water, "Water, Build a bridge to cross it");
            descriptions.Add(TileTypes.Bridge, "Bridges, Create these with a log");
            descriptions.Add(TileTypes.Passive, "Cows, Kill these with a sword for food");
            descriptions.Add(TileTypes.Hostile, "Tigers, Don't come to close, they will eat you");
            descriptions.Add(TileTypes.Treasure, "Treasure, Dig up the relic with a spade");
            descriptions.Add(TileTypes.Chest, "Chest, Contains a relic");
            descriptions.Add(TileTypes.Home, "Home Village, Your humble abode");
            foreach(TileTypes tileType in Enum.GetValues(typeof(TileTypes)))
            {
                if (tileType == TileTypes.Cave || tileType == TileTypes.Stone || tileType == TileTypes.Ravine || tileType == TileTypes.Player)//Ignore removed types and player tiles
                    continue;
                Tile tileRef = TileOperators.extractControlFromType(tileType, Zone.Surface);
                Image image = tileRef.Image;
                Utils.nonAntialiasingPictureBox tileIcon = new Utils.nonAntialiasingPictureBox();
                tileIcon.Image = image;
                tileIcon.BackColor = Color.Transparent;
                tileIcon.SizeMode = PictureBoxSizeMode.Zoom;
                tileIcon.Size = new Size(tileSize, tileSize);
                tileIcon.Location = new Point((tileSize + labelLength) * (counter / 5) + 10, (tileSize + 5) * (counter % 5) + 300);
                this.Controls.Add(tileIcon);
                tileIcon.BringToFront();
                tileIcon.Show();

                Label lblDescription = new Label();

                lblDescription.Text = descriptions[tileType];
                lblDescription.BackColor = Color.Transparent;
                lblDescription.ForeColor = Color.White;
                lblDescription.Size = new Size(labelLength, tileSize);
                lblDescription.TextAlign = ContentAlignment.MiddleCenter;
                lblDescription.Font = new Font("Times new Roman", 9, FontStyle.Italic);
                lblDescription.Location = new Point((tileSize + labelLength) * (counter / 5) + 10 + tileSize, (tileSize + 5) * (counter % 5) + 300);
                this.Controls.Add(lblDescription);
                lblDescription.BringToFront();
                lblDescription.Show();


                counter++;
            }
        }

        private void HowToPlay_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.menuRef.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
