using System;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    [ExecuteInEditMode]
    public class ImageSpritePivot : MonoBehaviour
    {
        [SerializeField] private Image image;

        private void Update()
        {
            if (!image) return;
            
            var pivotX = image.sprite.pivot.x / image.sprite.rect.width;
            var pivotY = image.sprite.pivot.y / image.sprite.rect.height;

            var pivot = new Vector2(pivotX, pivotY);

            (transform as RectTransform)!.pivot = pivot;
        }
    }
}
