///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 27/01/2020 12:10
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Managers;
using Com.IsartDigital.Platformer.Screens.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Platformer.Screens {
    public class TitleCard : AScreen {

        public delegate void TitleCardEventHandler(TitleCard title);//Delegates appel�s au clic sur les diff�rents boutons du TitleCard

        public TitleCardEventHandler OnLeaderBoardClicked;
        public TitleCardEventHandler OnSoundTriggerClicked;
        public TitleCardEventHandler OnLocalisationClicked;
        public TitleCardEventHandler OnCreditsClicked;
        public TitleCardEventHandler OnGameStart;

        public static event TitleCardEventHandler OnChangeLanguage; 

        private Button[] buttons;//Tableau des diff�rents boutons contenus dans le TitleCard

        //variables contenant les r�f�rences aux bons boutons (lisibilit� du code)
        private Button leaderBoardButton;
        private Button creditsButton;
        private Button playButton;

        [SerializeField] private string buttonLeaderBoardTag = "LeaderBoard";
        [SerializeField] private string buttonCreditsTag = "Credits";
        [SerializeField] private string buttonPlayTag = "PlayButton";

        [SerializeField] private Toggle localizationToggle = null; 
        [SerializeField] private Toggle soundToggle = null; 

        private void Awake()
        {
            animator = GetComponent<Animator>();
            animator.SetTrigger(enter); 

            soundToggle.isOn = false;
            soundToggle.onValueChanged.AddListener(delegate { OnSoundTriggerClicked?.Invoke(this); });

            localizationToggle.isOn = LocalizationManager.toggleBool; 
            localizationToggle.onValueChanged.AddListener(delegate { OnChangeLanguage?.Invoke(this); }); 

            buttons = GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)//Assigne les bonnes r�f�rences de chaque boutons gr�ce � leurs tags
            {
                if (buttons[i].CompareTag(buttonLeaderBoardTag)) leaderBoardButton = buttons[i];
                else if (buttons[i].CompareTag(buttonPlayTag)) playButton = buttons[i];
                else if (buttons[i].CompareTag(buttonCreditsTag)) creditsButton = buttons[i];

                buttons[i].GetComponent<MenuButton>().OnMenuButtonClicked += TitleCard_OnMenuButtonClicked;
            }
            if (SoundManager.Instance) SoundManager.Instance.Play(sounds.Music_Menu);
        }

        private void Start()
        {
            if (SoundManager.Instance) SoundManager.Instance.Play(sounds.Music_Menu);
        }

        private void TitleCard_OnMenuButtonClicked(Button sender)
        {
            if(sender.CompareTag(buttonLeaderBoardTag))
            {
                OnLeaderBoardClicked?.Invoke(this);
                /*for (int i = buttons.Length - 1; i >= 0; i--)
                {
                    Debug.Log("unsubscribe for leaderboard");
                    //button.GetComponent<MenuButton>().OnMenuButtonClicked -= TitleCard_OnMenuButtonClicked;
                }*/
            }
            else if(sender.CompareTag(buttonPlayTag)) animator.SetTrigger(exit);//OnGameStart?.Invoke(this);
            else
            {
                OnCreditsClicked?.Invoke(this);

                /*for (int i = buttons.Length - 1; i >= 0; i--)
                {
                    buttons[i].GetComponent<MenuButton>().OnMenuButtonClicked -= TitleCard_OnMenuButtonClicked;
                }*/
            }
        }

        public void OnAnimEnd()
        {
            OnGameStart?.Invoke(this);  
        }
        public override void UnsubscribeEvents()
        {
            localizationToggle.onValueChanged.RemoveListener(delegate { OnChangeLanguage?.Invoke(this); });
            soundToggle.onValueChanged.RemoveListener(delegate { OnSoundTriggerClicked?.Invoke(this); });
            LocalizationManager.toggleBool = localizationToggle.isOn; 
            LocalizationManager.currentFileName = LocalizationManager.Instance.FileName;
            OnCreditsClicked = null;
            OnLeaderBoardClicked = null;
            OnLocalisationClicked = null;
            OnSoundTriggerClicked = null;
        }
        
    }
}