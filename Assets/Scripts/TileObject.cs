using UnityEngine;

[ExecuteInEditMode]
public class TileObject : MonoBehaviour
{
    private static readonly Vector3 Offset = new(0.5f, 0.5f);
    
    private static Vector3 WorldToTile(Vector3 worldPosition)
    {
        var tilePosition = new Vector3(
            Mathf.FloorToInt(worldPosition.x),
            Mathf.FloorToInt(worldPosition.y)
        );
        return tilePosition + Offset;
    }

    private void Update()
    {
        transform.position = WorldToTile(transform.position);
    }
}