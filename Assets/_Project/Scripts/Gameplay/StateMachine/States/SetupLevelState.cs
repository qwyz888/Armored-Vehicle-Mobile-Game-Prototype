using Camera;
using Gameplay.Level;
using Gameplay.StateMachine.States.Core;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Cysharp.Threading.Tasks;

namespace Gameplay.StateMachine.States
{
    public class SetupLevelState : IGameplayState, IState
    {
        private readonly IStateMachine<IGameplayState> _stateMachine;
        private readonly ILogService _logService;
        private readonly LevelManager _levelManager;
        private readonly GameplayCameraController _gameplayCameraController;

        public SetupLevelState(IStateMachine<IGameplayState> stateMachine, ILogService logService, LevelManager levelManager, GameplayCameraController gameplayCameraController)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _levelManager = levelManager;
            _gameplayCameraController = gameplayCameraController;
        }

        public void Enter()
        {
            _logService.Log("Gameplay.LoadLevelState.Enter");

            _levelManager.StartLevel();

            if (_gameplayCameraController != null)
            {
                var vehicle = UnityEngine.Object.FindFirstObjectByType<Gameplay.Car.VehicleController>();
                if (vehicle != null)
                {
                    _gameplayCameraController.TransitionToFollow(vehicle.transform).Forget();
                }
            }

            //level loading here
            //use GameplayData from IPersistentDataModel

        }
    }
}