using Cysharp.Threading.Tasks;
using Gameplay.Car;
using System;
using UnityEngine;

namespace Gameplay.Level
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private LevelConfig levelConfig;
        [SerializeField] private int levelIndex = 0;

        [Header("References")]
        [SerializeField] private VehicleController vehicle;
        [SerializeField] private EnemySpawner spawner;

        private LevelConfig.LevelData _currentLevel;
        private float _startZ;
        private bool _isRunning;

        public void StartLevel()
        {
            if (levelConfig == null || vehicle == null || spawner == null) return;

            _currentLevel = levelConfig.Get(levelIndex);
            _startZ = vehicle.transform.position.z;
            _isRunning = true;

            vehicle.StartMoving();

            spawner.Initialize(_currentLevel);
            spawner.StartSpawning();

            MonitorLoop().Forget();
        }

        public void StopLevel()
        {
            if (!_isRunning) return;
            _isRunning = false;
            vehicle.StopMoving();
            spawner.StopSpawning();
            // TODO: signal UI result
        }

        private async UniTaskVoid MonitorLoop()
        {
            while (_isRunning)
            {
                float traveled = Math.Abs(vehicle.transform.position.z - _startZ);

                if (traveled >= _currentLevel.Length)
                {
                    StopLevel();
                    break;
                }

                await UniTask.NextFrame();
            }
        }
    }
}
