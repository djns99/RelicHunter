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
    public partial class InventoryWindow : Form
    {
        public InventoryWindow()
        {
            InitializeComponent();
        }

        //Displays inventory
        private void InventoryWindow_Load(object sender, EventArgs e)
        {
            //Maximum number of items in inventory is 100
            /*if (levelHandler.player.inventory.Count == 0)
            {
                this.lblEmpty.Show();
                return;
            }
            int number = 0;
            int numberX = 10;
            int size = 34;
            foreach (Item item in levelHandler.player.inventory)
            {
                int x = 16 + (size + 16) * (number % numberX);
                int y = 16 + (size + 16) * (number / numberX);
                Utils.nonAntialiasingPictureBox naPicRef = new Utils.nonAntialiasingPictureBox();
                this.Controls.Add(naPicRef);
                naPicRef.Location = new Point(x, y);
                naPicRef.Size = new Size(size, size);
                naPicRef.Image = item.sprite;
                naPicRef.SizeMode = PictureBoxSizeMode.Zoom;
                naPicRef.BackColor = Color.Transparent;
                naPicRef.Show();
                number++;
            }*/
            int size = 120;
            int numberX = 4;
            int index = 0;
            //Loop through each item and get how many of that item the player has
            foreach (ItemTypes item in Enum.GetValues(typeof(ItemTypes)))
            {
                if (item == ItemTypes.Relic && levelHandler.relic == null)
                {
                    continue;
                }
                int number = 0;
                //Calculate how many of that item the player has
                while (levelHandler.player.HasItem(item, number + 1))
                {
                    number++;
                }
                //Get coordinates at which to set it
                int x = 10 + (size + 4) * (index % numberX);
                int y = 10 + (size + 4) * (index / numberX);
                Utils.nonAntialiasingPictureBox naPicRef = new Utils.nonAntialiasingPictureBox();
                this.Controls.Add(naPicRef);
                naPicRef.Location = new Point(x, y);
                naPicRef.Size = new Size(size, size);
                naPicRef.Image = ItemOperators.extractControlFromType(item).sprite;
                naPicRef.SizeMode = PictureBoxSizeMode.Zoom;
                naPicRef.BackColor = Color.Transparent;
                naPicRef.Show();

                System.Windows.Forms.Label lblCount = new System.Windows.Forms.Label();
                this.Controls.Add(lblCount);
                lblCount.Parent = naPicRef;
                lblCount.Location = new Point(naPicRef.Width - (size / 2), naPicRef.Height - (size / 6));
                lblCount.Size = new Size(size / 2, size / 6);
                lblCount.Text = number.ToString();
                lblCount.Font = new Font("Times New Roman", 12, lblCount.Font.Style);
                lblCount.TextAlign = ContentAlignment.MiddleRight;
                lblCount.ForeColor = Color.White;
                lblCount.BackColor = Color.Transparent;
                lblCount.Show();

                index++;
            }
        }

        //Return to menu
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Exit if escapekey is pressed
        private void InventoryWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
