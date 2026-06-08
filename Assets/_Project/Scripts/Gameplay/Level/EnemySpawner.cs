using Cysharp.Threading.Tasks;
using Gameplay.Enemy;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Level
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform vehicleTransform;
        [SerializeField] private GameObject enemyPrefab;

        [Header("Spawn Zone")]
        [SerializeField] private float spawnAheadDistance = 30f;
        [SerializeField] private float spawnZoneHalfWidth = 3f;

        private LevelConfig.LevelData _levelData;
        private bool _isRunning;
        private readonly List<GameObject> _activeEnemies = new();

        public void Initialize(LevelConfig.LevelData levelData)
        {
            _levelData = levelData;
        }

        public void StartSpawning()
        {
            if (_isRunning) return;
            _isRunning = true;
            SpawnLoop().Forget();
        }

        public void StopSpawning()
        {
            _isRunning = false;
        }

        private async UniTaskVoid SpawnLoop()
        {
            while (_isRunning)
            {
                try
                {
                    // clean dead
                    _activeEnemies.RemoveAll(e => e == null);

                    if (_activeEnemies.Count < _levelData.MaxConcurrentEnemies)
                    {
                        SpawnOne();
                    }

                    await UniTask.Delay(TimeSpan.FromSeconds(_levelData.EnemySpawnInterval));
                }
                catch (Exception)
                {
                    // swallow and continue
                }
            }
        }

        private void SpawnOne()
        {
            if (enemyPrefab == null || vehicleTransform == null || _levelData == null) return;

            float x = UnityEngine.Random.Range(-spawnZoneHalfWidth, spawnZoneHalfWidth);
            Vector3 spawnPos = vehicleTransform.position + vehicleTransform.forward * spawnAheadDistance + vehicleTransform.right * x;

            GameObject go = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            // set hp if EnemyHealth exists
            var health = go.GetComponent<EnemyHealth>();
            if (health != null)
            {
                float hp = UnityEngine.Random.Range(_levelData.EnemyHpMin, _levelData.EnemyHpMax);
                // reflection or internal field set: EnemyHealth has private maxHealth, so try to set via property if exists, else via a method (not present). We'll try to set current via SendMessage.
                go.SendMessage("SetMaxHealth", hp, SendMessageOptions.DontRequireReceiver);
            }

            _activeEnemies.Add(go);
        }
    }
}
