using System.Collections;
using UnityEngine;

namespace EnemyScripts
{
    public class Enemy : MonoBehaviour
    {
        public bool isDead;
        public bool huntingMode;
        [SerializeField] private float _moveSpeed;
        [SerializeField] Vector3 _target;
        private Vector2Int _size;
        private PathFinder _pathFinder = new PathFinder();
        private Animator _animator;
        private Vector3 _direction;
        private Vector3 _startPosition;
        private SphereCollider _collider;
        private bool _isTarget;

        public bool IsTarget
        {
            get => _isTarget;
            set
            {
                _animator.SetBool("IsTarget", value);
                _isTarget = value;
            }
        }

        public Vector3 StartPosition
        {
            get => _startPosition;
            set
            {
                if (_startPosition == Vector3.zero)
                    _startPosition = value;
            }
        }

        private void Awake()
        {
            _animator = this.gameObject.GetComponent<Animator>();
        }

        private void Start()
        {
            _collider = this.gameObject.GetComponent<SphereCollider>();
            _size = GameController.Singletone.Size;
            ChoosedTarget();
        }

        private void Update()
        {
            ChoosedDirection();
            StartCoroutine(MoveCorutine());
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (IsTarget)
                {
                    _collider.isTrigger = true;
                    Die();
                }
            }
        }

        private void ChoosedDirection()
        {
            if (huntingMode && !IsTarget) _target = GameController.Singletone.Player.transform.position;
            var pos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
            if (Mathf.Abs((pos - transform.position).magnitude) < 0.1f)
            {
                if (Mathf.Abs((_target - transform.position).magnitude) < 0.2f)
                    ChoosedTarget();
                else
                {
                    var direction = _pathFinder.FindPath(transform.position, _target, GameController.Singletone.Map);
                    _direction = direction;
                }
            }
        }

        private void ChoosedTarget()
        {
            if (isDead)
                Resurrect();

            var targetCell = GameController.Singletone.Map[Random.Range(1, _size.x - 2), Random.Range(1, _size.y - 2)];

            if (targetCell.distance != 0)
                ChoosedTarget();
            else if (!huntingMode || IsTarget)
                _target = new Vector3(targetCell.cellPosition.x, targetCell.cellPosition.y, transform.position.z);
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.name == "Wall(Clone)")
                transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
        }
        private void Die()
        {
            _target = _startPosition;
            huntingMode = false;
            _animator.SetBool("IsDead", true);
            isDead = true;
        }

        private void Resurrect()
        {
            _animator.SetBool("IsDead", false);
            isDead = false;
            huntingMode = true;
            _collider.isTrigger = false;
        }

        private IEnumerator MoveCorutine()
        {
            transform.position = transform.position + Time.deltaTime * _direction * _moveSpeed;
            yield return null;
        }
    }
}
