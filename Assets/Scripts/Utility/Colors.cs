using System;
using Enumerations;
using Unity.VisualScripting;
using UnityEngine;

namespace Utility
{
    public static class Colors
    {
        public static Color FromRarity(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Common => new Color(0.83f, 0.83f, 0.83f),
                Rarity.Uncommon => new Color(0.36f, 1f, 0.36f),
                Rarity.Unique => new Color(0.36f, 0.44f, 1f),
                Rarity.Rare => new Color(0.92f, 0.51f, 1f),
                Rarity.Legendary => new Color(1f, 0.72f, 0.3f),
                _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
            };
        }
        
        public static string EndTag => "</color>";

        public static string RichTag(this Color color) => "<color=#" + color.ToHexString() + ">";
    }
}