using UnityEngine;

/// <summary>
/// Handles how sprites are used in game
/// </summary>
public static class SpriteManager
{
    private static readonly Vector2 PivotCenter = new(0.5f, 0.5f);

    /// <summary>
    /// Centers the pivot of a sprite
    /// </summary>
    /// <param name="sprite"></param>
    /// <returns>Sprite with a pivot of 0.5, 0.5</returns>
    public static Sprite CenterSprite(Sprite sprite)
    {
        var texture = sprite.texture;
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), PivotCenter, 16);
    }

    /// <summary>
    /// Creates a Sprite from the texture with an outline. Computationally expensive.
    /// </summary>
    /// <param name="sprite">Original sprite</param>
    /// <param name="outlineColor">Color of the outline</param>
    /// <param name="keepPivot">Optional. Keeps the pivot in the same location</param>
    /// <returns>Sprite with outline around texture</returns>
    public static Sprite GenerateOutline(Sprite sprite, Color outlineColor, bool keepPivot = true)
    {
        var texture2D = GetSubTexture(sprite.texture, sprite.textureRect);

        var pivot = keepPivot ? CalculatePivot(sprite) : PivotCenter;
            
        // Since we are increasing the size of the texture by (2, 2) we need to offset the pivot by the save percentage
        if (keepPivot)
        {
            // 1. Find offset from center
            var centerOffset = pivot - Vector2.one * 0.5f;
            // 2. Calculate scalar
            var scalarWidth = texture2D.width / (texture2D.width + 2f);
            var scalarHeight = texture2D.height / (texture2D.height + 2f);
            // 3. Multiply center offset by scalars
            centerOffset.x *= scalarWidth;
            centerOffset.y *= scalarHeight;
            // 4. Move offset back to center
            pivot = centerOffset + Vector2.one * 0.5f;
        }

        // Create a copy of the original texture
        var outlineTexture = new Texture2D(texture2D.width + 2, texture2D.height + 2, TextureFormat.ARGB32, false)
        {
            filterMode = FilterMode.Point
        };
            
        // Set all pixels to be transparent
        // Copy the original sprite
        for (var x = 0; x < outlineTexture.width; x++)
        {
            for (var y = 0; y < outlineTexture.height; y++)
            {
                outlineTexture.SetPixel(x, y, Color.clear);
            }
        }
        outlineTexture.Apply();

        // Copy the original sprite and add outline
        for (var x = 0; x < outlineTexture.width; x++)
        {
            for (var y = 0; y < outlineTexture.height; y++)
            {
                // Get the corresponding pixel on the original texture
                var originalX = x - 1;
                var originalY = y - 1;
                    
                // Stop if out of range
                if (originalX < 0 || originalX >= texture2D.width) continue;
                if (originalY < 0 || originalY >= texture2D.height) continue;

                var targetColor = texture2D.GetPixel(originalX, originalY);
                    
                // If it has a found a non-transparent pixel, copy it
                if (targetColor.a <= 0f) continue;
                outlineTexture.SetPixel(x, y, targetColor);
                        
                // Set neighbouring pixels to an outline (if not a non-transparent pixel)
                var offsets = new[] { -1, 0, 1 };
                    
                foreach (var xOffset in offsets)
                {
                    foreach (var yOffset in offsets)
                    {
                        if (xOffset == 0 && yOffset == 0) continue;
                            
                        var neighbourX = originalX + xOffset;
                        var neighbourY = originalY + yOffset;
                    
                        if (neighbourX < 0 || neighbourX >= texture2D.width)
                        {
                            if (neighbourX + 1 < 0 || neighbourX + 1 >= outlineTexture.width) continue;
                            outlineTexture.SetPixel(neighbourX + 1, neighbourY + 1, outlineColor);
                        }
                    
                        if (neighbourY < 0 || neighbourY >= texture2D.height)
                        {
                            if (neighbourY + 1 < 0 || neighbourY + 1 >= outlineTexture.width) continue;
                            outlineTexture.SetPixel(neighbourX + 1, neighbourY + 1, outlineColor);
                        }
                    
                        var neighbourColor = texture2D.GetPixel(neighbourX, neighbourY);
                    
                        if (neighbourColor.a == 0f)
                        {
                            outlineTexture.SetPixel(neighbourX + 1, neighbourY + 1, outlineColor);
                        }
                    }
                }
            }
        }
        outlineTexture.Apply();
            
        return Sprite.Create(outlineTexture, new Rect(0, 0, outlineTexture.width, outlineTexture.height), pivot, 16);
    }
    
    private static Texture2D GetSubTexture(Texture2D texture, Rect rect)
    {
        var subTexture = new Texture2D((int)rect.width, (int)rect.height);
        var pixels = texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
        subTexture.SetPixels(pixels);
        subTexture.Apply();
        return subTexture;
    }

    /// <summary>
    /// Calculates the pivot of a sprite
    /// </summary>
    /// <param name="sprite">Sprite to calculate from</param>
    /// <returns>Calculated pivot value</returns>
    private static Vector2 CalculatePivot(Sprite sprite)
    {
        var bounds = sprite.bounds;
        var pivotX = -bounds.center.x / bounds.extents.x / 2 + 0.5f;
        var pivotY = -bounds.center.y / bounds.extents.y / 2 + 0.5f;
            
        return new Vector2(pivotX, pivotY);
    }
}