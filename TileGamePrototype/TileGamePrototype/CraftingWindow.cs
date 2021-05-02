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
    public partial class CraftingWindow : Form
    {
        public CraftingWindow()
        {
            InitializeComponent();
        }

        //Info
        private int itemSelectSize = 128;
        private int itemInSlots = 0;

        //Load up tools at top
        private void CraftingWindow_Load(object sender, EventArgs e)
        {
            Utils.nonAntialiasingPictureBox naPicSwordSelect = new Utils.nonAntialiasingPictureBox();
            setItemSelectInfo(naPicSwordSelect, new Point(40, 60), "SwordSelect", ItemOperators.extractControlFromType(ItemTypes.Sword));
            Utils.nonAntialiasingPictureBox naPicSpadeSelect = new Utils.nonAntialiasingPictureBox();
            setItemSelectInfo(naPicSpadeSelect, new Point(40 + (itemSelectSize + 32), 60), "SpadeSelect", ItemOperators.extractControlFromType(ItemTypes.Spade));
            Utils.nonAntialiasingPictureBox naPicAxeSelect = new Utils.nonAntialiasingPictureBox();
            setItemSelectInfo(naPicAxeSelect, new Point(40 + (itemSelectSize + 32) * 2, 60), "AxeSelect", ItemOperators.extractControlFromType(ItemTypes.Axe));
            this.Controls.Add(naPicSwordSelect);
            this.Controls.Add(naPicSpadeSelect);
            this.Controls.Add(naPicAxeSelect);
        }

        //Get the type of item being crafted
        private ItemTypes extractItemToCraft(string ToolName)
        {
            switch (ToolName)
            {
                case "SwordSelect":
                    return ItemTypes.Sword;
                case "SpadeSelect":
                    return ItemTypes.Spade;
                case "AxeSelect":
                    return ItemTypes.Axe;
                default:
                    return ItemTypes.Sword;
            }
        }

        //Return the required for crafting tool - but this goes directly from name could also use built in array
        private List<ItemTypes> extractRequiredItems(string ToolName)
        {
            switch (ToolName)
            { 
                case "SwordSelect":
                    return new List<ItemTypes> { ItemTypes.Stick, ItemTypes.Rock };
                case "SpadeSelect":
                    return new List<ItemTypes> { ItemTypes.Stick, ItemTypes.Rock };
                case "AxeSelect":
                    return new List<ItemTypes> { ItemTypes.Stick, ItemTypes.Rock };
                default:
                    return new List<ItemTypes>();
            }
        }
        
        //Get the type of required item for crafting
        private ItemTypes extractRequiredItemType(string ItemName)
        {
            if (ItemName.Equals(ItemTypes.Stick.ToString() + "ToCollect") || ItemName.Equals(ItemTypes.Stick.ToString() + "Collected"))
                return ItemTypes.Stick;
            else if (ItemName.Equals(ItemTypes.Rock.ToString() + "ToCollect") || ItemName.Equals(ItemTypes.Rock.ToString() + "Collected"))
                return ItemTypes.Rock;
            else
                return ItemTypes.Stick;
        }

        //Set details for items used in crafting
        private void setItemSelectInfo(Utils.nonAntialiasingPictureBox naPicRef, Point Location, string name, Item item)
        {
            naPicRef.Location = Location;
            naPicRef.Size = new Size(itemSelectSize, itemSelectSize);
            naPicRef.SizeMode = PictureBoxSizeMode.Zoom;
            naPicRef.BackColor = Color.Transparent;
            naPicRef.Name = name;
            naPicRef.Image = item.sprite;
            naPicRef.Click += new System.EventHandler(itemSelectClickedOn);
        }

        //tool being crafted
        Utils.nonAntialiasingPictureBox toolToCraft;

        //Checks 
        private void itemSelectClickedOn(object sender, EventArgs e)
        {
            //Console.WriteLine("Item Selected");
            //Get reference to item or tool being selected
            Utils.nonAntialiasingPictureBox itemSelected = (Utils.nonAntialiasingPictureBox)sender;
            //Add new 'items required' if tool is clicked on
            if (itemSelected.Name.Contains("Select"))
            {
                toolToCraft = itemSelected;
                List<ItemTypes> required = extractRequiredItems(itemSelected.Name);
                int multiplier = 0;

                itemInSlots = 0;
                //Remove previous 'items required'
                for (int i = this.Controls.Count - 1; i >= 0; i-- )
                {
                    Control control = this.Controls[i];
                    if (control.Name.Contains("Collect") || control.Name.Contains("Craft"))
                    {
                        this.Controls.Remove(control);
                    }
                }

                //Add new items required
                foreach (ItemTypes itemType in required)
                {
                    Item item = ItemOperators.extractControlFromType(itemType);
                    Utils.nonAntialiasingPictureBox itemReq = new Utils.nonAntialiasingPictureBox();
                    setItemSelectInfo(itemReq, new Point(40 + (itemSelectSize + 32) * multiplier /*algebra - YEAH!*/, lblRequiredItemsTitle.Bottom + 10), itemType.ToString() + "ToCollect", item);
                    multiplier++;
                    this.Controls.Add(itemReq);
                }
            }
            //Moves item to crafting 'equation' if player has it
            else if (itemSelected.Name.Contains("ToCollect"))
            {
                ItemTypes itemType = extractRequiredItemType(itemSelected.Name);
                int spot = (itemSelected.Location.X - 40) / (itemSelectSize + 32);
                if (levelHandler.player.HasItem(itemType))
                {
                    itemSelected.Hide();
                    itemSelected.Location = new Point(60 + (140 * spot), 220);
                    itemSelected.Size = new Size(75, 75);
                    itemSelected.Show();
                    itemInSlots++;
                    itemSelected.Name = itemType.ToString() + "Collected";
                    //Show output tool if 'equation' is complete
                    if (itemInSlots >= 2)
                    {
                        lblClickHere.Show();
                        ItemTypes selectedToolType = extractItemToCraft(toolToCraft.Name);
                        Item tool = ItemOperators.extractControlFromType(selectedToolType);
                        Utils.nonAntialiasingPictureBox itemCrafted = new Utils.nonAntialiasingPictureBox();
                        setItemSelectInfo(itemCrafted, new Point(375, 220), selectedToolType.ToString() + "ToCraft", tool);
                        itemCrafted.Size = new Size(75, 75);
                        itemCrafted.Click += collectItem;
                        this.Controls.Add(itemCrafted);
                    }
                }
            }
            //Move tool out of equation
            else if (itemSelected.Name.Contains("Collected"))
            {
                lblClickHere.Hide();
                ItemTypes itemType = extractRequiredItemType(itemSelected.Name);
                int spot = (itemSelected.Location.X - 60) / 140;
                itemSelected.Hide();
                itemSelected.Location = new Point(40 + (itemSelectSize + 32) * spot, lblRequiredItemsTitle.Bottom + 10);
                itemSelected.Size = new Size(itemSelectSize, itemSelectSize);
                itemSelected.Show();
                itemInSlots--;
                itemSelected.Name = itemType.ToString() + "ToCollect";

                //Remove equation output if there
                for (int i = this.Controls.Count - 1; i >= 0; i--)
                {
                    Control control = this.Controls[i];
                    if (control.Name.Contains("Craft"))
                    {
                        control.Click -= collectItem;
                        this.Controls.Remove(control);
                    }
                }
            }
        }

        //Colect item from output slot
        private void collectItem(object sender, EventArgs e)
        {
            ItemTypes itemType = extractItemToCraft(toolToCraft.Name);
            List<ItemTypes> reqItems = extractRequiredItems(toolToCraft.Name);

            //Simple check to see that player has all items in case an unknown case slips through
            foreach (ItemTypes item in reqItems)
            { 
                if(!levelHandler.player.HasItem(item))
                {
                    Console.WriteLine("Does Not Have items");
                    return;
                }
            }

            foreach (ItemTypes item in reqItems)
            {
                levelHandler.player.RemoveItem(item);
            }

            itemInSlots = 0;
            lblClickHere.Hide();

            levelHandler.player.CollectItem(itemType);
            //Remove all items in the 'equation' and under required items
            for (int i = this.Controls.Count - 1; i >= 0; i--)
            {
                Control control = this.Controls[i];
                if (control.Name.Contains("Collect") || control.Name.Contains("Craft"))
                {
                    this.Controls.Remove(control);
                }
            }
        }

        //Close, unless player hasn't finished crafting
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (itemInSlots >= 2)
            {
                Utils.MessageBox messagebox = new Utils.MessageBox(new Size(this.ClientSize.Width, this.ClientSize.Height), global::TileGamePrototype.Properties.Resources.WoodTexture);
                this.Controls.Add(messagebox);
                messagebox.ShowMessageBox("Do you wish to continue crafting your " + extractItemToCraft(toolToCraft.Name).ToString() + "?", new Utils.MessageBox.DialogResultCallBack(DialogExitCallBack));
                return;
            }
            this.Close();
        }

        //Exit callback
        void DialogExitCallBack(Utils.MessageBox sender, bool result)
        {
            if (!result)
            {
                this.Close();
                lblClickHere.Hide();
            }
            else
            {
                sender.Hide();
                this.Controls.Remove(sender);
                sender.Controls.Clear();
                sender.Dispose();
            }
        }

        //Call exit callback
        private void close()
        {
            btnExit_Click(null, EventArgs.Empty);
        }

        //Escape to exit
        private void CraftingWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                close();
            }
        }

    }
}
