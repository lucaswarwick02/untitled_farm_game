using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility.Map
{
    public static class IslandGenerator
    {
        public const int IslandSize = 300;
        
        public static int[,] GenerateIsland()
        {
            // 1. Probability map
            var probabilityMap = new float[IslandSize, IslandSize];
            for (var x = 0; x < IslandSize; x++)
            {
                for (var y = 0; y < IslandSize; y++)
                {
                    const float middle = IslandSize / 2f;
                    var distanceFromMiddle = (Mathf.Abs(middle - x) + Mathf.Abs(middle - y)) / 2f;
                    probabilityMap[x, y] = Mathf.Clamp01(1 - distanceFromMiddle / middle);
                }
            }
            
            // 2. Alive Threshold
            var aliveMap = new int[IslandSize, IslandSize];
            for (var x = 0; x < IslandSize; x++)
            {
                for (var y = 0; y < IslandSize; y++)
                {
                    if (Random.Range(0f, 1f) < probabilityMap[x, y]) aliveMap[x, y] = 1;
                }
            }
            
            // 3. Blur
            var blurMap = new float[IslandSize, IslandSize];
            for (var x = 0; x < IslandSize; x++)
            {
                for (var y = 0; y < IslandSize; y++)
                {
                    var neighbours = GetNeighbours(aliveMap, x, y, offset: 3);
                    blurMap[x, y] = neighbours.Sum() / (float) neighbours.Length;
                }
            }
            
            // 4. Blur Threshold
            var blurThresholdMap = new int[IslandSize, IslandSize];
            for (var x = 0; x < IslandSize; x++)
            {
                for (var y = 0; y < IslandSize; y++)
                {
                    if (blurMap[x, y] > 0.5f) blurThresholdMap[x, y] = 1;
                }
            }

            // 5. Smooth
            var smoothMap = new int[IslandSize, IslandSize];
            for (var iteration = 0; iteration < 5; iteration++)
            {
                for (var x = 0; x < IslandSize; x++)
                {
                    for (var y = 0; y < IslandSize; y++)
                    {
                        var neighbours = GetNeighbours(blurThresholdMap, x, y, offset: 1);

                        if (blurThresholdMap[x, y] == 1)
                        {
                            // Cell is alive, check if it should die
                            if (neighbours.Sum() < 3)
                            {
                                smoothMap[x, y] = 0;
                            }
                            else
                            {
                                smoothMap[x, y] = 1;
                            }
                        }
                        else
                        {
                            if (neighbours.Sum() > 4)
                            {
                                smoothMap[x, y] = 1;
                            }
                            else
                            {
                                smoothMap[x, y] = 0;
                            }
                        }
                    }
                }
            }

            var endTime = Time.timeAsDouble;

            return smoothMap;
        }
        
        private static int[] GetNeighbours(int[,] grid, int x, int y, int offset)
        {
            var neighbours = new List<int>();

            for (var n_x = x - offset; n_x < x + offset + 1; n_x++)
            {
                for (var n_y = y - offset; n_y < y + offset + 1; n_y++)
                {
                    if (n_x is >= 0 and < IslandSize && n_y is >= 0 and < IslandSize)
                    {
                        neighbours.Add(grid[n_x, n_y]);
                    }
                }
            }

            return neighbours.ToArray();
        }
    }
}