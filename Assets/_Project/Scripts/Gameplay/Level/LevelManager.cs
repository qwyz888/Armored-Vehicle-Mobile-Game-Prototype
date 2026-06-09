using Cysharp.Threading.Tasks;
using Gameplay.Car;
using System;
using UI.Windows.Gameplay;
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

        private Infrastructure.Services.Window.Core.IWindowService _windowService;
        private Gameplay.Health.HealthComponent _vehicleHealth;
        private Infrastructure.StateMachine.Main.Core.IStateMachine<Infrastructure.StateMachine.Game.States.Core.IGameState> _gameStateMachine;

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

            _vehicleHealth = vehicle.GetComponent<Gameplay.Health.HealthComponent>();
            if (_vehicleHealth != null)
                _vehicleHealth.OnDeath += OnVehicleDeath;
        }

        [VContainer.Inject]
        public void Construct(Infrastructure.Services.Window.Core.IWindowService windowService, Infrastructure.StateMachine.Main.Core.IStateMachine<Infrastructure.StateMachine.Game.States.Core.IGameState> gameStateMachine)
        {
            _windowService = windowService;
            _gameStateMachine = gameStateMachine;
        }

        public void StopLevel()
        {
            if (!_isRunning) return;
            _isRunning = false;
            vehicle.StopMoving();
            spawner.StopSpawning();
        }

        private async UniTaskVoid MonitorLoop()
        {
            while (_isRunning)
            {
                float traveled = Math.Abs(vehicle.transform.position.z - _startZ);

                if (traveled >= _currentLevel.Length)
                {
                    StopLevel();
                    var win = await _windowService.CreateWindow(Infrastructure.Services.Window.Core.WindowID.LevelCompletedWindow);
                    if (win is LevelCompletedWindow lc)
                    {
                        lc.NextLevelAction = StartNextLevel;
                    }
                    await win.Show();
                    break;
                }

                await UniTask.NextFrame();
            }
        }

        private async void OnVehicleDeath()
        {
            StopLevel();
            if (_windowService != null)
            {
                var win = await _windowService.CreateWindow(Infrastructure.Services.Window.Core.WindowID.LevelFailedWindow);
                if (win is LevelFailedWindow lf) 
                {
                    lf.RestartAction = RestartCurrentLevel;
                }
                await win.Show();
            }
        }

        public void RestartCurrentLevel()
        {
            _gameStateMachine?.Enter<Infrastructure.StateMachine.Game.States.RestartState>();
        }

        public void StartNextLevel()
        {
            _gameStateMachine?.Enter<Infrastructure.StateMachine.Game.States.LoadNextLevelState>();
        }
    }
}
