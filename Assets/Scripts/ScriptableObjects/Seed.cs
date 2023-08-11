using ScriptableObjects.Items;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Seed", menuName = "Items/Seed", order = 2)]
    public class Seed : Item
    {
        [field: Header("Seed")]
        [field: SerializeField] public Vegetable Vegetable { get; private set; }
        [field: SerializeField] public int MinAmount { private set; get; }
        [field: SerializeField] public int MaxAmount { private set; get; }
        [field: SerializeField] public Sprite DeadImage { private set; get; }
        [field: SerializeField] public Sprite[] GrowthImages { private set; get; }
        [field: SerializeField] public float GrowthTime { private set; get; }
        [field: SerializeField] public float DeathTime { private set; get; }

        private void OnValidate()
        {
            if (DeathTime < GrowthTime) DeathTime = GrowthTime;
            
            if (MinAmount < 1) MinAmount = 1;
            if (MaxAmount < MinAmount) MaxAmount = MinAmount;
        }
    }
}