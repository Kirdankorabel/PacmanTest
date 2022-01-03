using UnityEngine;

namespace MapScripts
{
    public class Cell
    {
        public Vector2Int cellPosition;
        public int distance = 0;
        public bool isMarked = false;
        public bool mayContainsItems = false;

        public void Mark()
        {
            if (isMarked) return;
            distance = -1;
            isMarked = true;
        }

        public Cell() { }

        public Cell(Cell cell)
        {
            cellPosition = cell.cellPosition;
            distance = cell.distance;
            isMarked = cell.isMarked;
            mayContainsItems = cell.mayContainsItems;
        }
    }
}
