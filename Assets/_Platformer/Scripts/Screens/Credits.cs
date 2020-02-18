///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 27/01/2020 14:06
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Screens.Buttons;
using UnityEngine.UI;

namespace Com.IsartDigital.Platformer.Screens
{
    public class Credits : AScreen {

        public delegate void CreditsEventHandler(Credits credits);
        public CreditsEventHandler OnBackToTitleClicked;

        private Button homeButton;

        private void Awake()
        {
            homeButton = GetComponentInChildren<Button>();
            homeButton.GetComponent<MenuButton>().OnMenuButtonClicked += CreditsBackToTitle_Clicked;
        }

        private void CreditsBackToTitle_Clicked(Button sender)
        {
            OnBackToTitleClicked?.Invoke(this);
            homeButton.GetComponent<MenuButton>().OnMenuButtonClicked -= CreditsBackToTitle_Clicked;
        }

        public override void UnsubscribeEvents()
        {
            OnBackToTitleClicked = null;
        }
    }
}