using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using MapScripts;
using PlayerScripts;
using EnemyScripts;

public class GameController : MonoBehaviour
{
    public static GameController Singletone { get; set; }
    public UnityEvent reset;
    public UnityEvent gameOver;
    public UnityEvent resumeLevel;

    [SerializeField] private Vector2Int _size;
    [SerializeField] private EnemyController _enemyController;    
    [SerializeField] private CoinsController _coinsController;
    [SerializeField] private MapGenerator _generator;
    [SerializeField] private Player _player;
    [SerializeField] private Vector3 _playerStartPosition;

    private Cell[,] _map;
    private int _points;
    private int _healthCounter;

    public Vector2Int Size => _size;
    public Cell[,] Map => _map;
    public Player Player => _player;
    public EnemyController EnemyController => _enemyController;
    public CoinsController CoinsController => _coinsController;
    public int HealthCounter => _healthCounter;
    public int Points => _points;

    private void Awake()
    {
        _size = new Vector2Int(StaticDate.Settings.Heigth, StaticDate.Settings.Weidth);
        Singletone = this;
    }

    private void Start()
    {
        _player.takeCoin.AddListener(() => _points += 10);
        _player.takeEnergizer.AddListener(() => _points += 50);
        _player.killedEnemy.AddListener(() => _points += 200);
        _player.playerCaught.AddListener(LoseHealth);
        _coinsController.Wictory.AddListener(StartNewLevel);
        resumeLevel.AddListener(RespawnPlayer);
    }

    public void StartNewLevel()
    {
        _player.gameObject.SetActive(true);
        RespawnPlayer();
        _healthCounter = 3;
        reset?.Invoke();
        _map = _generator.Generate(StaticDate.Settings.EnemiesNumber);
        _enemyController.Spawn(_generator.SpawnsCoordinates);
        _coinsController.CalclounchEnemyPoints();
    }

    public void ResetAccount()       
        => _points = 0;    

    private void RespawnPlayer()
    {
        _player.Animator.SetBool("IsDead", false);
        _player.transform.position = _playerStartPosition;
        _player.enabled = true;
        _player.PlayerMover.enabled = true;
    }

    private void LoseHealth()
    {
        if (_healthCounter > 0)
        {
            _healthCounter--;
            StartCoroutine(TimerRoutine());
        }
        else
            gameOver?.Invoke();
    }

    private IEnumerator TimerRoutine()
    {
        yield return new WaitForSeconds(2);
        resumeLevel?.Invoke();
    }
}
