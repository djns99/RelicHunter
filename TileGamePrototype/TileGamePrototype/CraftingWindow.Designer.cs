namespace TileGamePrototype
{
    partial class CraftingWindow
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
            this.lblSelectToolTitle = new System.Windows.Forms.Label();
            this.lblRequiredItemsTitle = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblClickHere = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblSelectToolTitle
            // 
            this.lblSelectToolTitle.AutoSize = true;
            this.lblSelectToolTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSelectToolTitle.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectToolTitle.ForeColor = System.Drawing.Color.White;
            this.lblSelectToolTitle.Location = new System.Drawing.Point(50, 20);
            this.lblSelectToolTitle.Name = "lblSelectToolTitle";
            this.lblSelectToolTitle.Size = new System.Drawing.Size(422, 31);
            this.lblSelectToolTitle.TabIndex = 0;
            this.lblSelectToolTitle.Text = "Select The Tool You Wish To Craft";
            // 
            // lblRequiredItemsTitle
            // 
            this.lblRequiredItemsTitle.AutoSize = true;
            this.lblRequiredItemsTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblRequiredItemsTitle.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRequiredItemsTitle.ForeColor = System.Drawing.Color.White;
            this.lblRequiredItemsTitle.Location = new System.Drawing.Point(50, 308);
            this.lblRequiredItemsTitle.Name = "lblRequiredItemsTitle";
            this.lblRequiredItemsTitle.Size = new System.Drawing.Size(361, 31);
            this.lblRequiredItemsTitle.TabIndex = 1;
            this.lblRequiredItemsTitle.Text = "Items Required To Craft Tool";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExit.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(391, 471);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(110, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.TabStop = false;
            this.btnExit.Text = "Return to game";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblClickHere
            // 
            this.lblClickHere.BackColor = System.Drawing.Color.Transparent;
            this.lblClickHere.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClickHere.ForeColor = System.Drawing.Color.White;
            this.lblClickHere.Location = new System.Drawing.Point(365, 190);
            this.lblClickHere.Name = "lblClickHere";
            this.lblClickHere.Size = new System.Drawing.Size(101, 23);
            this.lblClickHere.TabIndex = 4;
            this.lblClickHere.Text = "Click Here!";
            this.lblClickHere.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblClickHere.Visible = false;
            // 
            // CraftingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::TileGamePrototype.Properties.Resources.WorkBenchTexture;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(512, 513);
            this.Controls.Add(this.lblClickHere);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblRequiredItemsTitle);
            this.Controls.Add(this.lblSelectToolTitle);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(528, 551);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(528, 551);
            this.Name = "CraftingWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Relic Hunter - Crafting Window";
            this.Load += new System.EventHandler(this.CraftingWindow_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CraftingWindow_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSelectToolTitle;
        private System.Windows.Forms.Label lblRequiredItemsTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblClickHere;


    }
}