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
    public partial class LevelMap : Form
    {
        //Map outline sourced from online
        //http://imagebase.net/Concept/World-Map-Dark
        public LevelMap()
        {
            InitializeComponent();
        }

        private void LevelMap_Load(object sender, EventArgs e)
        {
            //Send the title to behind the level icons - some overlap
            lblTitle.SendToBack();
        }

        //Function for ibtaining level number from last two characters of its name string
        private int extractLevelNumberFromName(string name)
        { 
            string shortenedName = name.Remove(0, name.Length - 2);
            if (shortenedName.Remove(1) == "1")
            {
                return Convert.ToInt32(shortenedName);
            }
            else 
            {
                return Convert.ToInt32(shortenedName.Remove(0, 1));
            }
        }

        //Level selected
        private void naPicLevel_Click(object sender, EventArgs e)
        {
            //Get reference to picture box
            Utils.nonAntialiasingPictureBox level = (Utils.nonAntialiasingPictureBox)sender;
            //Extract level number from last two letters of picture ax's name
            int levelNumber = extractLevelNumberFromName(level.Name);
            //Checks it is a valid level
            if (levelNumber > Program.numberOfLevels)
            {
                //Tell the user the level is not availible
                lblTitle.Text = "Coming Soon!";
                lblTitle.Refresh();
                System.Threading.Thread.Sleep(1000);
                lblTitle.Text = "Select the level you wish to play";
                lblTitle.Refresh();
                return;
            }
            //Change title to say loading level
            lblTitle.Text = "Loading Level...";
            lblTitle.Refresh();
            //Opens level and loads game window
            if (!Program.SelectLevel(levelNumber - 1))
            {
                //Tell the user they have not completed the previous levels
                lblTitle.Text = "Complete the previous levels first";
                lblTitle.Refresh();
                System.Threading.Thread.Sleep(1000);
                lblTitle.Text = "Select the level you wish to play";
                lblTitle.Refresh();
                return;
            }
            Program.formRef.Show();
            this.Hide();
        }

        //Reopens the level map
        private void LevelMap_Shown(object sender, EventArgs e)
        {
            if (this.Visible == false)
            {
                return;
            }
            lblTitle.Text = "Select the level you wish to play";
            Program.formRef.Hide();
        }

        //Handles key down
        private void LevelMap_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void naPicFinal_Click(object sender, EventArgs e)
        {
            if (Program.levelsComplete.ToList().Contains(new LevelContainerClass.rank()))
            {
                //Tell the user they have not completed the previous levels
                lblTitle.Text = "Find all three relics first!";
                lblTitle.Refresh();
                System.Threading.Thread.Sleep(1000);
                lblTitle.Text = "Select the level you wish to play";
                lblTitle.Refresh();
                return;
            }
            int gold = 0;
            int silver = 0;
            int bronze = 0;
            foreach (LevelContainerClass.rank rank in Program.levelsComplete)
            {
                if (rank.medal == 1)
                    gold++;
                else if (rank.medal == 2)
                    silver++;
                else if (rank.medal == 3)
                    bronze++;
            }
            this.KeyDown -= LevelMap_KeyDown;
            this.MaximumSize = Program.formRef.Size;
            this.MinimumSize = Program.formRef.Size;
            this.ClientSize = Program.formRef.ClientSize;
            this.Controls.Clear();
            Utils.MessageBox messageBox = new Utils.MessageBox(this.ClientSize, Program.formRef.BackgroundImage);
            this.Controls.Add(messageBox);
            messageBox.ShowMessageBox("Congratulations!\nYou completed the game with :\n" + 
                                        gold.ToString() + "/" + Program.numberOfLevels.ToString() + " levels at gold\n" + 
                                        (gold + silver).ToString() + "/" + Program.numberOfLevels.ToString() + " levels at silver or better\n" + 
                                        (gold + silver + bronze).ToString() + "/" + Program.numberOfLevels.ToString() + " levels at bronze or better\n\nDid you enjoy playing it?", new Utils.MessageBox.DialogResultCallBack(gameComplete));
        }

        private void gameComplete(Utils.MessageBox sender, bool result)
        {
            if (!result)
                levelHandler.requestPlayersBrutallyPainfulDeath();//Kill the player if they didn't like the game :-(
            else //Tell the player the dev hacks if they said they liked the game :-)
            {
                for (int i = 10; i > 0; i--)
                {
                    sender.ShowMessageBox("I am glad you enjoyed the game and I hope you learnt something from playing it.\nBy the way, next time you play you can click on Madagascar to unlock all the levels!\nThanks for playing!\n\nWARNING\nThis message will self destruct in " + i.ToString(), null);
                    System.Threading.Thread.Sleep(1000);
                }
            }
	        Environment.Exit(0);
        }

        private void LevelMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            Program.menuRef.Show();
        }

        private void naPicUnlockAllLevels_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Program.levelsComplete.Length; i++)
            {
                if(!Program.levelsComplete[i].Equals(new LevelContainerClass.rank()))
                    continue;
                Program.levelsComplete[i].movesTaken = 1000;
                Program.levelsComplete[i].medal = 4;
                ((Utils.nonAntialiasingPictureBox)(Program.mapRef.Controls.Find("naPicLevel" + (i + 1).ToString(), true).FirstOrDefault())).Image = null;
            }
            ((Utils.nonAntialiasingPictureBox)(Program.mapRef.Controls.Find("naPicFinal", true).FirstOrDefault())).Image = null;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
