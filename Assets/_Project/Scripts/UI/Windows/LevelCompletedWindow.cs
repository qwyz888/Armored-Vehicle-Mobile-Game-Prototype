using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Window.Core;
using Infrastructure.UI.Windows.Core;
using UnityEngine;
using UnityEngine.UI;


namespace UI.Windows.Gameplay
{
    public class LevelCompletedWindow : BaseNavigationalWindow, IWindow
    {
        [SerializeField] private Button _nextLevelButton;
        public Action NextLevelAction;

        private void Awake()
        {
            base.Awake();
            if (_nextLevelButton != null)
                _nextLevelButton.onClick.AddListener(OnNextLevelClicked);
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

        public void OnNextLevelClicked()
        {
            NextLevelAction?.Invoke();
            _ = Hide();
        }
    }
}
