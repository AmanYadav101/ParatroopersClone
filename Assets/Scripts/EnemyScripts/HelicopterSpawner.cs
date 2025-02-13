using EnemyScripts;
using UnityEngine;

namespace EnemyScripts
{
    public class HelicopterSpawner : MonoBehaviour
    {
        public GameObject enemyDropperHelicopterPrefab; // Assign the EnemyDropperHelicopter prefab
        public GameObject shootingHelicopterPrefab; // Assign the ShootingHelicopter prefab
        public float spawnInterval = 5f; // Time between helicopter spawns
        private float spawnHeight;
        public float spawnHeightLeft = 0.8f; // Y position as a percentage of the viewport height
        public float spawnHeightRight = 0.9f;
        private float _nextSpawnTime;

        private void Start()
        {
            _nextSpawnTime = Time.time + spawnInterval;
        }

        private void Update()
        {
            if (Time.time >= _nextSpawnTime)
            {
                SpawnHelicopter();
                _nextSpawnTime = Time.time + spawnInterval;
            }
        }

        private void SpawnHelicopter()
        {
            bool spawnOnRight = Random.value > 0.5f;

            spawnHeight = spawnOnRight ? spawnHeightRight : spawnHeightLeft;
            Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(
                spawnOnRight ? 1.1f : -0.1f, // Slightly outside the viewport
                spawnHeight, // Spawn at 80% of the screen height
                0
            ));
            spawnPosition.z = 0; // Ensure Z is 0 for 2D

            // Randomly choose between enemy-dropping and shooting helicopters
            GameObject helicopterPrefab = Random.value > 0.5f ? enemyDropperHelicopterPrefab : shootingHelicopterPrefab;

            GameObject helicopter = Instantiate(helicopterPrefab, spawnPosition, Quaternion.identity);
            Helicopter helicopterScript = helicopter.GetComponent<Helicopter>();
            if (helicopterScript != null)
            {
                helicopterScript.SetDirection(!spawnOnRight);
            }
        }
    }
}