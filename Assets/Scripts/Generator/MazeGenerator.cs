using UnityEngine;

namespace MapScripts
{
    public static class MazeGenerator
    {
        public static void CreateMaze(Cell[,] map)
        {
            var sizeX = map.GetLength(0);
            var sizeY = map.GetLength(1);
            for (int i = 0; i < sizeX; i++)
                for (int j = 0; j < sizeY; j++)
                {
                    if (i == 0 || j == 0 || i == sizeX - 1 || j == sizeY - 1)
                        map[i, j].distance = 2;

                    else if (i % 2 == 0 && j % 2 == 0 && Random.value > 0.2f)
                    {
                        map[i, j].distance = 1;

                        int a = Random.value < 0.5f ? 0 : (Random.value < 0.5f ? -1 : 1);
                        int b = a != 0 ? 0 : (Random.value < 0.5f ? -1 : 1);
                        map[i + a, j + b].distance = 1;
                    }

                    if (map[i, j].distance != 1 && map[i, j].distance != 2)
                    {
                        map[i, j].distance = 0;
                        map[i, j].mayContainsItems = true;
                    }
                }
            FixMaze(map);
        }

        private static void FixMaze(Cell[,] map)
        {
            var sizeX = map.GetLength(0);
            var sizeY = map.GetLength(1);
            for (int i = 1; i < sizeX - 1; i++)
                for (int j = 1; j < sizeY - 1; j++)
                {
                    if (map[i, j].distance == 0
                        && map[i - 1, j].distance == 1 && map[i + 1, j].distance == 1
                        && map[i, j - 1].distance == 1 && map[i, j + 1].distance == 1)
                    {
                        map[i - 1, j].distance = 0;
                        map[i, j - 1].distance = 1;
                    }
                }
        }
    }
}
