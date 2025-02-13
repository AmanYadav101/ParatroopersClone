using UnityEngine;

namespace EnemyScripts
{
    public abstract class Helicopter : MonoBehaviour
    {
        public float moveSpeed = 3f; // Speed of the helicopter
        protected bool _movingRight; // Direction of movement
        private Camera _camera;
        protected GameObject player;

        protected virtual void Awake()
        {
            _camera = Camera.main;
            player = GameObject.FindGameObjectWithTag("Player");

        }

        protected virtual void Update()
        {
            Move();
            DestroyIfOutOfBounds();
        }

        protected void Move()
        {
            Vector3 direction = _movingRight ? Vector3.right : Vector3.left;
            if (_movingRight)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }

            transform.Translate(direction * (moveSpeed * Time.deltaTime));
        }

        protected void DestroyIfOutOfBounds()
        {
            Vector3 viewportPosition = _camera.WorldToViewportPoint(transform.position);
            if (viewportPosition.x < -0.1f || viewportPosition.x > 1.1f)
            {
                Destroy(gameObject); // Destroy the helicopter if it goes out of bounds
            }
        }

        public void SetDirection(bool movingRight)
        {
            _movingRight = movingRight;
        }
    }
}