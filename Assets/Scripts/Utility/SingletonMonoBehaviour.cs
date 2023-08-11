using UnityEngine;

namespace Utility
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour
    {
        public static T Instance { protected set; get; }
    }
}