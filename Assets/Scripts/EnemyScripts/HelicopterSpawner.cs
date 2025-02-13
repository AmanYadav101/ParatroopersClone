using UnityEngine;

namespace EnemyScripts
{
    public class HelicopterSpawner : MonoBehaviour
    {
        public GameObject enemyDropperHelicopterPrefab; 
        public GameObject shootingHelicopterPrefab; 

        public float spawnInterval = 5f; 
        public float spawnHeightLeft = 0.8f; 
        public float spawnHeightRight = 0.9f; 

        private float _nextSpawnTime;
        private int _spawnCount = 0;
        private int _waveIndex = 0;

        private readonly int[] _waveSpawnCounts = { 7, 4 }; 
        private readonly bool[] _waveIsDropper = { true, false }; 

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
            bool isDropperWave = _waveIsDropper[_waveIndex];
            GameObject helicopterPrefab = isDropperWave ? enemyDropperHelicopterPrefab : shootingHelicopterPrefab;

            bool spawnOnRight = Random.value > 0.5f;
            float spawnHeight = spawnOnRight ? spawnHeightRight : spawnHeightLeft;

            if (Camera.main)
            {
                Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(
                    spawnOnRight ? 1.1f : -0.1f, 
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

            _spawnCount++;

            if (_spawnCount >= _waveSpawnCounts[_waveIndex])
            {
                _spawnCount = 0;
                _waveIndex++;

                if (_waveIndex >= _waveSpawnCounts.Length)
                {
                    _waveIndex = 0;
                }
            }
        }
    }
}
