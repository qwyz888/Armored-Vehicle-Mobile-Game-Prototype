using Camera;
using Gameplay.Level;
using Gameplay.StateMachine.States.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Cysharp.Threading.Tasks;
using Gameplay.Car;

namespace Gameplay.StateMachine.States
{
    public class SetupLevelState : IGameplayState, IState
    {
        private readonly IStateMachine<IGameplayState> _stateMachine;
        private readonly ILogService _logService;
        private readonly LevelManager _levelManager;
        private readonly GameplayCameraController _gameplayCameraController;
        private readonly VehicleController _vehicleController;

        public SetupLevelState(IStateMachine<IGameplayState> stateMachine, ILogService logService, LevelManager levelManager, GameplayCameraController gameplayCameraController, VehicleController vehicleController)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _levelManager = levelManager;
            _gameplayCameraController = gameplayCameraController;
            _vehicleController = vehicleController;
        }

        public void Enter()
        {
            _logService.Log("Gameplay.LoadLevelState.Enter");

            _levelManager.StartLevel();

            if (_gameplayCameraController != null)
            {
                if (_vehicleController != null)
                {
                    _gameplayCameraController.TransitionToFollow(_vehicleController.transform).Forget();
                }
            }

            //level loading here
            //use GameplayData from IPersistentDataModel

        }
    }
}