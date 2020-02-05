///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 22/01/2020 16:06
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Screens.Buttons;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Platformer.Screens {
	public class PauseMenu : AScreen {

        public delegate void PauseMenuEventHandler(PauseMenu pauseMenu);//delegate appelé quand on clique sur le bouton correspondant
        public PauseMenuEventHandler OnResumeClicked;
        public PauseMenuEventHandler OnRetryClicked;
        public PauseMenuEventHandler OnHomeClicked;

        private Button[] buttons;//tableau des boutons contenu dans le Menu Pause

        private Button resumeButton;
        private Button retryButton;
        private Button homeButton;

        [SerializeField] private string buttonResumeTag = "ResumeButton";
        [SerializeField] private string buttonRetryTag = "RetryButton";
        [SerializeField] private string buttonHomeTag = "HomeButton";//pas spécialement utile vu le if/else if/ else du Awake()

        private void Awake()
        {
            buttons = GetComponentsInChildren<Button>();//récupère tous les boutons du Menu Pause

            for (int i = 0; i < buttons.Length; i++)//Assigne les bonnes références de chaque boutons grâce à leurs tags
            {
                if (buttons[i].CompareTag(buttonResumeTag)) resumeButton = buttons[i];
                else if (buttons[i].CompareTag(buttonRetryTag)) retryButton = buttons[i];
                else homeButton = buttons[i];

                buttons[i].GetComponent<MenuButton>().OnMenuButtonClicked += PauseMenu_OnButtonClicked;

            }

        }

        private void PauseMenu_OnButtonClicked(Button sender)
        {
            if (sender.CompareTag(buttonResumeTag)) OnResumeClicked?.Invoke(this);
            else if (sender.CompareTag(buttonRetryTag)) OnRetryClicked?.Invoke(this);
            else OnHomeClicked?.Invoke(this);

            foreach (Button button in buttons)
            {
                button.GetComponent<MenuButton>().OnMenuButtonClicked -= PauseMenu_OnButtonClicked;
            }
        }


    }
}