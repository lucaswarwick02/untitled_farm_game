using Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Utility.Map
{
    public class MapBuilder : MonoBehaviour
    {
        [Header("Tilemaps")]
        [SerializeField] private Tilemap grassTilemap;
        [SerializeField] private Tilemap waterTilemap;
        
        [Header("Tiles")]
        [SerializeField] private TileBase grassTile;
        [SerializeField] private TileBase waterTile;

        private static int WaterSize => IslandGenerator.IslandSize / 2 + 10;
        
        private void Start()
        {
            for (var x = 0; x < IslandGenerator.IslandSize; x++)
            {
                for (var y = 0; y < IslandGenerator.IslandSize; y++)
                {
                    if (SaveManager.CurrentSave.island[x, y] == 1) grassTilemap.SetTile(IslandArrayToTilemap(x, y), grassTile);
                }
            }

            for (var x = -WaterSize; x < WaterSize; x++)
            {
                for (var y = -WaterSize; y < WaterSize; y++)
                {
                    waterTilemap.SetTile(new Vector3Int(x, y), waterTile);
                }
            }
        }

        private static Vector3Int IslandArrayToTilemap(int x, int y)
        {
            return new Vector3Int(x - IslandGenerator.IslandSize / 2, y - IslandGenerator.IslandSize / 2);
        }
    }
}