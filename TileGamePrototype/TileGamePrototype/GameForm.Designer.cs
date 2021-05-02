namespace TileGamePrototype
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblFooter = new System.Windows.Forms.Label();
            this.lblLevelNo = new System.Windows.Forms.Label();
            this.lblMoveCounter = new System.Windows.Forms.Label();
            this.lblRankTitle = new System.Windows.Forms.Label();
            this.naPicMedalIcon = new TileGamePrototype.Utils.nonAntialiasingPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.naPicMedalIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(510, 30);
            this.lblHeader.TabIndex = 0;
            // 
            // lblFooter
            // 
            this.lblFooter.BackColor = System.Drawing.Color.Transparent;
            this.lblFooter.Location = new System.Drawing.Point(0, 440);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(510, 70);
            this.lblFooter.TabIndex = 1;
            // 
            // lblLevelNo
            // 
            this.lblLevelNo.BackColor = System.Drawing.Color.Transparent;
            this.lblLevelNo.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLevelNo.ForeColor = System.Drawing.Color.White;
            this.lblLevelNo.Location = new System.Drawing.Point(0, 0);
            this.lblLevelNo.Name = "lblLevelNo";
            this.lblLevelNo.Size = new System.Drawing.Size(160, 30);
            this.lblLevelNo.TabIndex = 2;
            this.lblLevelNo.Text = "Level : 1";
            // 
            // lblMoveCounter
            // 
            this.lblMoveCounter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMoveCounter.BackColor = System.Drawing.Color.Transparent;
            this.lblMoveCounter.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMoveCounter.ForeColor = System.Drawing.Color.White;
            this.lblMoveCounter.Location = new System.Drawing.Point(326, 0);
            this.lblMoveCounter.Name = "lblMoveCounter";
            this.lblMoveCounter.Size = new System.Drawing.Size(185, 30);
            this.lblMoveCounter.TabIndex = 3;
            this.lblMoveCounter.Text = "Moves: 0";
            this.lblMoveCounter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblRankTitle
            // 
            this.lblRankTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblRankTitle.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRankTitle.ForeColor = System.Drawing.Color.White;
            this.lblRankTitle.Location = new System.Drawing.Point(160, 0);
            this.lblRankTitle.Name = "lblRankTitle";
            this.lblRankTitle.Size = new System.Drawing.Size(100, 30);
            this.lblRankTitle.TabIndex = 4;
            this.lblRankTitle.Text = "Rank:";
            // 
            // naPicMedalIcon
            // 
            this.naPicMedalIcon.BackColor = System.Drawing.Color.Transparent;
            this.naPicMedalIcon.Location = new System.Drawing.Point(260, 0);
            this.naPicMedalIcon.Name = "naPicMedalIcon";
            this.naPicMedalIcon.Size = new System.Drawing.Size(30, 30);
            this.naPicMedalIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.naPicMedalIcon.TabIndex = 12;
            this.naPicMedalIcon.TabStop = false;
            // 
            // GameForm
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::TileGamePrototype.Properties.Resources.WoodTexture;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(512, 512);
            this.Controls.Add(this.naPicMedalIcon);
            this.Controls.Add(this.lblRankTitle);
            this.Controls.Add(this.lblMoveCounter);
            this.Controls.Add(this.lblLevelNo);
            this.Controls.Add(this.lblFooter);
            this.Controls.Add(this.lblHeader);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Relic Hunter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameForm_FormClosing);
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.naPicMedalIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label lblHeader;
        public System.Windows.Forms.Label lblFooter;
        public System.Windows.Forms.Label lblLevelNo;
        public System.Windows.Forms.Label lblMoveCounter;
        public System.Windows.Forms.Label lblRankTitle;
        public Utils.nonAntialiasingPictureBox naPicMedalIcon;
    }
}

