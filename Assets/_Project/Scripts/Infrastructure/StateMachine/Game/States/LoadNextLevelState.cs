using System;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.Data.Models.Persistent.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class LoadNextLevelState : IGameState, IState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ILogService _logService;
        private readonly IPersistentDataModel _persistent;

        public LoadNextLevelState(IStateMachine<IGameState> stateMachine, ILogService logService, IPersistentDataModel persistent)
        {
            _stateMachine = stateMachine;
            _logService = logService;
            _persistent = persistent;
        }

        public void Enter()
        {
            _logService.Log("Game.LoadNextLevelState.Enter");

            // simply save current persistent data (already updated by LevelManager) and reload scene
            _stateMachine.Enter<SaveDataState, System.Action>(() =>
            {
                _stateMachine.Enter<LoadSceneWithLoadingScreenState, LoadSceneWithLoadingScreenState.Payload>(
                    new LoadSceneWithLoadingScreenState.Payload { SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name }
                );
            });
        }
    }
}
