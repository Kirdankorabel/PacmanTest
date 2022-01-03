using EnemyScripts;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerScripts
{
    public class Player : MonoBehaviour
    {
        public UnityEvent playerCaught;
        public UnityEvent takeCoin;
        public UnityEvent takeEnergizer;
        public UnityEvent killedEnemy;
        [SerializeField] private PlayerMover _playerMover;
        private Animator _animator;

        public PlayerMover PlayerMover => _playerMover;
        public Animator Animator => _animator;

        private void Awake()
        {
            _animator = this.gameObject.GetComponent<Animator>();
        }

        private void Start()
        {
            playerCaught.AddListener(Die);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                var enemy = collision.gameObject.GetComponent<Enemy>();
                if (!enemy.IsTarget)
                    playerCaught?.Invoke();
                else if (!enemy.isDead)
                    killedEnemy?.Invoke();
            }
            else if (collision.gameObject.name == "Coin(Clone)")
            {
                takeCoin?.Invoke();
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.name == "Energizer(Clone)")
            {
                takeEnergizer?.Invoke();
                Destroy(collision.gameObject);
            }
        }

        private void Die()
        {
            _animator.SetBool("IsDead", true);
            _playerMover.enabled = false;
        }
    }
}
