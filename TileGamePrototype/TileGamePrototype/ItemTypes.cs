using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace TileGamePrototype
{
    public static class ItemOperators
    {
        //Returns item from enum value
        public static Item extractControlFromType(ItemTypes itemType)
        {
            switch (itemType)
            {
                case ItemTypes.Stick:
                    {
                        return new Stick();
                    }
                case ItemTypes.Rock:
                    {
                        return new Rock();
                    }
                case ItemTypes.Meat:
                    {
                        return new Meat();
                    }
                case ItemTypes.Berries:
                    {
                        return new Berries();
                    }
                case ItemTypes.Log:
                    {
                        return new Log();
                    }
                case ItemTypes.Axe:
                    {
                        return new Axe();
                    }
                case ItemTypes.Sword:
                    {
                        return new Sword();
                    }
                case ItemTypes.Spade:
                    {
                        return new Spade();
                    }
                //Special Case. This will be a specific Relic for the level.
                case ItemTypes.Relic:
                    {
                        return levelHandler.relic;
                    }
                default:
                    {
                        return null;
                    }
            };
        }
    }

    //Items Enum
    public enum ItemTypes
    {
        Stick,
        Rock,
        Meat,
        Berries,
        Log,
        Axe,
        Sword,
        Spade,
        Relic
    }

    //Generic Item class
    public class Item
    {
        public Image sprite;//Image
        public TileTypes[] targetTiles;//Tiles it can be used on
        public ItemTypes[] craftingItems;//Items to create it
        public bool isFood;//Counts as a food item
    }

    #region ItemClassDefinitions

    public class Stick : Item
    {
        public Stick()
        {
            base.sprite = global::TileGamePrototype.Properties.Resources._32ItemStick;
            base.targetTiles = null;
            base.craftingItems = null;
            base.isFood = false;
        }
    }

    public class Rock : Item
    {
        public Rock()
        {
            base.sprite = global::TileGamePrototype.Properties.Resources._32ItemRock;
            base.targetTiles = null;
            base.craftingItems = null;
            base.isFood = false;
        }
    }

    public class Meat : Item
    {
        public Meat()
        {
            base.sprite = global::TileGamePrototype.Properties.Resources._32ItemMeat;
            base.targetTiles = null;
            base.craftingItems = null;
            base.isFood = true;
        }
    }

    public class Berries : Item
    {
        public Berries()
        {
            base.sprite = global::TileGamePrototype.Properties.Resources._32ItemBerry;
            base.targetTiles = null;
            base.craftingItems = null;
            base.isFood = true;
        }
    }

    public class Log : Item
    {
        public Log()
        {
            base.sprite = global::TileGamePrototype.Properties.Resources._32ItemLog;
            base.targetTiles = new TileTypes[] { TileTypes.Water, TileTypes.Ravine };
            base.craftingItems = null;
            base.isFood = false;
        }
    }

    public class Axe : Item
    {
        public Axe()
        {
            base.sprite = global::TileGamePrototype.Properties.Resources._32ItemAxe;
            base.targetTiles = new TileTypes[] { TileTypes.Tree };
            base.craftingItems = new ItemTypes[] { ItemTypes.Stick, ItemTypes.Rock };
            base.isFood = false;
        }
    }

    public class Sword : Item
    {
        public Sword()
        {
            base.sprite = global::TileGamePrototype.Properties.Resources._32ItemSword;
            base.targetTiles = new TileTypes[] { TileTypes.Hostile };
            base.craftingItems = new ItemTypes[] { ItemTypes.Stick, ItemTypes.Rock }; ;
            base.isFood = false;
        }
    }

    public class Spade : Item
    {
        public Spade()
        {
            base.sprite = global::TileGamePrototype.Properties.Resources._32ItemSpade;
            base.targetTiles = new TileTypes[] { TileTypes.Treasure };
            base.craftingItems = new ItemTypes[] { ItemTypes.Stick, ItemTypes.Rock }; ;
            base.isFood = false;
        }
    }

    public class Relic : Item
    {
        public string info;

        public Relic(Image image)
        {
            base.sprite = image;
            base.targetTiles = null;
            base.craftingItems = null;
            base.isFood = false;
            info = "<Name>\nThis is a relic from <Insert Continent>\nIt has historical/cultural value to the local people because...\nSee www.example-website.com for more information";
        }
    }

    #endregion

}
