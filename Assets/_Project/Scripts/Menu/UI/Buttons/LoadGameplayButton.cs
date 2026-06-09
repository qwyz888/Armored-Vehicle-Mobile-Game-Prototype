using Cysharp.Threading.Tasks;
using Gameplay.StateMachine.States;
using Gameplay.StateMachine.States.Core;
using Infrastructure.Services.Window.Core;
using Infrastructure.StateMachine.Main.Core;
using UI.Common;
using UI.Windows.Menu;
using VContainer;

namespace Menu.UI.Buttons
{
    public class LoadGameplayButton : BaseButton
    {
        private IStateMachine<IGameplayState> _stateMachine;
        private IWindowService _windowService;

        [Inject]
        public void Construct(IStateMachine<IGameplayState> stateMachine, IWindowService windowService)
        {
            _stateMachine = stateMachine;
            _windowService = windowService;
        }

        protected override void OnClick()
        {
            if (_windowService.TryFind(WindowID.MenuInitialWindow, out IWindow menuWindow))
                ((MenuInitialWindow)menuWindow).SetActiveBottomButtons(false);

            _stateMachine.Enter<SetupLevelState>();
        }
    }
}