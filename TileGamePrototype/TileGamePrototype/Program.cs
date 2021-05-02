using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TileGamePrototype
{
    static class Program
    {
        //The main background image was sourced from online
        //Background Image From: https://lh5.googleusercontent.com/-zURAdGPvF0c/TjDMTGK1iWI/AAAAAAAAAMw/oJH2lKR4zj0/s512/wood2.jpg


        //Global variables
        public static int formWidth = 576;
        public static int formHeight = 576;
        public static int level = -1;
        public static int numberOfLevels = 0;
        public static GameForm formRef;
        public static MenuScreen menuRef;
        public static LevelMap mapRef;
        public static bool male;

        public static LevelContainerClass.rank[] levelsComplete;//0 - incomplete, otherwise number of moves taken
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LevelContainerClass.initAllLevels();
            numberOfLevels = LevelContainerClass.GetLevelCount();
            levelsComplete = new LevelContainerClass.rank[numberOfLevels];
            if (!NextLevel())
                throw new Exception("Error generating the first level");
            formRef = new GameForm();
            mapRef = new LevelMap();
            menuRef = new MenuScreen();
            Application.Run(menuRef);
        }

        public static void LoadLevel()
        {
            //Remove tiles from the first level if another level is currently being played
            if (formRef != null)
                formRef.RemoveAllControls();
            //Load the new level
            levelHandler.GenerateLevel(level);
            //Check it completed
            if (levelHandler.currentLevelSurfaceArray == null)
            {
                Console.WriteLine("Did not complete level generation");
                Environment.Exit(1);
            }
            //Add the controls to the form if it exists
            if (formRef != null)
                formRef.AddAllControls();
        }

        //Switch to a given level
        public static bool SelectLevel(int _level)
        {
            if (_level != 0 && levelsComplete[_level - 1].Equals(new LevelContainerClass.rank()))
                return false;
            //Set the new level
            level = _level;
            LoadLevel();
            return true;
        }

        //Switch to the next level
        public static bool NextLevel()
        {
            if (level != -1 && level != numberOfLevels - 1 && levelsComplete[level].Equals(new LevelContainerClass.rank()))
                return false;
            level = (level + 1) % numberOfLevels;
            LoadLevel();
            return true;
        }

        //Restart the current level
        public static void RestartLevel()
        {
            LoadLevel();
        }
    }
    static class levelHandler
    {
        //Information about the level
        public static Tile[,] currentLevelSurfaceArray;
        public static Tile[,] currentLevelCaveArray;
        public static List<ObjectiveTypes> objectives;
        public static List<ItemTypes> itemsToCollect;
        static LevelBuilder levelBuilder = new LevelBuilder();
        public static Player player = null;
        public static Relic relic = new Relic(global::TileGamePrototype.Properties.Resources._32ItemBerry);
        public static int gold;
        public static int silver;
        public static int bronze;
        public static bool returnHome = false;

        public static LevelInfoWindow levelInfoWindow;

        //Delete the previous level's data
        private static void DeletePreviousArray()
        {
            if (player != null)
            {
                player.ClearSubscriptions();
                player.Dispose();
            }
            currentLevelSurfaceArray = null;
            currentLevelCaveArray = null;
            objectives = null;
            itemsToCollect = null;
            moveNumber = 0;
            gameOver = false;
            returnHome = false;
                
            player = null;
            relic = null;
        }

        //Generate the level specified by levelNumber
        public static void GenerateLevel(int levelNumber)
        {
            //Clear data
            DeletePreviousArray();
            //Get new level data
            LevelContainerClass.levelInfo levelInfo = LevelContainerClass.extractLevelInfo(levelNumber);
            //Load Help Window
            levelInfoWindow = new LevelInfoWindow();

            //Get level rankings
            gold = levelInfo.gold;
            silver = levelInfo.silver;
            bronze = levelInfo.bronze;
            //Update help window
            levelInfoWindow.lblMedalsList.Text = "Gold: " + gold.ToString() + "\nSilver: " + silver.ToString() + "\nBronze: " + bronze.ToString();
            //Reset the medal status icon
            if(Program.formRef != null)
                Program.formRef.naPicMedalIcon.Image = global::TileGamePrototype.Properties.Resources.GoldMedal;


            //Get list of objectives
            objectives = levelInfo.objectives;
            //Get list of items to collect
            itemsToCollect = levelInfo.itemsToCollect;
            //Check level robustness
            if (objectives.Contains(ObjectiveTypes.CollectItem) && itemsToCollect == null)
            {
                Console.WriteLine("Invalid Objectives Provided");
                return;
            }

            //Set variable used when checking if the player has to return to the village
            if (objectives.Contains(ObjectiveTypes.ReturnHome))
            {
                returnHome = true;
            }

            //////Update the help window with objective requirements
            string objectivesList = "";
            //Loop every objective. Not most efficient but convinient for getting the total count of any given objective. Enums are short so low cost
            foreach (ObjectiveTypes objective in Enum.GetValues(typeof(ObjectiveTypes)))
            {
                //Calculates the number of times that objective appears
                int count = ObjectiveOperators.getObjectiveOccurances(objectives, objective);
                //Check it is required
                if (count > 0)
                {
                    //List items that you need to find
                    if (objective == ObjectiveTypes.CollectItem)
                    {
                        objectivesList += "Collect Items: (" + ObjectiveOperators.getItemsToCollectString() + "), ";
                    }
                    //Ignore count for returning home
                    else if (objective == ObjectiveTypes.ReturnHome)
                    {
                        objectivesList += objective.ToString() + ", ";
                    }
                    //Else list how many times that objective is required
                    else
                    {
                        objectivesList += objective.ToString() + " x" + count.ToString() + ", ";
                    }
                }
            }
            objectivesList = objectivesList.Remove(objectivesList.Length - 2); // remove the last comma
            //Update help window
            levelInfoWindow.lblObjeciveList.Text = objectivesList;
            levelInfoWindow.lblobjRemainingList.Text = objectivesList;

            //Get Relic for current level
            relic = levelInfo.relic;

            //Update help window
            if (objectives.Contains(ObjectiveTypes.ObtainRelic))
            {
                //Check level robustness
                if (relic == null)
                    return;
                //Show the relic info box in the help window
                levelInfoWindow.lblRelicInfoList.Text = relic.info;
                levelInfoWindow.lblRelicInfoList.Show();
                levelInfoWindow.lblRelicInfoTitle.Show();
            }


            //////Generate level using level builder
            currentLevelSurfaceArray = levelBuilder.buildLevelSurface(levelInfo);

            //Caves were going to be added, code accomadating their addition exists but will rarely be used (such as Zones etc.). Not worth removing at present.
            /*if (levelInfo.containsCave)
            {
                currentLevelCaveArray = levelBuilder.buildLevelCave(levelInfo);
            }*/

            //Give animals paths for moving. This is buggy and does not support multiple animals moving and they can walk on water :-P
            if (levelInfo.paths != null)
            {
                //Loops through every path provided
                foreach (KeyValuePair<System.Drawing.Point, int[]> entry in levelInfo.paths)
                {
                    System.Drawing.Point location = entry.Key;
                    int[] path = entry.Value;
                    Tile tile = currentLevelSurfaceArray[location.X, location.Y];
                    //Checks if it is an animal and sets up its paths
                    if (tile.isLiving && !tile.isPlayer)
                    {
                        tile.setupPath(path);
                    }
                }
            }

            //Adds every animal to event
            foreach (Tile tile in currentLevelSurfaceArray)
            {
                if (tile.isLiving && !tile.isPlayer)
                {
                    player.playerMove += new Action(tile.playerAction);
                }
            }

            /*foreach (Tile tile in currentLevelCaveArray)
            {
                if (tile.isLiving && !tile.isPlayer)
                {
                    player.playerMove += new Action(tile.playerAction);
                }
            }*/

            //Adds animal movement handler to the move event
            player.playerMove += new Action(AnimalMovementHandler.handleAllRequests);
            //Add move counter to move event
            player.playerMove += new Action(moveCounter);
            //Add objective checking function to move event
            player.playerMove += new Action(checkLevelComplete);
            //Add check for if the player is recently deceased to move event
            player.playerMove += new Action(checkPlayerStatus);
            //Add restart level to death event
            player.playerDeath += new Action(restartLevel);
        }

        //Move counter
        static int moveNumber = 0;
        //Function for updating move counter
        public static void moveCounter()
        {
            moveNumber++;
            if(moveNumber <= gold)
                Program.formRef.naPicMedalIcon.Image = global::TileGamePrototype.Properties.Resources.GoldMedal;
            else if (moveNumber <= silver)
                Program.formRef.naPicMedalIcon.Image = global::TileGamePrototype.Properties.Resources.SilverMedal;
            else if (moveNumber <= bronze)
                Program.formRef.naPicMedalIcon.Image = global::TileGamePrototype.Properties.Resources.BronzeMedal;
            else
                Program.formRef.naPicMedalIcon.Image = null;
            Program.formRef.lblMoveCounter.Text = "Move: " + moveNumber.ToString();
        }

        //Function for restarting level if the player dies
        public static void restartLevel()
        {
            Utils.MessageBox levelFailed = new Utils.MessageBox(Program.formRef.ClientSize, Program.formRef.BackgroundImage);
            Program.formRef.Controls.Add(levelFailed);
            levelFailed.BringToFront();
            levelFailed.ShowMessageBox("Level Failed, Retry?", new Utils.MessageBox.DialogResultCallBack(RestartLevelCallback));
        }

        //Restart level message box callback
        public static void RestartLevelCallback(Utils.MessageBox sender, bool result)
        {
            if (result)
            {
                Program.RestartLevel();
            }
            else
            {
                Program.formRef.ReturnToMenu(null, EventArgs.Empty);
            }
            sender.Hide();
            Program.formRef.Controls.Remove(sender);
            sender.Controls.Clear();
            sender.Dispose();
        }

        //Handles if the level is complete and moves to the next level
        public static void levelComplete()
        {
            string rankString = "Level Complete";
            if (Program.levelsComplete[Program.level].medal == 0)//Will always be either 1,2,3 ir 4 if initialised
            {
                Program.levelsComplete[Program.level].movesTaken = Int32.MaxValue;
                Program.levelsComplete[Program.level].medal = 4;

                //Remove the lock symbol if this is ther first time you completed the level
                if (Program.level + 1 < Program.numberOfLevels)
                {
                    ((Utils.nonAntialiasingPictureBox)(Program.mapRef.Controls.Find("naPicLevel" + (Program.level + 2).ToString(), true).FirstOrDefault())).Image = null;
                }
                else
                {
                    ((Utils.nonAntialiasingPictureBox)(Program.mapRef.Controls.Find("naPicFinal", true).FirstOrDefault())).Image = null;
                }
            }
            if (moveNumber <= gold)
            {
                rankString = "Level Completed with GOLD rank";
                
                if (Program.levelsComplete[Program.level].medal > 1)
                {
                    ((Utils.nonAntialiasingPictureBox)(Program.mapRef.Controls.Find("naPicLevel" + (Program.level + 1).ToString(), true).FirstOrDefault())).Image = global::TileGamePrototype.Properties.Resources.GoldMedal;
                    Program.levelsComplete[Program.level].medal = 1;
                }
            }
            else if (moveNumber <= silver)
            {
                rankString = "Level Completed with Silver rank!";
                if (Program.levelsComplete[Program.level].medal > 2)
                {
                    ((Utils.nonAntialiasingPictureBox)(Program.mapRef.Controls.Find("naPicLevel" + (Program.level + 1).ToString(), true).FirstOrDefault())).Image = global::TileGamePrototype.Properties.Resources.SilverMedal;
                    Program.levelsComplete[Program.level].medal = 2;
                }
            }
            else if (moveNumber <= bronze)
            {
                rankString = "Level Completed with Bronze rank";
                if (Program.levelsComplete[Program.level].medal > 3)
                {
                    ((Utils.nonAntialiasingPictureBox)(Program.mapRef.Controls.Find("naPicLevel" + (Program.level + 1).ToString(), true).FirstOrDefault())).Image = global::TileGamePrototype.Properties.Resources.BronzeMedal;
                    Program.levelsComplete[Program.level].medal = 3;
                }
            }

            if(moveNumber < Program.levelsComplete[Program.level].movesTaken)//Update best score if it was improved
                Program.levelsComplete[Program.level].movesTaken = moveNumber;

            Utils.MessageBox levelComplete = new Utils.MessageBox(Program.formRef.ClientSize, Program.formRef.BackgroundImage);
            Program.formRef.Controls.Add(levelComplete);
            levelComplete.BringToFront();
            levelComplete.ShowMessageBox(rankString + "\n Continue to the next level", new Utils.MessageBox.DialogResultCallBack(LevelCompleteCallback));
        }

        //Callback for level complete message box
        private static void LevelCompleteCallback(Utils.MessageBox sender, bool result)
        {
            if (result)
            {
                Program.NextLevel();
            }
            else
            {
                Program.formRef.ReturnToMenu(null, EventArgs.Empty);
            }
            sender.Hide();
            Program.formRef.Controls.Remove(sender);
            sender.Controls.Clear();
            sender.Dispose();
        }

        //Variable for determining if the player should die
        public static bool gameOver = false;
        //Function where entities can politely ask for the player to die. Very descriptive name!
        public static void requestPlayersBrutallyPainfulDeath()
        {
            gameOver = true;
        }

        //Kills the player if he needs to die after move phase
        public static void checkPlayerStatus()
        {
            if (gameOver == true)
                player.Die();
        }

        //Checks if player has completed level
        public static void checkLevelComplete()
        {
            //Console.WriteLine("Checking Level Completeness");
            if (!checkObjectiveStatus() || returnHome)
            {
                return;
            }

            levelComplete();
        }

        //Check if all objectives are complete, updates help menu
        public static bool checkObjectiveStatus()
        {
            string[] objectivesList = levelInfoWindow.lblObjeciveList.Text.Split(',');
            string completed = "";
            bool success = true;
            foreach (ObjectiveTypes objective in Enum.GetValues(typeof(ObjectiveTypes)))
            {
                //Check it is required
                if (!objectives.Contains(objective))
                {
                    continue;
                }
                if (objective == ObjectiveTypes.ReturnHome)
                {
                    completed += Utils.findStringInArray(objectivesList, objective.ToString()) + ", ";
                    continue;
                }
                if (!ObjectiveOperators.meetsRequirements(objective))
                {
                    if (objective == ObjectiveTypes.CollectItem)
                    {
                        completed += Utils.findStringInArray(objectivesList, "Collect Item") + ", ";
                    }
                    else
                        completed += Utils.findStringInArray(objectivesList, objective.ToString()) + ", ";
                    success = false;
                }
            }
            if (completed != "")//Ensure values have been added
            {
                levelInfoWindow.lblobjRemainingList.Text = completed.Remove(completed.Length - 2);//Remove last comma
            }
            return success;
        }

        //Class for creating tiles from levelInfo struct
        class LevelBuilder
        {
            //Builds the surface
            public Tile[,] buildLevelSurface(LevelContainerClass.levelInfo levelInformation)
            {
                //Create array to add tiles to
                Tile[,] controlArray;
                controlArray = new Tile[levelInformation.width, levelInformation.height];
                int caveIndex = 0;// No longer used
                int tileSize = Math.Min(Program.formWidth, Program.formHeight) / Math.Max(levelInformation.width, levelInformation.height);//Set the tiles to be the size of the smallest dimension of the space it is in divided by the longest side of level
                //Loop every tile
                for (int x = 0; x < levelInformation.width; x++)
                {
                    for (int y = 0; y < levelInformation.height; y++)
                    {
                        //Handle player seperately
                        if (levelInformation.surface[x, y] == TileTypes.Player)
                        {
                            //Assume player is on grass
                            controlArray[x, y] = TileOperators.extractControlFromType(TileTypes.Grass, Zone.Surface);
                            
                            //Create the player
                            player = new Player(TileTypes.Grass);
                            player.hiddenControl = controlArray[x, y];
                            player.BringToFront();

                            //Sets the playersdetails
                            TileOperators.setTileDetails(player, tileSize, 0, 0);
                            //Override the players location
                            player.Location = new System.Drawing.Point(0, 0);
                        }
                        else
                        {
                            //Extract the tile at this location
                            controlArray[x, y] = TileOperators.extractControlFromType(levelInformation.surface[x, y], Zone.Surface);
                            //Set cave ID - Not used
                            if (levelInformation.surface[x, y] == TileTypes.Cave)
                            {
                                Cave cave = (Cave)controlArray[x, y];
                                cave.SetId(levelInformation.surfaceCaveId[caveIndex]);
                                caveIndex++;
                            }
                        }
                        //Set details of the tile at current location
                        TileOperators.setTileDetails(controlArray[x, y], tileSize, x, y);
                    }
                }
                //Return initialised level
                return controlArray;
            }
            //Deprecated cave generation
            /*public Tile[,] buildLevelCave(LevelContainerClass.levelInfo levelInformation)
            {
                Tile[,] controlArray;
                controlArray = new Tile[levelInformation.width, levelInformation.height];
                int caveIndex = 0;
                for (int x = 0; x < levelInformation.width; x++)
                {
                    for (int y = 0; y < levelInformation.height; y++)
                    {
                        if (levelInformation.cave[x, y] == TileTypes.Player)
                        {
                            controlArray[x, y] = TileOperators.extractControlFromType(TileTypes.Grass, Zone.Cave);
                            Player player = new Player(TileTypes.Grass);
                            player.hiddenControl = controlArray[x, y];
                            controlArray[x, y] = player;
                        }
                        else
                        {
                            controlArray[x, y] = TileOperators.extractControlFromType(levelInformation.cave[x, y], Zone.Cave);
                            if (levelInformation.cave[x, y] == TileTypes.Cave)
                            {
                                Cave cave = (Cave)controlArray[x, y];
                                cave.SetId(levelInformation.caveCaveId[caveIndex]);
                                caveIndex++;
                                foreach (Tile tile in levelHandler.currentLevelSurfaceArray)
                                {
                                    if (tile.GetType() == typeof(Cave))
                                    { 
                                        Cave surfCave = (Cave)tile;
                                        if (surfCave.caveId == cave.caveId)
                                        {
                                            surfCave.SetPair(cave);
                                            cave.SetPair(surfCave);
                                        }

                                    }
                                }
                            }
                        }
                        controlArray[x, y].Name = controlArray[x, y].GetType().ToString() + " at " + x.ToString() + ", " + y.ToString();
                        int tileSize = Math.Min(Program.formWidth, Program.formHeight) / Math.Max(levelInformation.caveWidth, levelInformation.caveHeight);
                        controlArray[x, y].Width = tileSize;
                        controlArray[x, y].Height = tileSize;
                        controlArray[x, y].Location = new System.Drawing.Point(tileSize * y, tileSize * x);
                    }
                }
                return controlArray;
            }*/
        }
    }
}
