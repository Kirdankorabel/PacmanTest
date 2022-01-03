using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyScripts
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private List<Enemy> _enemiesPrefab;
        private List<Enemy> _enemies;
        private int _count = 0;

        public int ActiveEnemyCount => _count + 1;

        private void Awake()
        {
            _enemies = new List<Enemy>();
            foreach (var enemy in _enemiesPrefab)
                enemy.enabled = false;
        }

        private void Start()
        {
            GameController.Singletone.Player.playerCaught.AddListener(DeactivateEnemies);
            GameController.Singletone.Player.takeEnergizer.AddListener(SwitchRoles);
            GameController.Singletone.resumeLevel.AddListener(Respawn);
            GameController.Singletone.CoinsController.LaunchNextEnemy.AddListener(ActivateNextEnemy);
            GameController.Singletone.reset.AddListener(DestroyAll);
        }

        public void Spawn(List<Vector3> positions)
        {
            _enemies.Clear();
            _count = 0;
            var number = 0;

            foreach (var position in positions)
            {
                var enemy = Instantiate(_enemiesPrefab[number % _enemiesPrefab.Count], position, Quaternion.identity, this.transform);
                enemy.StartPosition = position;
                _enemies.Add(enemy);
                number++;
            }
            ActivateNextEnemy();
        }

        private void Respawn()
        {
            foreach (var enemy in _enemies)
            {
                enemy.gameObject.transform.position = enemy.StartPosition;
                enemy.gameObject.SetActive(true);
            }
        }

        private void DeactivateEnemies()
        {
            foreach (var enemy in _enemies)
                enemy.gameObject.SetActive(false);
        }

        private void SwitchRoles()
        {
            foreach (var enemy in _enemies)
                enemy.IsTarget = true;
            StartCoroutine(ReturnRolesRoutine());
        }

        private void ActivateNextEnemy()
        {
            if (_count < _enemies.Count)
            {
                _enemies[_count].enabled = true;
                _count++;
            }
            else
                foreach (var enemy in _enemies)
                    enemy.huntingMode = true;
        }

        private void DestroyAll()
        {
            if (_enemies != null)
                foreach (var enemy in _enemies)
                    Destroy(enemy.gameObject);
        }

        private IEnumerator ReturnRolesRoutine()
        {
            yield return new WaitForSeconds(4);

            foreach (var enemy in _enemies)
                enemy.IsTarget = false;
        }
    }
}
