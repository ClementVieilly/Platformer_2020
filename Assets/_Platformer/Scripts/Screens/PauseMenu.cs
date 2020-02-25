///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 22/01/2020 16:06
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Screens.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Platformer.Screens
{
    public class PauseMenu : AScreen {

        public delegate void PauseMenuEventHandler(PauseMenu pauseMenu);//delegate appelé quand on clique sur le bouton correspondant
        public PauseMenuEventHandler OnResumeClicked;
        public PauseMenuEventHandler OnRetryClicked;
        public PauseMenuEventHandler OnHomeClicked;

        private Button[] buttons;//tableau des boutons contenu dans le Menu Pause

        [SerializeField] private string buttonResumeTag = "ResumeButton";
        [SerializeField] private string buttonRetryTag = "RetryButton";

        private void Awake()
        {
            buttons = GetComponentsInChildren<Button>();//récupère tous les boutons du Menu Pause

            for (int i = 0; i < buttons.Length; i++)// Abonne le menu de pause aux boutons
                buttons[i].GetComponent<MenuButton>().OnMenuButtonClicked += PauseMenu_OnButtonClicked;
        }

        private void PauseMenu_OnButtonClicked(Button sender)
        {
            if (sender.CompareTag(buttonResumeTag)) OnResumeClicked?.Invoke(this);
            else if (sender.CompareTag(buttonRetryTag)) OnRetryClicked?.Invoke(this);
            else OnHomeClicked?.Invoke(this);

            for (int i = buttons.Length - 1; i >= 0; i--)
                buttons[i].GetComponent<MenuButton>().OnMenuButtonClicked -= PauseMenu_OnButtonClicked;
        }

        public override void UnsubscribeEvents()
        {
            OnHomeClicked = null;
            OnResumeClicked = null;
            OnRetryClicked = null;
        }
    }
}