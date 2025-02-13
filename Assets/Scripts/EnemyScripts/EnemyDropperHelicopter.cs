using Unity.VisualScripting;
using UnityEngine;

namespace EnemyScripts
{
    public class EnemyDropperHelicopter : Helicopter
    {
        public GameObject enemyPrefab; // Enemy prefab to drop
        public Transform spawnPoint; // Point where enemies are dropped
        public float dropInterval = 2f; // Time between enemy drops

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
                var isLeft = !_movingRight;
                if (EnemyTracker.Instance.CanDropEnemy(isLeft))
                {
                    var enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                    EnemyTracker.Instance.AddEnemy(enemy);
                }

                _nextDropTime = Time.time + dropInterval;
            }
        }
    }
}