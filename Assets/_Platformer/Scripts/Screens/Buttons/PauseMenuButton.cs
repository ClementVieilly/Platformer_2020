///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 22/01/2020 16:35
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Platformer.Screens.Buttons {
	public class PauseMenuButton : MonoBehaviour {

        public delegate void PauseMenuButtonEventHandler(Button button);
        public PauseMenuButtonEventHandler OnPauseMenuButtonClicked;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(PauseMenuButton_OnClick);
        }

        private void PauseMenuButton_OnClick()
        {
            OnPauseMenuButtonClicked?.Invoke(button);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(PauseMenuButton_OnClick);
        }
    }
}