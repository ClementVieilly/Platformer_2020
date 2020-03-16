///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 27/01/2020 12:10
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Managers;
using Com.IsartDigital.Platformer.Screens.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Platformer.Screens
{
    public class TitleCard : AScreen {

        public delegate void TitleCardEventHandler(TitleCard title);//Delegates appelés au clic sur les différents boutons du TitleCard

        public TitleCardEventHandler OnLeaderBoardClicked;
        public TitleCardEventHandler OnSoundTriggerClicked;
        public TitleCardEventHandler OnLocalisationClicked;
        public TitleCardEventHandler OnCreditsClicked;
        public TitleCardEventHandler OnGameStart;

        private Button[] buttons;//Tableau des différents boutons contenus dans le TitleCard

        //variables contenant les références aux bons boutons (lisibilité du code)
        private Button leaderBoardButton;
        private Button soundTriggerButton;
        private Button localisationButton;
        private Button creditsButton;
        private Button playButton;

        [SerializeField] private string buttonLeaderBoardTag = "LeaderBoard";
        [SerializeField] private string buttonSoundTriggerTag = "SoundTrigger";
        [SerializeField] private string buttonLocalisationTag = "Localisation";
        [SerializeField] private string buttonCreditsTag = "Credits";
        [SerializeField] private string buttonPlayTag = "PlayButton";

        [SerializeField] private Toggle localizationToggle = null; 

        private void Awake()
        {
          
            LocalizationManager.isToggleChanged = false; //Coupe l'appel de la méthode OnChangeLanguage lorsqu'on change le toggle par code
            localizationToggle.isOn = LocalizationManager.toggleBool;
            LocalizationManager.isToggleChanged = true;

            buttons = GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)//Assigne les bonnes références de chaque boutons grâce à leurs tags
            {
                if (buttons[i].CompareTag(buttonLeaderBoardTag)) leaderBoardButton = buttons[i];
                else if (buttons[i].CompareTag(buttonSoundTriggerTag)) soundTriggerButton = buttons[i];
                else if (buttons[i].CompareTag(buttonLocalisationTag)) localisationButton = buttons[i];
                else if (buttons[i].CompareTag(buttonPlayTag)) playButton = buttons[i];
                else if (buttons[i].CompareTag(buttonCreditsTag)) creditsButton = buttons[i];

                buttons[i].GetComponent<MenuButton>().OnMenuButtonClicked += TitleCard_OnMenuButtonClicked;
            }
        }
		
        private void TitleCard_OnMenuButtonClicked(Button sender)
        {
            if (sender.CompareTag(buttonLeaderBoardTag))
            {
                OnLeaderBoardClicked?.Invoke(this);
                /*for (int i = buttons.Length - 1; i >= 0; i--)
                {
                    Debug.Log("unsubscribe for leaderboard");
                    //button.GetComponent<MenuButton>().OnMenuButtonClicked -= TitleCard_OnMenuButtonClicked;
                }*/
            }
            else if (sender.CompareTag(buttonSoundTriggerTag)) OnSoundTriggerClicked?.Invoke(this);
            else if (sender.CompareTag(buttonLocalisationTag)) OnLocalisationClicked?.Invoke(this);
            else if (sender.CompareTag(buttonPlayTag)) OnGameStart?.Invoke(this);
            else
            {
                OnCreditsClicked?.Invoke(this);
				
                /*for (int i = buttons.Length - 1; i >= 0; i--)
                {
                    buttons[i].GetComponent<MenuButton>().OnMenuButtonClicked -= TitleCard_OnMenuButtonClicked;
                }*/
            }
        }

        public override void UnsubscribeEvents()
        {
            LocalizationManager.currentFileName = LocalizationManager.Instance.FileName;
            LocalizationManager.toggleBool = localizationToggle.isOn;
            OnCreditsClicked = null;
            OnLeaderBoardClicked = null;
            OnLocalisationClicked = null;
            OnSoundTriggerClicked = null;
        }

        
    }
}