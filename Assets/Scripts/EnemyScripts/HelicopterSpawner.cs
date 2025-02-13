// using EnemyScripts;
// using UnityEngine;
//
// namespace EnemyScripts
// {
//     public class HelicopterSpawner : MonoBehaviour
//     {
//         public GameObject enemyDropperHelicopterPrefab; // Assign the EnemyDropperHelicopter prefab
//         public GameObject shootingHelicopterPrefab; // Assign the ShootingHelicopter prefab
//         public float spawnInterval = 5f; // Time between helicopter spawns
//         private float _spawnHeight;
//         public float spawnHeightLeft = 0.8f; // Y position as a percentage of the viewport height
//         public float spawnHeightRight = 0.9f;
//         private float _nextSpawnTime;
//
//         private void Start()
//         {
//             _nextSpawnTime = Time.time + spawnInterval;
//         }
//
//         private void Update()
//         {
//             if (Time.time >= _nextSpawnTime)
//             {
//                 SpawnHelicopter();
//                 _nextSpawnTime = Time.time + spawnInterval;
//             }
//         }
//
//         private void SpawnHelicopter()
//         {
//             bool spawnOnRight = Random.value > 0.5f;
//
//             _spawnHeight = spawnOnRight ? spawnHeightRight : spawnHeightLeft;
//             if (Camera.main)
//             {
//                 Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(
//                     spawnOnRight ? 1.1f : -0.1f, // Slightly outside the viewport
//                     _spawnHeight, // Spawn at 80% of the screen height
//                     0
//                 ));
//                 spawnPosition.z = 0; // Ensure Z is 0 for 2D
//
//                 // Randomly choose between enemy-dropping and shooting helicopters
//                 GameObject helicopterPrefab = Random.value > 0.5f ? enemyDropperHelicopterPrefab : shootingHelicopterPrefab;
//
//                 GameObject helicopter = Instantiate(helicopterPrefab, spawnPosition, Quaternion.identity);
//                 Helicopter helicopterScript = helicopter.GetComponent<Helicopter>();
//                 if (helicopterScript != null)
//                 {
//                     helicopterScript.SetDirection(!spawnOnRight);
//                 }
//             }
//         }
//     }
// }



using UnityEngine;

namespace EnemyScripts
{
    public class HelicopterSpawner : MonoBehaviour
    {
        public GameObject enemyDropperHelicopterPrefab; // EnemyDropperHelicopter prefab
        public GameObject shootingHelicopterPrefab; // ShootingHelicopter prefab

        public float spawnInterval = 5f; // Time between helicopter spawns
        public float spawnHeightLeft = 0.8f; // Y position for left side
        public float spawnHeightRight = 0.9f; // Y position for right side

        private float _nextSpawnTime;
        private int _spawnCount = 0;
        private int _waveIndex = 0;

        private readonly int[] _waveSpawnCounts = { 7, 4 }; // Number of helicopters per wave
        private readonly bool[] _waveIsDropper = { true, false }; // True = Dropper, False = Shooter

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
            // Determine which helicopter type to spawn in this wave
            bool isDropperWave = _waveIsDropper[_waveIndex];
            GameObject helicopterPrefab = isDropperWave ? enemyDropperHelicopterPrefab : shootingHelicopterPrefab;

            // Alternate spawn position (Left or Right)
            bool spawnOnRight = Random.value > 0.5f;
            float spawnHeight = spawnOnRight ? spawnHeightRight : spawnHeightLeft;

            if (Camera.main)
            {
                Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(
                    spawnOnRight ? 1.1f : -0.1f, // Slightly outside viewport
                    spawnHeight,
                    0
                ));
                spawnPosition.z = 0;

                GameObject helicopter = Instantiate(helicopterPrefab, spawnPosition, Quaternion.identity);
                Helicopter helicopterScript = helicopter.GetComponent<Helicopter>();
                if (helicopterScript != null)
                {
                    helicopterScript.SetDirection(!spawnOnRight);
                }
            }

            // Increment spawn count for this wave
            _spawnCount++;

            // If this wave is complete, switch to the next one
            if (_spawnCount >= _waveSpawnCounts[_waveIndex])
            {
                _spawnCount = 0;
                _waveIndex++;

                // If we reached the last wave, reset to loop the waves
                if (_waveIndex >= _waveSpawnCounts.Length)
                {
                    _waveIndex = 0;
                }
            }
        }
    }
}
