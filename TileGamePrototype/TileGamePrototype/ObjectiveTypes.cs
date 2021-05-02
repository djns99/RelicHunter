using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileGamePrototype
{
    public static class ObjectiveOperators
    {
        //Calculates the number of times that objective is required
        public static int getObjectiveOccurances(List<ObjectiveTypes> list, ObjectiveTypes objectiveToCount)
        {
            int count = 0;
            foreach (ObjectiveTypes listObjective in list)
            {
                if (listObjective == objectiveToCount)
                    count++;
            }
            return count;
        }

        //Checks if the player has meet the requirements for the given objective
        public static bool meetsRequirements(ObjectiveTypes objective)
        {
            int countRequired = getObjectiveOccurances(levelHandler.objectives, objective);
            if (countRequired == 0)
            {
                return true;
            }
            switch (objective)
            {
                case ObjectiveTypes.FindFood:
                    for(int i = 0; i <= countRequired; i++)
                    {
                        if (levelHandler.player.HasItem(ItemTypes.Meat, i) &&
                            levelHandler.player.HasItem(ItemTypes.Berries, countRequired - i))
                        {
                            return true;
                        }
                    }
                    return false;
                case ObjectiveTypes.ReturnHome: //Shouldn't be handled by this, should be handled by the act of entering the village, as it cannot be done without all other objectives being complete
                    if (levelHandler.player.hiddenControl.GetType().ToString() == TileOperators.extractControlFromType(TileTypes.Home, Zone.Surface).GetType().ToString())
                    {
                        return true;
                    }
                    else 
                    {
                        return false;
                    }
                case ObjectiveTypes.CollectItem:
                    Dictionary<ItemTypes, int> countPerItem = new Dictionary<ItemTypes, int>();
                    foreach (ItemTypes itemType in levelHandler.itemsToCollect)
                    {
                        if (countPerItem.ContainsKey(itemType))
                        {
                            countPerItem[itemType]++;
                        }
                        else 
                        {
                            countPerItem.Add(itemType, 1);
                        }
                    }

                    foreach(ItemTypes itemType in Enum.GetValues(typeof(ItemTypes)))
                    {
                        //Check if the item has an entry in the dictionary
                        int val;
                        if (!countPerItem.TryGetValue(itemType, out val))
                        {
                            continue;
                        }
                        if (!levelHandler.player.HasItem(itemType, val))
                        {
                            return false;
                        }
                    }
                    return true;
                case ObjectiveTypes.ObtainRelic:
                    if (levelHandler.player.HasItem(ItemTypes.Relic, countRequired))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return false;
            }
        }

        //Returns string containing the items that need to be collected
        public static string getItemsToCollectString()
        {
            string list = "";
            foreach (ItemTypes itemToCollect in Enum.GetValues(typeof(ItemTypes)))
            {
                if (levelHandler.itemsToCollect.Contains(itemToCollect))
                {
                    int count = 0;
                    foreach (ItemTypes item in levelHandler.itemsToCollect)
                    {
                        if (item == itemToCollect)
                            count++;
                    }
                    list += itemToCollect.ToString() + " x" + count.ToString() + ", ";
                }
            }
            return list.Remove(list.Length - 2);//Remove final comma
        }
    }

    //Enum for objectives
    public enum ObjectiveTypes
    { 
        FindFood,
        ReturnHome,
        CollectItem,
        ObtainRelic
    }
}
