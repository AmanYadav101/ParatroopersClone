using EnemyScripts;
using UnityEngine;

namespace EnemyScripts
{
    public class ShootingHelicopter : Helicopter
    {
        public GameObject projectilePrefab; 
        public Transform shootPoint; 
        public float shootInterval = 2f; 
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
                    GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

                    Vector2 direction = (player.transform.position - shootPoint.position).normalized;

                    Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

                    projectileRb.velocity = new Vector2(direction.x * _projectileSpeed, -_projectileSpeed);

                    _nextShootTime = Time.time + shootInterval;
                }
            }
        }
    }
}