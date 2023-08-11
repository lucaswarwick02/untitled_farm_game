using Enumerations;
using UnityEngine;

namespace ScriptableObjects.Items
{
    public abstract class Item : ScriptableObject
    {
        [field: SerializeField] public string ItemID { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Image { get; private set; }
        [field: SerializeField] public Rarity Rarity { get; private set; }
    }
}