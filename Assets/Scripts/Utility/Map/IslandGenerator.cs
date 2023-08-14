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
            var stopwatch = new System.Diagnostics.Stopwatch();

            // 1. Probability map
            stopwatch.Start();
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
            stopwatch.Stop();
            Debug.Log($"(1 / 6) Probability map: {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Reset();
            
            // 2. Alive Threshold
            stopwatch.Start();
            var aliveMap = new int[IslandSize, IslandSize];
            for (var x = 0; x < IslandSize; x++)
            {
                for (var y = 0; y < IslandSize; y++)
                {
                    if (Random.Range(0f, 1f) < probabilityMap[x, y]) aliveMap[x, y] = 1;
                }
            }
            stopwatch.Stop();
            Debug.Log($"(2 / 6) Alive Threshold: {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Reset();
            
            // 3. Blur
            stopwatch.Start();
            var blurMap = new float[IslandSize, IslandSize];
            for (var x = 0; x < IslandSize; x++)
            {
                for (var y = 0; y < IslandSize; y++)
                {
                    var neighbours = GetNeighbours(aliveMap, x, y, offset: 3);
                    blurMap[x, y] = neighbours.Sum() / (float) neighbours.Length;
                }
            }
            stopwatch.Stop();
            Debug.Log($"(3 / 6) Blur: {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Reset();
            
            // 4. Blur Threshold
            stopwatch.Start();
            var blurThresholdMap = new int[IslandSize, IslandSize];
            for (var x = 0; x < IslandSize; x++)
            {
                for (var y = 0; y < IslandSize; y++)
                {
                    if (blurMap[x, y] > 0.5f) blurThresholdMap[x, y] = 1;
                }
            }
            stopwatch.Stop();
            Debug.Log($"(4 / 6) Blur Threshold: {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Reset();

            // 5. Smooth
            stopwatch.Start();
            var smoothMap = blurThresholdMap.Clone() as int[,];
            for (var iteration = 0; iteration < 10; iteration++)
            {
                for (var x = 0; x < IslandSize; x++)
                {
                    for (var y = 0; y < IslandSize; y++)
                    {
                        var neighbours = GetDirectNeighbours(smoothMap, x, y);

                        if (smoothMap[x, y] == 1)
                        {
                            // Cell is alive, check if it should die
                            if (neighbours.Sum() < 2)
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
                            if (neighbours.Sum() > 2)
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
            stopwatch.Stop();
            Debug.Log($"(5 / 6) Smoothing x10: {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Reset();

            stopwatch.Start();
            var island = smoothMap.Clone() as int[,];
            for (var x = 1; x < IslandSize - 1; x++)
            {
                for (var y = 1; y < IslandSize - 1; y++)
                {
                    if (island[x, y] == 0) continue;

                    if (island[x - 1, y] == 0 && island[x + 1, y] == 0)
                    {
                        island[x, y] = 0;
                        continue;
                    }

                    if (island[x, y - 1] == 0 && island[x, y + 1] == 0)
                    {
                        island[x, y] = 0;
                        continue;
                    }
                }
            }
            stopwatch.Stop();
            Debug.Log($"(6 / 6) Bridge removing: {stopwatch.ElapsedMilliseconds}ms");

            return island;
        }

        private static int[] GetDirectNeighbours(int[,] grid, int x, int y)
        {
            var neighbours = new List<int>();
            
            if (x - 1 >= 0) neighbours.Add(grid[x - 1, y]);
            if (x + 1 < IslandSize) neighbours.Add(grid[x + 1, y]);
            if (y - 1 >= 0) neighbours.Add(grid[x, y - 1]);
            if (y + 1 < IslandSize) neighbours.Add(grid[x, y + 1]);

            return neighbours.ToArray();
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