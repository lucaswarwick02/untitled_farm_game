using System;
using ScriptableObjects.Items;
using Utility.Map;

namespace Data
{
    [Serializable]
    public class SaveData
    {
        public string guid;
        public string lastSaveTime;

        public int[,] island;

        public Inventory inventory;

        public SaveData(string guid, int[,] island)
        {
            this.guid = guid;
            inventory = new Inventory();
            this.island = island;
        }
    }
}