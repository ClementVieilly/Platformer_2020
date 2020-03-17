///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 27/01/2020 15:41
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Screens.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Platformer.Screens
{
    public class LevelSelector : AScreen {

        public delegate void LevelSelectorEventHandler(LevelSelector levelSelector, int level);
        public LevelSelectorEventHandler OnLevelClicked;
        public LevelSelectorEventHandler OnBackToTitleClicked;

        private Button[] buttons;

		[SerializeField] private Button level1Button;
		[SerializeField] private Button level2Button;
        [SerializeField] private Button backToTitleButton;

        [SerializeField] private string buttonLevel1Tag = "Level1Button";
        [SerializeField] private string buttonLevel2Tag = "Level2Button";

        private void Awake()
        {
            buttons = GetComponentsInChildren<Button>();
            animator = GetComponent<Animator>();
            animator.SetTrigger(enter); 
            for (int i = 0; i < buttons.Length; i++)//Assigne les bonnes références de chaque boutons grâce à leurs tags
            {
                if (buttons[i].CompareTag(buttonLevel1Tag)) level1Button = buttons[i];
                else if (buttons[i].CompareTag(buttonLevel2Tag)) level2Button = buttons[i];
                else backToTitleButton = buttons[i];

                buttons[i].GetComponent<MenuButton>().OnMenuButtonClicked += LevelSelector_OnButtonClicked;
            }
        }

        private void LevelSelector_OnButtonClicked(Button sender)
        {
            if (sender == level1Button) OnLevelClicked?.Invoke(this, 1);
            else if (sender == level2Button) OnLevelClicked?.Invoke(this, 2);
            else OnBackToTitleClicked?.Invoke(this, 0);

            for (int i = buttons.Length - 1; i >= 0; i--)
                buttons[i].GetComponent<MenuButton>().OnMenuButtonClicked -= LevelSelector_OnButtonClicked;
        }

        public override void UnsubscribeEvents()
        {
            OnLevelClicked = null;
            OnBackToTitleClicked = null;
        }
    }
}