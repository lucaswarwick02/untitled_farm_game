using System.Collections.Generic;
using ScriptableObjects;
using ScriptableObjects.Items;
using UnityEngine;

namespace Databases
{
    public static class ItemDatabase
    {
        private static Dictionary<string, Item> Dictionary { get; } = new();

        public static Item Query(string key)
        {
            return Dictionary[key];
        }
        
        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad()
        {
            var items = Resources.LoadAll<Item>("Items");

            foreach (var item in items)
            {
                Dictionary.Add(item.ItemID, item);
            }
        }
    }
}