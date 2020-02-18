///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 22/01/2020 16:35
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Platformer.Screens.Buttons {
	public class MenuButton : MonoBehaviour {

        public delegate void MenuButtonEventHandler(Button button);
        public MenuButtonEventHandler OnMenuButtonClicked;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(MenuButton_OnClick);
        }

        private void MenuButton_OnClick()
        {
            OnMenuButtonClicked?.Invoke(button);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(MenuButton_OnClick);
        }
    }
}