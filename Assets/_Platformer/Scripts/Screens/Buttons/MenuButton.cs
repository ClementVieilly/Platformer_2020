///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 22/01/2020 16:35
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Managers;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Platformer.Screens.Buttons {
	public class MenuButton : MonoBehaviour {

        public delegate void MenuButtonEventHandler(Button button);
        public MenuButtonEventHandler OnMenuButtonClicked;
        [SerializeField] private SoundsSettings sounds = null;
        [SerializeField] private ButtonType type = ButtonType.NORMAL;

        private Button button;
		public Button Button { get => button; }

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(MenuButton_OnClick);
        }

        private void MenuButton_OnClick()
        {
            OnMenuButtonClicked?.Invoke(button);
            if (type == ButtonType.NORMAL) SoundManager.Instance.Play(sounds.Ui_Button);
            else if (type == ButtonType.PAUSE) SoundManager.Instance.Play(sounds.Ui_Pause);
            else if (type == ButtonType.START) SoundManager.Instance.Play(sounds.Ui_Start);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(MenuButton_OnClick);
        }
    }

    public enum ButtonType
    {
        NORMAL,
        PAUSE,
        START
    }
}