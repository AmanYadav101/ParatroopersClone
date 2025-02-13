using Unity.VisualScripting;
using UnityEngine;

namespace EnemyScripts
{
    public class EnemyDropperHelicopter : Helicopter
    {
        public GameObject enemyPrefab; 
        public Transform spawnPoint; 
        public float dropInterval = 2f; 

        private float _nextDropTime;


        void Start()
        {
            _nextDropTime = Time.time + Random.Range(dropInterval, dropInterval + 2f);
        }

        protected override void Update()
        {
            base.Update();
            DropEnemies();
        }

        private void DropEnemies()
        {
            if (Time.time >= _nextDropTime)
            {
                if (EnemyTracker.Instance.CanDropEnemy(true))
                {
                    if (CanSpawnEnemies())
                    {
                        var enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

                    }
                }

                _nextDropTime = Time.time + dropInterval;
            }
        }

        private bool IsAtBottomViewport(GameObject enemy)
        {
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(enemy.transform.position);
            return viewportPosition.y < 0.027f;
        }

        private bool CanSpawnEnemies()
        {
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(gameObject.transform.position);
            bool isInsideViewport = viewportPosition.x is > 0 and < 1;
            return (isInsideViewport) &&
                   (gameObject.transform.position.x < EnemyTracker.Instance.turret.position.x - 1f ||
                    gameObject.transform.position.x > EnemyTracker.Instance.turret.position.x + 1f);
        }
    }
}