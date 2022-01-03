using UnityEngine;
using MapScripts;

namespace EnemyScripts
{
    public class PathFinder
    {
        private Vector3 _position;
        private Cell[,] _map;
        private int _coordinateX;
        private int _coordinateY;

        public Vector3 FindPath(Vector3 position, Vector2 target, Cell[,] map)
        {
            _position = position;

            _map = new Cell[map.GetLength(0), map.GetLength(1)];

            foreach (var item in map)
                _map[item.cellPosition.x, item.cellPosition.y] = new Cell(item);

            _coordinateX = (int)Mathf.Round(_position.x);
            _coordinateY = (int)Mathf.Round(_position.y);

            MarkedPath(target);

            return GetDirection();
        }

        private void MarkedPath(Vector2 target)
        {
            var posX = (int)Mathf.Round(target.x);
            var posY = (int)Mathf.Round(target.y);

            _map[posX, posY].distance = -1;

            while (_map[_coordinateX, _coordinateY].distance > -1)
            {
                foreach (var cell in _map)
                    if (cell.distance == -1 && !cell.isMarked)
                        MarkedNeighborCells(_map, cell);
                foreach (var cell in _map)
                    if (cell.distance < 0 && !cell.isMarked)
                        cell.distance--;
                foreach (var cell in _map)
                    cell.isMarked = false;
            }
        }

        private void MarkedNeighborCells(Cell[,] _map, Cell cell)
        {
            if (_map[cell.cellPosition.x + 1, cell.cellPosition.y].distance == 0)
                _map[cell.cellPosition.x + 1, cell.cellPosition.y].Mark();
            if (_map[cell.cellPosition.x - 1, cell.cellPosition.y].distance == 0)
                _map[cell.cellPosition.x - 1, cell.cellPosition.y].Mark();
            if (_map[cell.cellPosition.x, cell.cellPosition.y + 1].distance == 0)
                _map[cell.cellPosition.x, cell.cellPosition.y + 1].Mark();
            if (_map[cell.cellPosition.x, cell.cellPosition.y - 1].distance == 0)
                _map[cell.cellPosition.x, cell.cellPosition.y - 1].Mark();
        }

        private Vector3 GetDirection()
        {
            if (_map[_coordinateX, _coordinateY].distance == -1)
            {
                foreach (var cell in _map)
                    if (cell.distance < 0) cell.distance++;

                if (_map[_coordinateX + 1, _coordinateY].distance == -1)
                    return Vector3.right;
                else if (_map[_coordinateX, _coordinateY + 1].distance == -1)
                    return Vector3.up;
                else if (_map[_coordinateX - 1, _coordinateY].distance == -1)
                    return Vector3.left;
                else if (_map[_coordinateX, _coordinateY - 1].distance == -1)
                    return Vector3.down;
            }
            return Vector3.down;
        }
    }
}
