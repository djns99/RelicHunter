using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TileGamePrototype
{
    // Generic Tile Related Functions
    public static class TileOperators
    {
        // Sets the details for a tile
        public static void setTileDetails(Tile tileRef, int tileSize, int x, int y)
        {
            
            tileRef.Name = tileRef.GetType().ToString() + " at " + x.ToString() + ", " + y.ToString();
            tileRef.Width = tileSize;
            tileRef.Height = tileSize;
            tileRef.Location = new System.Drawing.Point(tileSize * y, tileSize * x + GameForm.headerHeight);//Coordinate system appears to be inverted compared to mine
        }

        //Returns a tile control if passed a value from tile types enum
        public static Tile extractControlFromType(TileTypes tileType, Zone zone)
        {
            TileTypes baseType;
            if (zone == Zone.Cave)
            {
                baseType = TileTypes.Stone;
            }
            else
            {
                baseType = TileTypes.Grass;
            }
            switch (tileType)
            {
                case TileTypes.Grass:
                    {
                        return new Grass();
                    }
                case TileTypes.Stone:
                    {
                        return new Stone();
                    }
                case TileTypes.Tree:
                    {
                        return new Tree(baseType);
                    }
                case TileTypes.Sticks:
                    {
                        return new Sticks(baseType);
                    }
                case TileTypes.Rocks:
                    {
                        return new Rocks(baseType);
                    }
                case TileTypes.Bush:
                    {
                        return new Bush();
                    }
                case TileTypes.Water:
                    {
                        return new Water();
                    }
                case TileTypes.Ravine:
                    {
                        return new Ravine();
                    }
                case TileTypes.Bridge:
                    {
                        if (zone == Zone.Cave)
                        {
                            baseType = TileTypes.Ravine;
                        }
                        else
                        {
                            baseType = TileTypes.Water;
                        }
                        return new Bridge(baseType);
                    }
                case TileTypes.Cave:
                    {
                        return new Cave(baseType);
                    }
                case TileTypes.Hostile:
                    {
                        return new HostileAnimal(baseType);
                    }
                case TileTypes.Passive:
                    {
                        return new PassiveAnimal(baseType);
                    }
                case TileTypes.Treasure:
                    {
                        return new Treasure(baseType);
                    }
                case TileTypes.Chest:
                    {
                        return new Chest(baseType);
                    }
                case TileTypes.Home:
                    {
                        return new HomeVillage(baseType);
                    }
                default: // Defaults to base type
                    if (zone == Zone.Cave)
                    {
                        return new Stone();
                    }
                    else
                    {
                        return new Grass();
                    }
            };
        }

        //Changes the tile at a specific location, creates a new instance
        public static void changeTileAtLocation(Tile current, TileTypes replacement, Zone zone)
        {
            Point pos = TileOperators.getGridPosition(current);
            int posY = pos.X;//these must be inverted as the initial Point is inverted due to the initial inversion of the placements
            int posX = pos.Y;
            Tile newControl = extractControlFromType(replacement, zone);
            setTileDetails(newControl, current.Width, posX, posY);

            if (zone == Zone.Surface)
            {
                levelHandler.currentLevelSurfaceArray[posX, posY] = newControl;
                if (levelHandler.player.currentZone == Zone.Surface)
                {
                    Program.formRef.RemoveControl(current);
                    Program.formRef.AddControl(newControl);
                }
            }
            else if (zone == Zone.Cave)
            {
                levelHandler.currentLevelCaveArray[posX, posY] = newControl;
                if (levelHandler.player.currentZone == Zone.Cave)
                {
                    Program.formRef.RemoveControl(current);
                    Program.formRef.AddControl(newControl);
                } 
            }
        }

        //Moves the control to new location, preserves state
        public static void moveControlLocation(Tile toMove, TileTypes replaceWith, Zone zone, Point newLoc)
        {
            Point pos = TileOperators.getGridPosition(toMove);
            int posX = pos.Y;
            int posY = pos.X;
            Tile replacement = extractControlFromType(replaceWith, zone);
            setTileDetails(replacement, toMove.Width, posX, posY);
            setTileDetails(toMove, toMove.Width, newLoc.X, newLoc.Y);
            if (zone == Zone.Surface)
            {
                levelHandler.currentLevelSurfaceArray[posX, posY] = replacement;
                if (levelHandler.player.currentZone == Zone.Surface)
                {
                    Program.formRef.RemoveControl(levelHandler.currentLevelSurfaceArray[newLoc.X, newLoc.Y]);
                    Program.formRef.AddControl(replacement);
                    replacement.Show();
                }
                levelHandler.currentLevelSurfaceArray[newLoc.X, newLoc.Y] = toMove;
            }
            else if (zone == Zone.Cave)
            {
                levelHandler.currentLevelCaveArray[posX, posY] = replacement;
                if (levelHandler.player.currentZone == Zone.Cave)
                {
                    Program.formRef.RemoveControl(levelHandler.currentLevelCaveArray[newLoc.X, newLoc.Y]);
                    Program.formRef.AddControl(replacement);
                }
                levelHandler.currentLevelCaveArray[newLoc.X, newLoc.Y] = toMove;
            }
        }
        
        //Return the tiles location on the grid
        public static Point getGridPosition(Tile tile)
        { 
            int tileSize = tile.Width;
            return new Point(tile.Location.X / tileSize, (tile.Location.Y - GameForm.headerHeight) / tileSize);
        }
    }

    //Different Tile Types
    public enum TileTypes
    {
        Player,
        Grass,
        Stone,
        Tree,
        Sticks,
        Rocks,
        Bush,
        Water,
        Ravine,
        Bridge,
        Cave,
        Passive,
        Hostile,
        Treasure,
        Chest,
        Home
    }

    // Valid Actions Enum
    public enum Actions
    {
        Move,
        PickUp,
        Enter,
        Attack,
        BuildBridge,
        CutDown,
        Uncover,
        Collect
    }

    // Zones in Level - Unused but still required
    public enum Zone
    {
        Surface,
        Cave
    }

    //Generic Tile Class, inherits from Picture Box 
    public class Tile : System.Windows.Forms.PictureBox
    {
        // Common properties
        public bool isWalkable { get; set; }
        public bool isLiving { get; set; }
        public bool isPlayer = false;
        public TileTypes baseType { get; set; }
        public Actions[] validActions { get; set; }
        public InterpolationMode InterpolationMode { get; set; }

        // Constructors
        public Tile(TileTypes _baseType)
        {
            this.baseType = _baseType;
            init();
        }

        public Tile()
        {
            init();
        }

        // Initialises all common properties
        void init()
        {
            InterpolationMode = InterpolationMode.NearestNeighbor;
            base.Margin = new System.Windows.Forms.Padding(0);
            base.BorderStyle = System.Windows.Forms.BorderStyle.None;
            base.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        }

        // Returns The Valid Action Array
        public Actions[] GetActions()
        {
            return validActions;
        }

        //Checks if the player can interact with the tile
        public bool isInRange()
        {            
            Point curPos = TileOperators.getGridPosition(this);
            int curPosX = curPos.X;
            int curPosY = curPos.Y;
            Point playerPos = TileOperators.getGridPosition(levelHandler.player.hiddenControl);
            int playerPosX = playerPos.X;
            int playerPosY = playerPos.Y;

            if (Math.Abs(playerPosX - curPosX) > 1 || // X is more than one away
                Math.Abs(playerPosY - curPosY) > 1 || // Y is more than one away
                Math.Abs(playerPosX - curPosX) == Math.Abs(playerPosY - curPosY) // Not On a diagonal or the player's tile
                )
            {
                return false;
            }
            return true;
        }

        //Default Clicked On For Tiles That Are Walkable
        public virtual void ClickedOn(object sender, EventArgs e)
        {
            int tileSize = base.Size.Width;
            if (isInRange() && isWalkable && //Can Walk On It
                validActions.Length == 1) // Walking is the only valid option
            {
                Point pos = TileOperators.getGridPosition(this);
                int posX = pos.X;
                int posY = pos.Y;

                levelHandler.player.MovePlayer(posX, posY);
                levelHandler.player.callMove();
            }
        }

        //Found override here: https://stackoverflow.com/questions/29157/how-do-i-make-a-picturebox-use-nearest-neighbor-resampling
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs paintEventArgs)
        {
            paintEventArgs.Graphics.InterpolationMode = InterpolationMode;
            base.OnPaint(paintEventArgs);
        }

        //A method that entities (that perform actions when the player moves) can override - 'cause interfaces are to much effort
        public virtual void playerAction()
        { 
            
        }

        //A method for moving entities to override - 'cause interfaces are to much effort
        public virtual void setupPath(int[] _path) { }
    }

    #region TileClassDefinitions

    public class Player : Tile
    {
        public event Action playerDeath;
        public event Action playerMove;
        public List<Item> inventory;
        public void ClearSubscriptions()
        {
            playerDeath = null;
            playerMove = null;
        }

        public Tile hiddenControl;
        public Zone currentZone;
        public Player(TileTypes _baseType)
            : base(_baseType)
        {
            base.isPlayer = true;
            base.isWalkable = false;
            base.isLiving = true;
            base.validActions = null;
            base.BackColor = Color.Transparent;
            inventory = new List<Item>();
            if (Program.male)
            {
                base.Image = global::TileGamePrototype.Properties.Resources._32TransparentPlayer;
            }
            else
            {
                base.Image = global::TileGamePrototype.Properties.Resources._32TransparentPlayerFemale;
            }
            if (_baseType == TileTypes.Stone)
            {
                currentZone = Zone.Cave;
            }
            else 
            {
                currentZone = Zone.Surface;
            }
        }

        public void changeZone(Zone zone, int newGridX, int newGridY)
        {
            //base.Image = global::TileGamePrototype.Properties.Resources._32TransparentPlayer;
            currentZone = zone;
            if (currentZone == Zone.Surface)
            {
                hiddenControl = levelHandler.currentLevelSurfaceArray[newGridX, newGridY];
            }
            else
            {
                hiddenControl = levelHandler.currentLevelCaveArray[newGridX, newGridY];
            }
            MovePlayer(newGridX, newGridY);
        }

        public void MovePlayer(int newGridX, int newGridY)
        {
            Tile[,] manipulateArray;

            if (currentZone == Zone.Surface)
            {
                manipulateArray = levelHandler.currentLevelSurfaceArray;
            }
            else
            {
                manipulateArray = levelHandler.currentLevelCaveArray;
            }
            //TODO Sort out parenting cause stuff is messed up
            /*hiddenControl = manipulateArray[newGridY, newGridX];
            this.Parent = null;
            this.BringToFront();
            hiddenControl.SendToBack();
            this.Parent = hiddenControl;
            base.Location = hiddenControl.Location;*/

            hiddenControl = manipulateArray[newGridY, newGridX];
            this.Parent = this.hiddenControl;
        }

        public void CollectItem(ItemTypes _item)
        {
            Console.WriteLine("Player Recieved: " + _item.ToString());
            Item item = ItemOperators.extractControlFromType(_item);
            inventory.Add(item);
        }

        public bool HasItem(ItemTypes _item, int number = 1)
        {
            int count = 0;
            if (number == 0)
                return true;
            if (number > inventory.Count)
                return false;
            Item exampleItem = ItemOperators.extractControlFromType(_item);
            foreach (Item item in inventory)
            {
                if (item.GetType().ToString() == exampleItem.GetType().ToString())
                {
                    count++; 
                }
            }
            if (count >= number)
                return true;  
            return false;
        }

        public void RemoveItem(ItemTypes _item)
        {
            Item exampleItem = ItemOperators.extractControlFromType(_item);
            foreach (Item item in inventory)
            {
                if (item.GetType().ToString() == exampleItem.GetType().ToString())
                {
                    inventory.Remove(item);
                    return;
                }
            }            
        }

        public void Die()
        {
            if (playerDeath != null)
            {
                playerDeath();
            }
            //TileOperators.changeTileAtLocation(this, base.baseType, currentZone);
        }

        public void callMove()
        {
            playerMove();
        }

        protected override void Dispose(bool disposing)
        {
            ClearSubscriptions();
            base.Dispose(disposing);
        }

    }

    public class Grass : Tile
    {
        public Grass() : base()
        {
            base.isWalkable = true;
            base.isLiving = false;
            base.baseType = TileTypes.Grass;
            base.validActions = new Actions[] { Actions.Move };
            base.Image = global::TileGamePrototype.Properties.Resources._32Grass;
        }
    }

    public class Stone : Tile
    {
        public Stone() : base()
        {
            base.isWalkable = true;
            base.isLiving = false;
            base.baseType = TileTypes.Stone;
            base.validActions = new Actions[] { Actions.Move };
            base.Image = global::TileGamePrototype.Properties.Resources._32Stone;
        }
    }

    public class Tree : Tile
    {
        public Tree(TileTypes _baseType)
            : base(_baseType)
        {
            base.isWalkable = false;
            base.isLiving = false;
            base.validActions = new Actions[] { Actions.CutDown };
            base.Image = global::TileGamePrototype.Properties.Resources._32GrassTree;
        }

        private void cutDown(Utils.MessageBox sender, bool result)
        {
            if (result && levelHandler.player.HasItem(ItemTypes.Axe))//It is possible for the player to use the item before pressing yes.
            {
                TileOperators.changeTileAtLocation(this, base.baseType, Zone.Surface);
                levelHandler.player.RemoveItem(ItemTypes.Axe);
                levelHandler.player.CollectItem(ItemTypes.Log);
                levelHandler.player.callMove();
            }
            sender.Hide();
            Program.formRef.Controls.Remove(sender);
            sender.Controls.Clear();
            sender.Dispose();
        }

        public override void ClickedOn(object sender, EventArgs e)
        {
            if (base.isInRange())
            {
                if (levelHandler.player.HasItem(ItemTypes.Axe))
                {
                    Utils.MessageBox messageBox = new Utils.MessageBox(Program.formRef.ClientSize, global::TileGamePrototype.Properties.Resources.WoodTexture);
                    Program.formRef.Controls.Add(messageBox);
                    messageBox.BringToFront();
                    messageBox.ShowMessageBox("Do you wish to cut down the tree?", new Utils.MessageBox.DialogResultCallBack(cutDown));
                }
                else
                {
                    Console.WriteLine("Requires Axe");
                }
            }
        }

    }

    public class Sticks : Tile
    {
        public Sticks(TileTypes _baseType)
            : base(_baseType)
        {
            base.isWalkable = false;
            base.isLiving = false;
            base.validActions = new Actions[] { Actions.PickUp };
            if (_baseType == TileTypes.Stone)
            {
                base.Image = global::TileGamePrototype.Properties.Resources._32StoneStick;
            }
            else
            {
                base.Image = global::TileGamePrototype.Properties.Resources._32GrassStick;
            }
        }

        public void pickUp()
        {
            if (base.baseType == TileTypes.Grass)
            {
                TileOperators.changeTileAtLocation(this, base.baseType, Zone.Surface);
                levelHandler.player.CollectItem(ItemTypes.Stick);
            }
            else if (base.baseType == TileTypes.Stone)
            {
                TileOperators.changeTileAtLocation(this, base.baseType, Zone.Cave);
                levelHandler.player.CollectItem(ItemTypes.Stick);
            }
            levelHandler.player.callMove();
        }

        public override void ClickedOn(object sender, EventArgs e)
        {
            if (base.isInRange())
            {
                pickUp();
            }
        }
    }

    public class Rocks : Tile
    {
        public Rocks(TileTypes _baseType)
            : base(_baseType)
        {
            base.isWalkable = false;
            base.isLiving = false;
            base.validActions = new Actions[] { Actions.PickUp };
            if (_baseType == TileTypes.Stone)
            {
                base.Image = global::TileGamePrototype.Properties.Resources._32StoneRock;
            }
            else
            {
                base.Image = global::TileGamePrototype.Properties.Resources._32GrassRock;
            }
        }

        public void pickUp()
        {
            if (base.baseType == TileTypes.Grass)
            {
                TileOperators.changeTileAtLocation(this, base.baseType, Zone.Surface);
            }
            else if (base.baseType == TileTypes.Stone)
            {
                TileOperators.changeTileAtLocation(this, base.baseType, Zone.Cave);
            }
            levelHandler.player.CollectItem(ItemTypes.Rock);
            levelHandler.player.callMove();
        }

        public override void ClickedOn(object sender, EventArgs e)
        {
            if (base.isInRange())
            {
                pickUp();
            }
        }

    }

    public class Bush : Tile
    {
        public Bush(): base()
        {
            base.isWalkable = false;
            base.isLiving = false;
            base.validActions = new Actions[] { Actions.PickUp };
            base.Image = global::TileGamePrototype.Properties.Resources._32GrassBush;
        }

        public void pickUp()
        {
            TileOperators.changeTileAtLocation(this, base.baseType, Zone.Surface);
            levelHandler.player.CollectItem(ItemTypes.Berries);
            levelHandler.player.callMove();
        }

        public override void ClickedOn(object sender, EventArgs e)
        {
            if (base.isInRange())
            {
                pickUp();
            }
        }
    }

    public class Water : Tile
    {
        public Water() : base()
        {
            base.isWalkable = false;
            base.isLiving = false;
            base.validActions = new Actions[] { Actions.BuildBridge };
            base.Image = global::TileGamePrototype.Properties.Resources.WaterVertical;
        }

        private void buildBridge(Utils.MessageBox sender, bool result)
        {
            if (result && levelHandler.player.HasItem(ItemTypes.Log))
            {
                TileOperators.changeTileAtLocation(this, TileTypes.Bridge, Zone.Surface);
                levelHandler.player.RemoveItem(ItemTypes.Log);
                levelHandler.player.callMove();
            }
            sender.Hide();
            Program.formRef.Controls.Remove(sender);
            sender.Controls.Clear();
            sender.Dispose();
        }

        public override void ClickedOn(object sender, EventArgs e)
        {
            if (base.isInRange())
            {
                if (levelHandler.player.HasItem(ItemTypes.Log))
                {
                    Utils.MessageBox messageBox = new Utils.MessageBox(Program.formRef.ClientSize, global::TileGamePrototype.Properties.Resources.WoodTexture);
                    Program.formRef.Controls.Add(messageBox);
                    messageBox.BringToFront();
                    messageBox.ShowMessageBox("Do you wish to build a bridge?", new Utils.MessageBox.DialogResultCallBack(buildBridge));
                }
                else
                {
                    Console.WriteLine("Requires Log");
                }
            }
        }
    }

    public class Ravine : Tile
    {
        public Ravine() : base()
        {
            base.isWalkable = false;
            base.isLiving = false;
            base.validActions = new Actions[] { Actions.BuildBridge };
            base.Image = global::TileGamePrototype.Properties.Resources.Ravine;
        }
        //TODO Change the zone to cave if implemented
        private void buildBridge(Utils.MessageBox sender, bool result)
        {
            if (result && levelHandler.player.HasItem(ItemTypes.Log))
            {
                TileOperators.changeTileAtLocation(this, TileTypes.Bridge, Zone.Surface);
                levelHandler.player.RemoveItem(ItemTypes.Log);
                levelHandler.player.callMove();
            }
            sender.Hide();
            Program.formRef.Controls.Remove(sender);
            sender.Controls.Clear();
            sender.Dispose();
        }

        public override void ClickedOn(object sender, EventArgs e)
        {
            if (base.isInRange())
            {
                if (levelHandler.player.HasItem(ItemTypes.Log))
                {
                    Utils.MessageBox messageBox = new Utils.MessageBox(Program.formRef.ClientSize, global::TileGamePrototype.Properties.Resources.WoodTexture);
                    Program.formRef.Controls.Add(messageBox);
                    messageBox.BringToFront();
                    messageBox.ShowMessageBox("Do you wish to build a bridge?", new Utils.MessageBox.DialogResultCallBack(buildBridge));
                }
                else
                {
                    Console.WriteLine("Requires Log");
                }
            }
        }
    }

    public class Bridge : Tile
    {
        public Bridge(TileTypes _baseType)
            : base(_baseType)
        {
            base.isWalkable = true;
            base.isLiving = false;
            base.validActions = new Actions[] { Actions.Move };
            if (_baseType == TileTypes.Water)
            {
                base.Image = global::TileGamePrototype.Properties.Resources.WaterBridge;
            }
            else
            {
                base.Image = global::TileGamePrototype.Properties.Resources.RavineBridge;
            }
        }
    }

    public class Cave : Tile
    {
        public int caveId;
        Cave cavePair;
        public Cave(TileTypes _baseType)
            : base(_baseType)
        {
            base.isWalkable = false;
            base.isLiving = false;
            base.validActions = new Actions[] { Actions.Enter };
            if (_baseType == TileTypes.Stone)
            {
                base.Image = global::TileGamePrototype.Properties.Resources._32StoneCave;
            }
            else
            {
                base.Image = global::TileGamePrototype.Properties.Resources._32GrassCave;
            }
        }

        public void SetId(int _caveId)
        {
            this.caveId = _caveId;
        }

        public void SetPair(Cave _cavePair)
        {
            this.cavePair = _cavePair;
        }
    }

    public class PassiveAnimal : Tile
    {
        private int[] path;
        private int pathLoc;
        private bool reversed;

        public PassiveAnimal(TileTypes _baseType)
            : base(_baseType)
        {
            base.isWalkable = false;
            base.isLiving = true;
            base.validActions = new Actions[] { Actions.Attack };
            this.path = null;
            this.pathLoc = 0;
            this.reversed = false;
            if (_baseType == TileTypes.Stone)
            {
                base.Image = global::TileGamePrototype.Properties.Resources._32StoneAnimal;
            }
            else
            {
                base.Image = global::TileGamePrototype.Properties.Resources._32GrassAnimal;
            }
        }

        public override void setupPath(int[] _path)
        {
            this.path = _path;
        }

        public Point getMovementLocation(int pathLocAlt, bool reversed)
        {
            Point loc = TileOperators.getGridPosition(this);
            int modifier = reversed ? 2 : 0; // Switches direction when going backward through the path
            if ((path[pathLocAlt] + modifier - 1) % 4 + 1 == 1)//Up
            {
                loc.Y--;
            }
            else if ((path[pathLocAlt] + modifier - 1) % 4 + 1 == 2)//Right
            {
                loc.X++;
            }
            else if ((path[pathLocAlt] + modifier - 1) % 4 + 1 == 3)//Down
            {
                loc.Y++;
            }
            else if ((path[pathLocAlt] + modifier - 1) % 4 + 1 == 4)//Left
            {
                loc.X--;
            }
            return new Point(loc.Y, loc.X);//Needs Flipped
        }

        public void movementCallBack(bool reversed)
        {
            if (reversed)
            {
                this.reversed = !this.reversed;
            }
            Point loc = getMovementLocation(pathLoc, this.reversed);
            pathLoc += (!this.reversed) ? 1 : -1;
            if (pathLoc < 0)
            {
                pathLoc = path.Length - 1;
            }
            if (pathLoc >= path.Length)
            {
                pathLoc = 0;
            }
            if (reversed)
            {
                loc = getMovementLocation(pathLoc, this.reversed);
            }
            TileOperators.moveControlLocation(this, this.baseType, (this.baseType == TileTypes.Stone) ? Zone.Cave : Zone.Surface, loc);
        }

        public override void playerAction()
        {
            if (path != null)
            {
                Point curLoc = TileOperators.getGridPosition(this);
                //Get the reversed path location
                int revPathLoc = (pathLoc + (reversed ? 1 : -1)) % path.Length;
                if (revPathLoc < 0)
                {
                    revPathLoc = path.Length - 1;
                }
                AnimalMovementHandler.Request request = new AnimalMovementHandler.Request(new Point(curLoc.Y, curLoc.X), getMovementLocation(pathLoc, false), getMovementLocation(revPathLoc, true), new AnimalMovementHandler.movementHandlerCallBack(movementCallBack), new AnimalMovementHandler.movementHandlerDeathCallBack(animalAttack), 0);
                AnimalMovementHandler.requestMovement(request);
            }
        }

        private void attack(Utils.MessageBox sender, bool result)
        {
            if (result && levelHandler.player.HasItem(ItemTypes.Sword))
            {
                levelHandler.player.RemoveItem(ItemTypes.Sword);
                levelHandler.player.CollectItem(ItemTypes.Meat);
                if (base.baseType == TileTypes.Grass)
                {
                    TileOperators.changeTileAtLocation(this, base.baseType, Zone.Surface);
                }
                else if (base.baseType == TileTypes.Stone)
                {
                    TileOperators.changeTileAtLocation(this, base.baseType, Zone.Cave);
                }
                levelHandler.player.callMove();
            }
            sender.Hide();
            Program.formRef.Controls.Remove(sender);
            sender.Controls.Clear();
            sender.Dispose();
        }

        public override void ClickedOn(object sender, EventArgs e)
        {
            if (base.isInRange())
            {
                if (levelHandler.player.HasItem(ItemTypes.Sword))
                {
                    Utils.MessageBox messageBox = new Utils.MessageBox(Program.formRef.ClientSize, global::TileGamePrototype.Properties.Resources.WoodTexture);
                    Program.formRef.Controls.Add(messageBox);
                    messageBox.BringToFront();
                    messageBox.ShowMessageBox("Do you wish to slay the cow?", new Utils.MessageBox.DialogResultCallBack(attack));
                }
                else
                {
                    Console.WriteLine("Requires Sword");
                }
            }
        }

        public void animalAttack()
        {
            if (base.baseType == TileTypes.Grass)
            {
                TileOperators.changeTileAtLocation(this, base.baseType, Zone.Surface);
            }
            else if (base.baseType == TileTypes.Stone)
            {
                TileOperators.changeTileAtLocation(this, base.baseType, Zone.Cave);
            }
        }
    }

    public class HostileAnimal : Tile
    {
        private int[] path;
        private int pathLoc;
        private bool reversed;

        public HostileAnimal(TileTypes _baseType)
            : base(_baseType)
        {
            base.isWalkable = false;
            base.isLiving = true;
            base.validActions = null;
            this.path = null;
            this.pathLoc = 0;
            this.reversed = false;
            if (_baseType == TileTypes.Stone)
            {
                base.Image = global::TileGamePrototype.Properties.Resources._32StoneHostile;
            }
            else
            {
                base.Image = global::TileGamePrototype.Properties.Resources._32GrassHostile;
            }
        }

        public override void setupPath(int[] _path)
        {
            this.path = _path;
        }

        public void attackNeighbouringTiles()
        {
            if (base.isInRange())
            {
                levelHandler.requestPlayersBrutallyPainfulDeath();
                return;
            }
            Point curLoc = TileOperators.getGridPosition(this);
            for (int i = -1; i <= 2; i += 2)
            {
                if(curLoc.Y + i < 0 || curLoc.Y + i >= levelHandler.currentLevelSurfaceArray.GetLength(0))
                {
                    continue;
                }
                Tile neighbour = levelHandler.currentLevelSurfaceArray[curLoc.Y + i, curLoc.X];
                if (neighbour.GetType().ToString() == (TileOperators.extractControlFromType(TileTypes.Passive, Zone.Surface)).GetType().ToString())
                {
                    PassiveAnimal animal = (PassiveAnimal)neighbour;
                    animal.animalAttack();
                }
            }
            for (int i = -1; i <= 2; i += 2)
            {
                if (curLoc.X + i < 0 || curLoc.X + i >= levelHandler.currentLevelSurfaceArray.GetLength(1))
                {
                    continue;
                }
                Tile neighbour = levelHandler.currentLevelSurfaceArray[curLoc.Y, curLoc.X + i];
                if (neighbour.GetType().ToString() == (TileOperators.extractControlFromType(TileTypes.Passive, Zone.Surface)).GetType().ToString())
                {
                    PassiveAnimal animal = (PassiveAnimal)neighbour;
                    animal.animalAttack();
                }
            }
        }

        public Point getMovementLocation(int pathLocAlt, bool reversed)
        {
            Point loc = TileOperators.getGridPosition(this);
            int modifier = reversed ? 2 : 0; // Switches direction when going backward through the path
            if ((path[pathLocAlt] + modifier - 1) % 4 + 1 == 1)//Up
            {
                loc.Y--;
            }
            else if ((path[pathLocAlt] + modifier - 1) % 4 + 1 == 2)//Right
            {
                loc.X++;
            }
            else if ((path[pathLocAlt] + modifier - 1) % 4 + 1 == 3)//Down
            {
                loc.Y++;
            }
            else if ((path[pathLocAlt] + modifier - 1) % 4 + 1 == 4)//Left
            {
                loc.X--;
            }
            return new Point(loc.Y, loc.X);//Needs Flipped
        }

        public void movementCallBack(bool reversed)
        {
            if (reversed)
            {
                this.reversed = !this.reversed;
            }
            Point loc = getMovementLocation(pathLoc, this.reversed);
            pathLoc += (!this.reversed) ? 1 : -1;
            if (pathLoc < 0)
            {
                pathLoc = path.Length - 1;
            }
            if (pathLoc >= path.Length)
            {
                pathLoc = 0;
            }
            if (reversed)
            {
                loc = getMovementLocation(pathLoc, this.reversed);
            }
            TileOperators.moveControlLocation(this, this.baseType, (this.baseType == TileTypes.Stone) ? Zone.Cave : Zone.Surface, loc);
            attackNeighbouringTiles();
        }

        public override void playerAction()
        {
            attackNeighbouringTiles();
            if (path != null)
            {
                Point curLoc = TileOperators.getGridPosition(this);
                //Get the reversed path location
                int revPathLoc = (pathLoc + (reversed ? 1 : -1)) % path.Length;
                if (revPathLoc < 0)
                {
                    revPathLoc = path.Length - 1;
                }
                AnimalMovementHandler.Request request = new AnimalMovementHandler.Request(new Point(curLoc.Y, curLoc.X), getMovementLocation(pathLoc, false), getMovementLocation(revPathLoc, true), new AnimalMovementHandler.movementHandlerCallBack(movementCallBack), new AnimalMovementHandler.movementHandlerDeathCallBack(Die), 1);
                AnimalMovementHandler.requestMovement(request);
            }
        }

        public void Die()
        {
            Console.WriteLine("Ummmmmmmmmmm This Shouldn't have happened...");
        }
    }

    public class Treasure : Tile
    {
        public Treasure(TileTypes _baseType)
            : base(_baseType)
        {
            base.isWalkable = true;
            base.isLiving = false;
            base.validActions = new Actions[] { Actions.Move, Actions.Uncover };
            if (_baseType == TileTypes.Stone)
            {
                base.Image = global::TileGamePrototype.Properties.Resources._32StoneTreasure;
            }
            else
            {
                base.Image = global::TileGamePrototype.Properties.Resources._32GrassTreasure;
            }
        }

        private void uncover(Utils.MessageBox sender, bool result)
        {
            if (result && levelHandler.player.HasItem(ItemTypes.Spade))
            {
                levelHandler.player.RemoveItem(ItemTypes.Spade);
                if (base.baseType == TileTypes.Grass)
                {
                    TileOperators.changeTileAtLocation(this, TileTypes.Chest, Zone.Surface);
                }
                else if (base.baseType == TileTypes.Stone)
                {
                    TileOperators.changeTileAtLocation(this, TileTypes.Chest, Zone.Cave);
                }
                levelHandler.player.callMove();
            }
            else if (base.isInRange())//Player could theoretically move before pressing no
            {
                Point pos = TileOperators.getGridPosition(this);
                levelHandler.player.MovePlayer(pos.X, pos.Y);
                levelHandler.player.callMove();
            }
            sender.Hide();
            Program.formRef.Controls.Remove(sender);
            sender.Controls.Clear();
            sender.Dispose();
        }

        public override void ClickedOn(object sender, EventArgs e)
        {
            if (base.isInRange())
            {
                if (levelHandler.player.HasItem(ItemTypes.Spade))
                {
                    Utils.MessageBox messageBox = new Utils.MessageBox(Program.formRef.ClientSize, global::TileGamePrototype.Properties.Resources.WoodTexture);
                    Program.formRef.Controls.Add(messageBox);
                    messageBox.BringToFront();
                    messageBox.ShowMessageBox("Do you wish to uncover the treasure?", new Utils.MessageBox.DialogResultCallBack(uncover));
                }
                else
                {
                    Point pos = TileOperators.getGridPosition(this);
                    levelHandler.player.MovePlayer(pos.X, pos.Y);
                    levelHandler.player.callMove();
                }
            }
        }
    }

    public class Chest : Tile
    {
        public Chest(TileTypes _baseType)
            : base(_baseType)
        {
            base.isWalkable = false;
            base.isLiving = false;
            base.validActions = new Actions[] { Actions.Collect };
            if (_baseType == TileTypes.Stone)
            {
                base.Image = global::TileGamePrototype.Properties.Resources._32StoneChest;
            }
            else
            {
                base.Image = global::TileGamePrototype.Properties.Resources._32GrassChest;
            }
        }

        public void collect()
        {
            levelHandler.player.CollectItem(ItemTypes.Relic);
            if (base.baseType == TileTypes.Grass)
            {
                TileOperators.changeTileAtLocation(this, base.baseType, Zone.Surface);
            }
            else if (base.baseType == TileTypes.Stone)
            {
                TileOperators.changeTileAtLocation(this, base.baseType, Zone.Cave);
            }
            levelHandler.player.callMove();
        }

        public override void ClickedOn(object sender, EventArgs e)
        {
            if (base.isInRange())
            {
                collect();
            }
        }
    }

    public class HomeVillage : Tile
    {
        public HomeVillage(TileTypes _baseType)
            : base(_baseType)
        {
            base.isWalkable = false;
            base.isLiving = false;
            base.validActions = new Actions[] { Actions.Enter };
            if (_baseType == TileTypes.Stone)
            {
                base.Image = global::TileGamePrototype.Properties.Resources._32StoneVillage;
            }
            else
            {
                base.Image = global::TileGamePrototype.Properties.Resources._32GrassVillage;
            }
        }

        //Overrides the default level completion checking function and changes level when complete
        public void enter()
        {
            if (levelHandler.checkObjectiveStatus() && levelHandler.returnHome)
            {
                levelHandler.player.callMove();
                levelHandler.levelComplete();
            }
        }

        public override void ClickedOn(object sender, EventArgs e)
        {
            if (base.isInRange())
            {
                enter();
            }
        }
    }
    

    #endregion

}
