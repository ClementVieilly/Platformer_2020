///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 27/01/2020 15:41
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Managers;
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
        private int level;

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
            if (SoundManager.Instance) SoundManager.Instance.Play(sounds.Music_Menu);
        }

        private void Start()
        {
            if (SoundManager.Instance) SoundManager.Instance.Play(sounds.Music_Menu);
        }

        private void LevelSelector_OnButtonClicked(Button sender)
        {
            if(sender == level1Button)
            {
                level = 1;
                animator.SetTrigger(exit);
            }
            else if(sender == level2Button)
            {
                level = 2;
                animator.SetTrigger(exit);
            }
            else OnBackToTitleClicked?.Invoke(this, 0);

            for (int i = buttons.Length - 1; i >= 0; i--)
                buttons[i].GetComponent<MenuButton>().OnMenuButtonClicked -= LevelSelector_OnButtonClicked;
        }

        public void OnAnimEnd()
        {
            OnLevelClicked?.Invoke(this, level);
            if (SoundManager.Instance) SoundManager.Instance.Stop(sounds.Music_Menu);
        }

        public override void UnsubscribeEvents()
        {
            OnLevelClicked = null;
            OnBackToTitleClicked = null;
        }
    }
}