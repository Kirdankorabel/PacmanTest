using System.Collections.Generic;
using UnityEngine;

namespace MapScripts
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _wallPrefab;
        [SerializeField] private GameObject _coinPrefab;
        [SerializeField] private GameObject _energizerPrefab;
        [SerializeField] private GameObject _enemyWallPrefab;
        [SerializeField] private GameObject _itemsParentObject;
        [SerializeField] private GameObject _wallsParentObject;
        private List<Vector3> _spawnsCoordinates;
        private List<GameObject> _objects;
        private Cell[,] _map;
        private Vector2Int _size;
        private Cell _spawnCell;

        public List<Vector3> SpawnsCoordinates => _spawnsCoordinates;

        private void Start()
        {
            GameController.Singletone.reset.AddListener(DestroyAll);
        }

        public Cell[,] Generate(int enemiesNumber)
        {
            _size = GameController.Singletone.Size;
            _map = new Cell[_size.x, _size.y];
            CreateMap();
            CreateEnemySpawn(enemiesNumber, new Vector2Int(_size.x / 2, (_size.y - enemiesNumber) / 2));
            InstantiateMap();
            return _map;
        }

        private void InstantiateMap()
        {
            _objects = new List<GameObject>();
            for (var i = 0; i < StaticDate.Settings.EnemiesNumber; i++)
                InstantiateEnergizer();
            InstantiateMaze();
        }

        private void InstantiateMaze()
        {
            var coinsCounter = 0;
            foreach (var item in _map)
            {
                if (item.distance == 1 || item.distance == 2)
                {
                    _objects.Add(Instantiate(_wallPrefab,
                        transform.position + new Vector3(item.cellPosition.x, item.cellPosition.y, 0),
                        Quaternion.identity, _wallsParentObject.transform));
                }
                else if (item.distance == 0 && item.mayContainsItems)
                {
                    _objects.Add(Instantiate(_coinPrefab,
                        transform.position + new Vector3(item.cellPosition.x, item.cellPosition.y, 0),
                        Quaternion.identity, _itemsParentObject.transform));
                    coinsCounter++;
                }
            }
            _objects.Add(Instantiate(_enemyWallPrefab,
                        transform.position + new Vector3(_spawnCell.cellPosition.x, _spawnCell.cellPosition.y, 1.01f),
                        Quaternion.identity, _itemsParentObject.transform));
            GameController.Singletone.CoinsController.CoinsCount = coinsCounter;
        }

        private Cell[,] CreateMap()
        {
            for (int i = 0; i < _size.x; i++)
                for (int j = 0; j < _size.y; j++)
                {
                    _map[i, j] = new Cell();
                    _map[i, j].cellPosition = new Vector2Int(i, j);
                }
            MazeGenerator.CreateMaze(_map);

            return _map;
        }

        private void InstantiateEnergizer()
        {
            var cell = _map[Random.Range(1, _size.x - 2), Random.Range(1, _size.y - 2)];

            if (cell.distance != 0 || !cell.mayContainsItems)
                InstantiateEnergizer();
            else
            {
                cell.mayContainsItems = false;
                _objects.Add(Instantiate(_energizerPrefab, transform.position + new Vector3(cell.cellPosition.x, cell.cellPosition.y, 0),
                        Quaternion.identity, _wallsParentObject.transform));
            }
        }

        private void CreateEnemySpawn(int enemiesNamber, Vector2Int spawnPosition)
        {
            _spawnsCoordinates = new List<Vector3>();
            var number = 0;
            for (var i = spawnPosition.x - 2; i < spawnPosition.x + 2 + enemiesNamber; i++)
                for (var j = spawnPosition.y - 2; j <= spawnPosition.y + 2; j++)
                {
                    _map[i, j].mayContainsItems = false;
                    if (i >= spawnPosition.x || j >= spawnPosition.y - 1
                        || i == spawnPosition.x + enemiesNamber || j == spawnPosition.y + 1)
                        _map[i, j].distance = 2;

                    if (i == spawnPosition.x - 2 || j == spawnPosition.y - 2
                        || i == spawnPosition.x + 1 + enemiesNamber || j == spawnPosition.y + 2)
                        _map[i, j].distance = 0;

                    if (i >= spawnPosition.x && i < spawnPosition.x + enemiesNamber && j == spawnPosition.y)
                    {
                        _map[i, j].distance = 0;

                        _spawnsCoordinates.Add(new Vector3(i, j, transform.position.z));
                        number++;
                    }
                }

            _map[spawnPosition.x - 1, spawnPosition.y].distance = 0;
            _spawnCell = _map[spawnPosition.x - 1, spawnPosition.y];
        }

        private void DestroyAll()
        {
            if (_objects != null)
            {
                foreach (var go in _objects)
                    Destroy(go.gameObject);
                _objects.Clear();
            }
        }
    }
}
