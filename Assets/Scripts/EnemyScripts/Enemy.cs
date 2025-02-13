using UnityEngine;

namespace EnemyScripts
{
    public class Enemy : MonoBehaviour
    {
        public GameObject parachute; 
        private Rigidbody2D _rigidbody;
        private float _velocity;
        private Camera _camera;

        private readonly float _initialVelocity = 5f;
        private readonly float _parachuteVelocity = 1f;

        private bool _parachuteDeactivated = false; 
        private bool _landed = false;

        private void Awake()
        {
            _velocity = _initialVelocity; 
            _camera = Camera.main;
            _rigidbody = GetComponent<Rigidbody2D>();

            if (parachute)
            {
                parachute.SetActive(false); 
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
            _velocity = _parachuteVelocity; 
            parachute.SetActive(true);
        }

        private void Land()
        {
            _rigidbody.velocity = Vector2.zero; 
            _landed = true;
            if (_parachuteDeactivated)
            {
                OnDeath();
            }

            else if (!_parachuteDeactivated || (parachute && parachute.activeInHierarchy))
            {
                EnemyTracker.Instance.AddEnemy(gameObject);
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





