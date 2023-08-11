using System;

namespace ScriptableObjects.Items
{
    [Serializable]
    public class ItemInstance
    {
        public string itemID;
        public int amount;

        public ItemInstance()
        {
            itemID = "";
            amount = 0;
        }
    }
}