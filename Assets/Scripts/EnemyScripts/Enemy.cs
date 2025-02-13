using UnityEngine;

namespace EnemyScripts
{
    public class Enemy : MonoBehaviour
    {
        public GameObject parachute; // Reference to the parachute GameObject
        private Rigidbody2D _rigidbody;
        private float _velocity;
        private Camera _camera;

        private readonly float _initialVelocity = 5f; // Fast fall speed
        private readonly float _parachuteVelocity = 1f; // Slow fall speed

        private bool _parachuteDeactivated = false; // Track if parachute is deactivated mid-air
        private bool _landed = false;

        private void Awake()
        {
            _velocity = _initialVelocity; // Initialize with fast fall speed
            _camera = Camera.main;
            _rigidbody = GetComponent<Rigidbody2D>();

            if (parachute)
            {
                parachute.SetActive(false); // Ensure parachute is initially inactive
            }
        }

        private void Update()
        {
            if (_landed) return;
            _rigidbody.velocity = Vector2.down * _velocity;
            if (IsAtBottomViewport())
            {
                Land();
            }
            else if (parachute && !parachute.activeInHierarchy && Mathf.Approximately(_velocity, _parachuteVelocity) &&
                     !_parachuteDeactivated)
            {
                _velocity = _initialVelocity;
                _parachuteDeactivated = true;
            }
            else if (parachute && !parachute.activeInHierarchy && IsBelowHalfViewport() && !_parachuteDeactivated)
            {
                ActivateParachute();
            }
        }

        private void ActivateParachute()
        {
            _velocity = _parachuteVelocity; // Slow down fall
            parachute.SetActive(true);
        }

        private void Land()
        {
            _rigidbody.velocity = Vector2.zero; // Stop movement
            _landed = true;
            // Check if the parachute was active at any point during the fall
            if (_parachuteDeactivated)
            {
                // Parachute was never active, so destroy the enemy
                OnDeath();
            }

            else if (!_parachuteDeactivated || (parachute && parachute.activeInHierarchy))
            {
                parachute.SetActive(false);
            }
        }


        private bool IsBelowHalfViewport()
        {
            Vector3 viewportPosition = _camera.WorldToViewportPoint(transform.position);
            return viewportPosition.y < 0.5f;
        }

        private bool IsAtBottomViewport()
        {
            Vector3 viewportPosition = _camera.WorldToViewportPoint(transform.position);
            return viewportPosition.y < 0.027f;
        }

        public void OnDeath()
        {
            EnemyTracker.Instance.RemoveEnemy(gameObject);
            Destroy(gameObject);
        }
    }
}





