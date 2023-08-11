using UnityEngine;

/// <summary>
/// Component that allows for sprites to be outlined.
/// </summary>
public class OutlineSpriteRenderer : MonoBehaviour
{
    [SerializeField] private bool updateOnAwake = true;
        
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color outlineColor = Color.white;
    private Sprite _originalSprite;
    private Sprite _outlinedSprite;

    private void Awake()
    {
        if (updateOnAwake) UpdateSprites(spriteRenderer.sprite);
    }

    /// <summary>
    /// Updates the stored sprites to be outlined.
    /// </summary>
    /// <param name="sprite">Default sprite</param>
    /// <param name="color">Color of the outline</param>
    /// <param name="centerPivot">Whether or not to center the pivot</param>
    public void UpdateSprites(Sprite sprite, bool centerPivot = false)
    {
        _originalSprite = centerPivot ? SpriteManager.CenterSprite(sprite) : sprite;
        _outlinedSprite = SpriteManager.GenerateOutline(_originalSprite, outlineColor);
            
        DisableOutline();
    }
    
    public void UpdateSpriteUsingRenderer(bool centerPivot = false)
    {
        UpdateSprites(spriteRenderer.sprite, centerPivot);
    }

    /// <summary>
    /// Enables the outline.
    /// </summary>
    public void EnableOutline()
    {
        spriteRenderer.sprite = _outlinedSprite;
    }
        
    /// <summary>
    /// Disables the outline.
    /// </summary>
    public void DisableOutline()
    {
        spriteRenderer.sprite = _originalSprite;
    }
}