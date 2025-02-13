using EnemyScripts;
using UnityEngine;

namespace EnemyScripts
{
    public class ShootingHelicopter : Helicopter
    {
        public GameObject projectilePrefab; // Projectile prefab to shoot
        public Transform shootPoint; // Point where projectiles are fired from
        public float shootInterval = 2f; // Time between shots
        private float _projectileSpeed = 3f;
        private float _nextShootTime;


        void Start()
        {
            _nextShootTime = Time.time + Random.Range(shootInterval, shootInterval + 2f);
        }

        protected override void Update()
        {
            base.Update();
            ShootProjectiles();
        }

        private void ShootProjectiles()
        {
            if (Time.time >= _nextShootTime)
            {
                if (player)
                {
                    // Instantiate the projectile
                    GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

                    // Calculate the direction towards the player
                    Vector2 direction = (player.transform.position - shootPoint.position).normalized;

                    // Get the Rigidbody2D component of the projectile
                    Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

                    // Apply initial horizontal velocity towards the player
                    projectileRb.velocity = new Vector2(direction.x * _projectileSpeed, -_projectileSpeed);

                    // Set the next shoot time
                    _nextShootTime = Time.time + shootInterval;
                }
            }
        }
    }
}