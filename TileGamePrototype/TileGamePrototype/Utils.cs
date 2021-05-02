using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TileGamePrototype
{
    public class Utils
    {
        //Picturebox that does not use antialiasing
        public class nonAntialiasingPictureBox : PictureBox
        {
            protected override void OnPaint(PaintEventArgs pe)
            {
                pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
 	            base.OnPaint(pe);
            }
        }


        //Custom message box class
        public class MessageBox : nonAntialiasingPictureBox
        {
            //Labels
            Label lblMessage;
            Label lblYes;
            Label lblNo;

            public MessageBox(System.Drawing.Size size, System.Drawing.Image backgroundImage) : base()
            {
                this.Size = size;
                this.BackgroundImage = backgroundImage;
                this.BackgroundImageLayout = ImageLayout.Stretch;
                //Message
                lblMessage = new Label();
                lblMessage.Parent = this;
                lblMessage.Show();
                lblMessage.Dock = DockStyle.Fill;
                lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                lblMessage.Font = new System.Drawing.Font("Times New Roman", 22.0f);
                lblMessage.ForeColor = System.Drawing.Color.White;
                lblMessage.BackColor = System.Drawing.Color.Transparent;
                //Yes button
                lblYes = new Label();
                lblYes.Parent = this;
                lblYes.Show();
                lblYes.Text = "Yes";
                lblYes.AutoSize = false;
                lblYes.Location = new System.Drawing.Point(10, size.Height - 50);
                lblYes.Size = new System.Drawing.Size(90, 40);
                lblYes.Click += Yes;
                lblYes.Font = new System.Drawing.Font("Times New Roman", 22.0f, System.Drawing.FontStyle.Bold);
                lblYes.ForeColor = System.Drawing.Color.White;
                lblYes.BackColor = System.Drawing.Color.Transparent;
                //No button
                lblNo = new Label();
                lblNo.Parent = this;
                lblNo.Show();
                lblNo.Text = "No";
                lblNo.AutoSize = false;
                lblNo.Location = new System.Drawing.Point(size.Width - 100, size.Height - 50);
                lblNo.Size = new System.Drawing.Size(90, 40);
                lblNo.Click += No;
                lblNo.Font = new System.Drawing.Font("Times New Roman", 22.0f, System.Drawing.FontStyle.Bold);
                lblNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                lblNo.ForeColor = System.Drawing.Color.White;
                lblNo.BackColor = System.Drawing.Color.Transparent;
            }

            //Callback delegate
            public delegate void DialogResultCallBack(MessageBox sender, bool returnValue);

            //Callback
            private DialogResultCallBack resultCallBack;

            //Shows the message box with the specified message
            public void ShowMessageBox(string message, DialogResultCallBack callback)
            {
                this.Show();
                this.BringToFront();
                resultCallBack = callback;
                lblMessage.Text = message;
                lblYes.Show();
                lblNo.Show();
                lblYes.BringToFront();
                lblNo.BringToFront();
                this.Refresh();
                Program.formRef.disableKeyPress = true; //disable moving while message box is shown
                Program.formRef.KeyDown += formRef_KeyDown;
            }

            // Add key down event so player can exit message box using keyboard shortcuts
            void formRef_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Yes(sender, EventArgs.Empty);
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    No(sender, EventArgs.Empty);
                }
            }

            //Restores to normal functionality
            void closeDown()
            {
                lblMessage.Text = "Loading...";
                lblNo.Hide();
                lblYes.Hide();
                this.Refresh();
                Program.formRef.disableKeyPress = false;
                Program.formRef.KeyDown -= formRef_KeyDown;
            }

            //Return positive result
            public void Yes(object sender, EventArgs e)
            {
                closeDown();
                resultCallBack(this, true);
            }

            //Return negative result
            public void No(object sender, EventArgs e)
            {
                closeDown();
                resultCallBack(this, false);
            }
        }

        //Small function for searching array for substring
        public static string findStringInArray(string[] array, string substring)
        {
            foreach (string element in array)
            {
                if (element.Contains(substring))
                {
                    return element;
                }
            }
            return "";
        }
    }
}
