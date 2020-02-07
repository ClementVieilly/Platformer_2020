///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 27/01/2020 15:41
///-----------------------------------------------------------------

using System;
using Com.IsartDigital.Platformer.Screens.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Platformer.Screens {
	public class LevelSelector : AScreen {

        public delegate void LevelSelectorEventHandler(LevelSelector levelSelector);
        public LevelSelectorEventHandler OnLevel1Clicked;
        public LevelSelectorEventHandler OnLevel2Clicked;
        public LevelSelectorEventHandler OnBackToTitleClicked;

        private Button[] buttons;

        private Button level1Button;
        private Button level2Button;
        private Button backToTitleButton;

        [SerializeField] private string buttonLevel1Tag  = "Level1Button";
        [SerializeField] private string buttonLevel2Tag = "Level2Button";
        [SerializeField] private string buttonBackToTitleTag = "BackToTitleCard";//pas spécialement utile vu le if/else if/ else du Awake()


        private void Awake()
        {
            buttons = GetComponentsInChildren<Button>();

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
            if (sender == level1Button) OnLevel1Clicked?.Invoke(this);
            else if (sender == level2Button) OnLevel2Clicked?.Invoke(this);
            else OnBackToTitleClicked?.Invoke(this);

            foreach (Button button in buttons)
            {
                button.GetComponent<MenuButton>().OnMenuButtonClicked -= LevelSelector_OnButtonClicked;
            }
        }
    }
}