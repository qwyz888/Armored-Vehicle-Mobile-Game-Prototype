using Cysharp.Threading.Tasks;
using Gameplay.Car;
using Gameplay.Health;
using Infrastructure.Data.Models.Persistent.Core;
using Infrastructure.Services.Window.Core;
using Infrastructure.Services.SaveLoad.Core;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using System;
using UI.Windows.Gameplay;
using UnityEngine;
using VContainer;

namespace Gameplay.Level
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private LevelConfig levelConfig;

        [Header("References")]
        [SerializeField] private VehicleController vehicle;
        [SerializeField] private EnemySpawner spawner;

        private LevelConfig.LevelData _currentLevel;
        private float _startZ;
        private bool _isRunning;
        private int levelIndex = 0;

        private IWindowService _windowService;
        private HealthComponent _vehicleHealth;
        private IStateMachine<IGameState> _gameStateMachine;
        private IPersistentDataModel _persistent;
        private ISaveLoadService _saveLoadService;

        [Inject]
        public void Construct(IWindowService windowService,
         IStateMachine<IGameState> gameStateMachine,
          IPersistentDataModel persistent,
          ISaveLoadService saveLoadService)
        {
            _windowService = windowService;
            _gameStateMachine = gameStateMachine;
            _persistent = persistent;
            _saveLoadService = saveLoadService;
        }

        public void StartLevel()
        {
            if (levelConfig == null || vehicle == null || spawner == null) return;

            if (_persistent != null && _persistent.Data != null && _persistent.Data.GameplayData != null)
            {
                int saved = _persistent.Data.GameplayData.CurrentLevelIndex;
                levelIndex = saved;
            }

            _currentLevel = levelConfig.Get(levelIndex);
         
            EnsureInitProgressUI().Forget();
            _startZ = vehicle.transform.position.z;
            _isRunning = true;

            vehicle.StartMoving();

            spawner.Initialize(_currentLevel);
            spawner.StartSpawning();

            MonitorLoop().Forget();

            _vehicleHealth = vehicle.GetComponent<HealthComponent>();
            if (_vehicleHealth != null)
                _vehicleHealth.OnDeath += OnVehicleDeath;
        }

        private async UniTaskVoid EnsureInitProgressUI()
        {
            float deadline = Time.realtimeSinceStartup + 2f;
            LevelProgressUI progressUI = null;
            while (Time.realtimeSinceStartup < deadline)
            {
                progressUI = FindFirstObjectByType<LevelProgressUI>();
                if (progressUI != null) break;
                await UniTask.NextFrame();
            }

            if (progressUI != null)
            {
                Debug.Log($"[LevelManager] Initializing LevelProgressUI with index={levelIndex}");
                progressUI.Initialize(levelConfig, levelIndex, vehicle);
            }
            else
            {
                Debug.Log("[LevelManager] LevelProgressUI not found to initialize");
            }
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

                    try
                    {
                        if (_persistent != null && _persistent.Data != null && _persistent.Data.GameplayData != null && levelConfig != null && levelConfig.Levels != null && levelConfig.Levels.Length > 0)
                        {
                            int current = _persistent.Data.GameplayData.CurrentLevelIndex;
                            int next = (current + 1) % levelConfig.Levels.Length;
                            _persistent.Data.GameplayData.CurrentLevelIndex = next;
                            Debug.Log($"[LevelManager] Incremented persistent CurrentLevelIndex -> {next}");

                            if (_saveLoadService != null)
                            {
                                _saveLoadService.Save("Data", _persistent.Data);
                                Debug.Log("[LevelManager] Saved persistent data after level completion");
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"[LevelManager] Failed to persist next level index: {ex}");
                    }

                    var win = await _windowService.CreateWindow(WindowID.LevelCompletedWindow);
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
                var win = await _windowService.CreateWindow(WindowID.LevelFailedWindow);
                if (win is LevelFailedWindow lf) 
                {
                    lf.RestartAction = RestartCurrentLevel;
                }
                await win.Show();
            }
        }

        public void RestartCurrentLevel()
        {
            _gameStateMachine?.Enter<RestartState>();
        }

        public void StartNextLevel()
        {
            _gameStateMachine?.Enter<LoadNextLevelState>();
        }
    }
}
