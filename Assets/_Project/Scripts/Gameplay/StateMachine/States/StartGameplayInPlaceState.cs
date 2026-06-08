using Cysharp.Threading.Tasks;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Menu.StateMachine.States.Core;
using Gameplay.Level;
using Camera;
using UnityEngine;

namespace Menu.StateMachine.States
{
    public class StartGameplayInPlaceState : IMenuState, IState
    {
        private readonly ILogService _logService;
        private readonly LevelManager _levelManager;
        private readonly GameplayCameraController _gameplayCameraController;

        public StartGameplayInPlaceState(ILogService logService, LevelManager levelManager, GameplayCameraController gameplayCameraController)
        {
            _logService = logService;
            _levelManager = levelManager;
            _gameplayCameraController = gameplayCameraController;
        }

        public void Enter()
        {
            _logService.Log("Menu.StartGameplayInPlaceState.Enter");

            _levelManager.StartLevel();

            if (_gameplayCameraController != null)
            {
                var vehicle = Object.FindObjectOfType<Gameplay.Car.VehicleController>();
                if (vehicle != null)
                {
                    _gameplayCameraController.TransitionToFollow(vehicle.transform).Forget();
                }
            }
        }
    }
}
