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
    public partial class MenuScreen : Form
    {
        public MenuScreen()
        {
            InitializeComponent();
        }

        private void MenuScreen_Load(object sender, EventArgs e)
        {
            Utils.nonAntialiasingPictureBox background = new Utils.nonAntialiasingPictureBox();
            background.BackgroundImage = this.BackgroundImage;
            background.Location = new Point(0, 0);
            background.Size = this.ClientSize;
            this.Controls.Add(background);
            background.BringToFront();

            Label lblGender = new Label();
            lblGender.Text = "Are You";
            lblGender.BackColor = Color.Transparent;
            lblGender.ForeColor = Color.White;
            lblGender.Size = new Size(200, 60);
            lblGender.TextAlign = ContentAlignment.MiddleCenter;
            lblGender.Font = new Font("Times new Roman", 32, FontStyle.Bold);
            lblGender.Location = new Point((this.ClientSize.Width / 2) - (lblGender.Size.Width / 2), (this.ClientSize.Height / 2) - (lblGender.Size.Height / 2));
            this.Controls.Add(lblGender);
            lblGender.BringToFront();
            lblGender.Show();

            Button btnFemale = new Button();
            btnFemale.Text = "Female";
            btnFemale.Font = new Font("Times new Roman", 24, FontStyle.Bold);
            btnFemale.Size = new Size(130, 50);
            btnFemale.Location = new Point((this.ClientSize.Width / 2) + (btnFemale.Size.Width / 4), (this.ClientSize.Height / 2) - (lblGender.Size.Height / 2) + (lblGender.Height));
            this.Controls.Add(btnFemale);
            btnFemale.BringToFront();
            btnFemale.Show();
            btnFemale.TabStop = false;
            btnFemale.Click += btnFemale_Click;

            Button btnMale = new Button();
            btnMale.Text = "Male";
            btnMale.Font = new Font("Times new Roman", 24, FontStyle.Bold);
            btnMale.Size = btnFemale.Size;
            btnMale.Location = new Point((this.ClientSize.Width / 2) - (btnMale.Size.Width) - (btnMale.Size.Width / 4), (this.ClientSize.Height / 2) - (lblGender.Size.Height / 2) + (lblGender.Height));
            this.Controls.Add(btnMale);
            btnMale.BringToFront();
            btnMale.Show();
            btnMale.TabStop = false;
            btnMale.Click += btnMale_Click;

            background.Tag = "Gender";
            lblGender.Tag = "Gender";
            btnMale.Tag = "Gender";
            btnFemale.Tag = "Gender";

        }

        void RemoveGenderMessageBox()
        {
            for (int i = this.Controls.Count - 1; i >= 0; i--)
            {
                if ((string)this.Controls[i].Tag == "Gender")
                {
                    this.Controls.Remove(this.Controls[i]);
                }
            }
        }

        void btnFemale_Click(object sender, EventArgs e)
        {
            RemoveGenderMessageBox();
            Program.male = false;
        }

        void btnMale_Click(object sender, EventArgs e)
        {
            RemoveGenderMessageBox();
            Program.male = true;
        }

        private void btnPlayGame_Click(object sender, EventArgs e)
        {
            this.Hide();
            Program.mapRef.Show();
        }

        private void btnHowToPlay_Click(object sender, EventArgs e)
        {
            HowToPlay howToPlay = new HowToPlay();
            this.Hide();
            howToPlay.Show();
        }

        private void btnCompendium_Click(object sender, EventArgs e)
        {
            Compendium compendium = new Compendium();
            this.Hide();
            compendium.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MenuScreen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
