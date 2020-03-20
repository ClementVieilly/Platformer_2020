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
        [SerializeField] SoundsSettings sounds = null;

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
            SoundManager.Instance.Play(sounds.Ui_Button);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(MenuButton_OnClick);
        }
    }
}