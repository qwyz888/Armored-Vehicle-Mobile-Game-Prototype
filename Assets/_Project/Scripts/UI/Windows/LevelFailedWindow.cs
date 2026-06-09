using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Window.Core;
using Infrastructure.UI.Windows.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Gameplay
{
    public class LevelFailedWindow : BaseNavigationalWindow, IWindow
    {
        [SerializeField] private Button _resartGameButton;
        public Action RestartAction;

        private void Awake()
        {
            base.Awake();
            if (_resartGameButton != null)
                _resartGameButton.onClick.AddListener(OnRestartClicked);
        }

        public override UniTask Show()
        {
            gameObject.SetActive(true);
            ContentCanvasGroup.interactable = true;
            return UniTask.CompletedTask;
        }

        public override UniTask Hide()
        {
            Destroy(gameObject);
            return UniTask.CompletedTask;
        }

        public void OnRestartClicked()
        {
            RestartAction?.Invoke();
            _ = Hide();
        }
    }
}
