///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 22/01/2020 15:05
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using Com.IsartDigital.Platformer.Screens;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Managers
{
    public class UIManager : MonoBehaviour
    {

        [SerializeField] private GameObject hudPrefab;
        [SerializeField] private GameObject pausePrefab;
        [SerializeField] private GameObject titleCardPrefab;
        [SerializeField] private GameObject creditPrefab;
        [SerializeField] private GameObject levelSelectorPrefab;

        private Hud currentHud;//correspond au hud actuel utilis� (PC ou mobile)
        private PauseMenu currentPauseMenu;//correspond au menu pause actuel utilis� 
        private TitleCard currentTitleCard;//correspond au titlecard actuel
        private Credits currentCredits;//correspond � la page de cr�dits actuelle utilis�e
        private LevelSelector currentLevelSelector;//correspond au levelSelector actuel utilis�

        private List<AScreen> allScreens = new List<AScreen>();

        private void Awake()
        {
            CreateTitleCard();
        }

        private void CreatePauseMenu() //Cr�e une instance de Menu Pause et �coute ses �v�nements
        {
            currentPauseMenu = Instantiate(pausePrefab).GetComponent<PauseMenu>();
            currentPauseMenu.OnResumeClicked += PauseMenu_OnResumeClicked;
            currentPauseMenu.OnRetryClicked += PauseMenu_OnRetryClicked;
            currentPauseMenu.OnHomeClicked += PauseMenu_OnHomeClicked;
            allScreens.Add(currentPauseMenu);

        }

        private void CreateLevelSelector()
        {
            currentLevelSelector = Instantiate(levelSelectorPrefab).GetComponent<LevelSelector>();
            currentLevelSelector.OnLevel1Clicked += LevelSelector_OnLevelButtonClicked;
            currentLevelSelector.OnLevel2Clicked += LevelSelector_OnLevelButtonClicked;
            currentLevelSelector.OnBackToTitleClicked += LevelSelector_OnBackToTitleClicked;
            allScreens.Add(currentLevelSelector);
        }

        private void CreateHud() //Cr�e une instance de Hud et �coute ses �v�nements
        {
            currentHud = Instantiate(hudPrefab).GetComponent<Hud>();
            currentHud.OnButtonPausePressed += Hud_OnPauseButtonPressed;
            allScreens.Add(currentHud);

        }
        private void CreateTitleCard()
        {
            currentTitleCard = Instantiate(titleCardPrefab).GetComponent<TitleCard>();
            currentTitleCard.OnCreditsClicked += TitleCard_OnCreditsClicked;
            currentTitleCard.OnLeaderBoardClicked += TitleCard_OnLeaderBoardClicked;
            currentTitleCard.OnLocalisationClicked += TitleCard_OnLocalisationClicked;
            currentTitleCard.OnSoundTriggerClicked += TitleCard_OnSoundTriggerClicked;
            currentTitleCard.OnGameStart += TitleCard_OnGameStart;
            allScreens.Add(currentTitleCard);

        }
        private void CreateCredits()
        {
            currentCredits = Instantiate(creditPrefab).GetComponent<Credits>();
            currentCredits.OnBackToTitleClicked += Credits_OnBackToTitleClicked;
            allScreens.Add(currentCredits);
        }

        private void CloseScreen(AScreen screen)
        {

            if (screen != null)
            {
                if (screen == currentHud)
                {
                    currentHud.OnButtonPausePressed -= Hud_OnPauseButtonPressed;
                }
                else if (screen == currentPauseMenu)
                {
                    currentPauseMenu.OnHomeClicked -= PauseMenu_OnHomeClicked;
                    currentPauseMenu.OnResumeClicked -= PauseMenu_OnResumeClicked;
                    currentPauseMenu.OnRetryClicked -= PauseMenu_OnRetryClicked;
                }
                else if (screen == currentTitleCard)
                {
                    currentTitleCard.OnCreditsClicked -= TitleCard_OnCreditsClicked;
                    currentTitleCard.OnLeaderBoardClicked -= TitleCard_OnLeaderBoardClicked;
                    currentTitleCard.OnLocalisationClicked -= TitleCard_OnLocalisationClicked;
                    currentTitleCard.OnSoundTriggerClicked -= TitleCard_OnSoundTriggerClicked;
                }
                else if (screen == currentCredits)
                {
                    currentCredits.OnBackToTitleClicked -= Credits_OnBackToTitleClicked;
                }
                else if (screen == currentLevelSelector)
                {
                    currentLevelSelector.OnLevel1Clicked -= LevelSelector_OnLevelButtonClicked;
                    currentLevelSelector.OnLevel2Clicked -= LevelSelector_OnLevelButtonClicked;
                    currentLevelSelector.OnBackToTitleClicked -= LevelSelector_OnBackToTitleClicked;
                }


                    Destroy(screen.gameObject);
                allScreens.RemoveAt(allScreens.IndexOf(screen));
            }
        }
        private void CloseAllScreens()
        {
            for (int i = allScreens.Count - 1; i > -1 ; i--)
            {
                CloseScreen(allScreens[i]);
            }
        }


        //Enements du TitleCard
        private void TitleCard_OnGameStart(TitleCard title)
        {
            CloseScreen(title);
            CreateLevelSelector();
        }
        private void TitleCard_OnSoundTriggerClicked(TitleCard title)
        {
            Debug.Log("active/d�sactive le son + change image");
        }

        private void TitleCard_OnLocalisationClicked(TitleCard title)
        {
            Debug.Log("change la langue");
        }

        private void TitleCard_OnLeaderBoardClicked(TitleCard title)
        {
            Debug.Log("affiche le leaderboard");
        }

        private void TitleCard_OnCreditsClicked(TitleCard title)
        {
            CloseScreen(title);
            CreateCredits();
        }

        //Evenements de la page de cr�dits
        private void Credits_OnBackToTitleClicked(Credits credits)
        {
            currentCredits.OnBackToTitleClicked -= Credits_OnBackToTitleClicked;
            CloseAllScreens();
            CreateTitleCard();

        }

        //Evenements du LevelSelector
        private void LevelSelector_OnLevelButtonClicked(LevelSelector levelSelector)
        {
            currentLevelSelector.OnLevel1Clicked -= LevelSelector_OnLevelButtonClicked;
            currentLevelSelector.OnLevel2Clicked -= LevelSelector_OnLevelButtonClicked;
            currentLevelSelector.OnBackToTitleClicked -= LevelSelector_OnBackToTitleClicked;
            CloseAllScreens();
            CreateHud();
        }
        
        private void LevelSelector_OnBackToTitleClicked(LevelSelector levelSelector)
        {
            currentLevelSelector.OnLevel1Clicked -= LevelSelector_OnLevelButtonClicked;
            currentLevelSelector.OnLevel2Clicked -= LevelSelector_OnLevelButtonClicked;
            currentLevelSelector.OnBackToTitleClicked -= LevelSelector_OnBackToTitleClicked;
            CloseAllScreens();
            CreateTitleCard();
        }



        //Evenements du HUD
        private void Hud_OnPauseButtonPressed(Hud hud) //Fonction callback de l'event de click sur le bouton pause du Hud
        {
            CreatePauseMenu();
        }

        //Evenements du Menu Pause
        private void PauseMenu_OnResumeClicked(PauseMenu pauseMenu)
        {
            currentPauseMenu.OnResumeClicked -= PauseMenu_OnResumeClicked;
            CloseScreen(pauseMenu);
        }
        private void PauseMenu_OnRetryClicked(PauseMenu pauseMenu)
        {
            currentPauseMenu.OnResumeClicked -= PauseMenu_OnRetryClicked;
            CloseScreen(pauseMenu);
        }
        private void PauseMenu_OnHomeClicked(PauseMenu pauseMenu)
        {
            currentPauseMenu.OnResumeClicked -= PauseMenu_OnHomeClicked;
            currentHud.OnButtonPausePressed -= Hud_OnPauseButtonPressed;
            CloseAllScreens();
            CreateTitleCard();

        }




    }
}