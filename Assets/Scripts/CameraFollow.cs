using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    private readonly float smoothSpeed = 0.125f;

    private void FixedUpdate()
    {
        var desiredPosition = target.position;
        desiredPosition.z = transform.position.z;
        
        var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        transform.position = smoothedPosition;
    }

    private void OnValidate()
    {
        var t = target.position;
        t.z = transform.position.z;
        transform.position = t;
    }
}