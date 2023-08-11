using System;
using UnityEngine;

public class SeeThroughObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private static readonly Color NormalColor = new(1f, 1f, 1f, 1f);
    private static readonly Color SeeThroughColor = new(1f, 1f, 1f, 0.5f);
    
    private const float FadeSpeed = 8f;
    
    private bool _isPlayerInside;
    
    private void Update()
    {
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, _isPlayerInside ? SeeThroughColor : NormalColor, Time.deltaTime * FadeSpeed);
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _isPlayerInside = other.transform.position.y > transform.position.y;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        _isPlayerInside = false;
        spriteRenderer.color = NormalColor;
    }
}