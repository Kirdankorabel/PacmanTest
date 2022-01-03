using System.Collections;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerMover : MonoBehaviour
    {
        private Vector3 _direction;
        [SerializeField] private float _moveSpeed;

        private void Start()
        {
            GameController.Singletone.CoinsController.Wictory.AddListener(() => ChangeDirection(Vector3.right));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
                ChangeDirection(Vector3.up);
            else if (Input.GetKeyDown(KeyCode.A))
                ChangeDirection(Vector3.left);
            else if (Input.GetKeyDown(KeyCode.D))
                ChangeDirection(Vector3.right);
            else if (Input.GetKeyDown(KeyCode.S))
                ChangeDirection(Vector3.down);

            StartCoroutine(MoveCorutine());
        }

        private void ChangeDirection(Vector3 vector)
        {
            var pos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
            RaycastHit hit;
            Physics.Raycast(pos, vector, out hit);
            if (hit.distance > 0.5f && Mathf.Abs((pos - transform.position).magnitude) < 0.4f)
            {
                _direction = vector;
                transform.position = pos;
                transform.rotation = Quaternion.FromToRotation(Vector3.left, _direction);
            }
        }

        private IEnumerator MoveCorutine()
        {
            transform.position = transform.position + Time.deltaTime * _direction * _moveSpeed;
            yield return null;
        }
    }
}
