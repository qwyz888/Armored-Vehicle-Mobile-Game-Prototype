using System;
using Infrastructure.Services.Log.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;

namespace Infrastructure.StateMachine.Game.States
{
    public class RestartState : IGameState, IState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly ILogService _logService;

        public RestartState(IStateMachine<IGameState> stateMachine, ILogService logService)
        {
            _stateMachine = stateMachine;
            _logService = logService;
        }

        public void Enter()
        {
            _logService.Log("Game.RestartState.Enter");
            var payload = new LoadSceneWithLoadingScreenState.Payload
            {
                SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            };

            _stateMachine.Enter<LoadSceneWithLoadingScreenState, LoadSceneWithLoadingScreenState.Payload>(payload);
        }
    }
}
