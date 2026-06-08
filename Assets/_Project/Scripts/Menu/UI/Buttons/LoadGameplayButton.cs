using Cysharp.Threading.Tasks;
using Infrastructure.Services.Window;
using Infrastructure.Services.Window.Core;
using Infrastructure.StateMachine.Main.Core;
using Menu.StateMachine.States;
using Menu.StateMachine.States.Core;
using UI.Common;
using VContainer;

namespace Menu.UI.Buttons
{
    public class LoadGameplayButton : BaseButton
    {
        private IStateMachine<IMenuState> _stateMachine;
        private IWindowService _windowService;

        [Inject]
        public void Construct(IStateMachine<IMenuState> stateMachine, IWindowService windowService)
        {
            _stateMachine = stateMachine;
            _windowService = windowService;
        }

        protected override void OnClick()
        {
            if (_windowService.TryFind(WindowID.MenuInitialWindow, out IWindow menuWindow))
                menuWindow.Hide().Forget();

            _stateMachine.Enter<StartGameplayInPlaceState>();
        }
    }
}