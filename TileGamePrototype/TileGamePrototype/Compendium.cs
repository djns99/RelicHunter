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
    public partial class Compendium : Form
    {
        public Compendium()
        {
            InitializeComponent();
        }

        private void Compendium_Load(object sender, EventArgs e)
        {
            lblTerracota.Text = LevelContainerClass.extractLevelInfo(4).relic.info;
            lblEgg.Text = LevelContainerClass.extractLevelInfo(9).relic.info;
            lblCross.Text = LevelContainerClass.extractLevelInfo(14).relic.info;
        }

        private void Compendium_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.menuRef.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Compendium_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
