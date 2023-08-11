using System;
using ScriptableObjects.Items;

namespace Data
{
    [Serializable]
    public class SaveData
    {
        public string guid;

        public Inventory inventory;

        public string lastSaveTime;

        public SaveData(string guid)
        {
            this.guid = guid;
            this.inventory = new Inventory();
        }
    }
}