using System;
using ScriptableObjects.Items;
using Utility;

namespace Data
{
    [Serializable]
    public class Inventory
    {
        public ItemInstance[] equipped = new ItemInstance[36];

        public event Action OnInventoryChanged;
        
        public Inventory()
        {
            for(var i = 0; i < equipped.Length; i++)
                equipped[i] = new ItemInstance();
            equipped[0] = new ItemInstance { itemID = "SEED_CARROTO", amount = 5};
        }

        public bool AddToInventory(ItemInstance itemInstance)
        {
            foreach (var t in equipped)
            {
                if (!t.itemID.Equals(itemInstance.itemID)) continue;
                t.amount += itemInstance.amount;
                
                OnInventoryChanged?.Invoke();
                return true;
            }
            
            for (var i = 0; i < equipped.Length; i++)
            {
                if (!string.IsNullOrEmpty(equipped[i].itemID)) continue;
                
                equipped[i] = itemInstance.DeepClone();
                OnInventoryChanged?.Invoke();
                return true;
            }

            return false;
        }
        
        public void UseItem(int index)
        {
            if (equipped[index].amount <= 0) return;
            
            equipped[index].amount--;

            if (equipped[index].amount <= 0)
                equipped[index] = new ItemInstance();
            
            OnInventoryChanged?.Invoke();
        }
    }
}