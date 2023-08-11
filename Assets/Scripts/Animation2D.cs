using System;
using UnityEngine;

public class Animation2D : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animation2DData[] animations;
    [SerializeField] private float framesPerSecond = 8;
    
    private string _currentAnimation;
    private Animation2DDirection _currentDirection;
    private int _currentFrame;

    private void Awake()
    {
        _currentAnimation = animations[0].name;
        _currentDirection = Animation2DDirection.Down;
        
        InvokeRepeating(nameof(UpdateSprite), 0f, 1f / framesPerSecond);
    }
    
    public void SetAnimation(string animationName)
    {
        if (_currentAnimation.Equals(animationName)) return;
        
        _currentAnimation = animationName;
        _currentFrame = 0;
    }

    public void SetDirection(Animation2DDirection direction)
    {
        if (_currentDirection == direction) return;
        
        _currentDirection = direction;
        _currentFrame = 0;
    }

    private void UpdateSprite()
    {
        var sprites = GetCurrentAnimation().GetSprites(_currentDirection);
        
        spriteRenderer.sprite = sprites[_currentFrame];

        _currentFrame++;
        if (_currentFrame >= sprites.Length)
        {
            _currentFrame = 0;
        }
    }

    private Animation2DData GetCurrentAnimation()
    {
        foreach (var animation in animations)
        {
            if (animation.name == _currentAnimation)
            {
                return animation;
            }
        }

        return animations[0];
    }
}

[Serializable]
public struct Animation2DData
{
    public string name;
    public Sprite[] up;
    public Sprite[] down;
    public Sprite[] left;
    public Sprite[] right;
    
    public Sprite[] GetSprites (Animation2DDirection direction)
    {
        return direction switch
        {
            Animation2DDirection.Up => up,
            Animation2DDirection.Down => down,
            Animation2DDirection.Left => left,
            Animation2DDirection.Right => right,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}

public enum Animation2DDirection
{
    Up,
    Down,
    Left,
    Right
}
