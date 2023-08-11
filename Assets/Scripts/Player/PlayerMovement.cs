using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animation2D animation2D;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    
    public void Move(InputAction.CallbackContext context)
    {
        var velocity = context.ReadValue<Vector2>();

        var direction = (Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg) switch
        {
            <= 45 and >= -45 => Animation2DDirection.Right,
            <= 135 and >= 45 => Animation2DDirection.Up,
            >= 135 or <= -135 => Animation2DDirection.Left,
            <= -45 and >= -135 => Animation2DDirection.Down,
            _ => Animation2DDirection.Down
        };
        
        animation2D.SetDirection(direction);
        animation2D.SetAnimation("Walk");
        
        rb.velocity = velocity * speed;
    }

    public void MoveEnd(InputAction.CallbackContext context)
    {
        rb.velocity = Vector2.zero;
        animation2D.SetAnimation("Idle");
    }
}