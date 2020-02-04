///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 22/01/2020 15:05
///-----------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using Com.IsartDigital.Platformer.Screens;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.IsartDigital.Platformer.Managers
{
    public class UIManager : MonoBehaviour
    {

        [SerializeField] private GameObject hudPrefab;
        [SerializeField] private GameObject pausePrefab;
        [SerializeField] private GameObject titleCardPrefab;
        [SerializeField] private GameObject creditPrefab;
        [SerializeField] private GameObject levelSelectorPrefab;

        [Header("Level names")]
        [SerializeField] private string menu;
        [SerializeField] private string level1;
        [SerializeField] private string level2;

        private Hud currentHud;//correspond au hud actuel utilisé (PC ou mobile)
        private PauseMenu currentPauseMenu;//correspond au menu pause actuel utilisé 
        private TitleCard currentTitleCard;//correspond au titlecard actuel
        private Credits currentCredits;//correspond à la page de crédits actuelle utilisée
        private LevelSelector currentLevelSelector;//correspond au levelSelector actuel utilisé

        private List<AScreen> allScreens = new List<AScreen>();

        private void Awake()
        {
            CreateTitleCard();
        }

        private void CreatePauseMenu() //Crée une instance de Menu Pause et écoute ses événements
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
            currentLevelSelector.OnLevel2Clicked += LevelSelector_OnLevel2ButtonClicked;
            currentLevelSelector.OnBackToTitleClicked += LevelSelector_OnBackToTitleClicked;

            allScreens.Add(currentLevelSelector);
        }

        private void CreateHud() //Crée une instance de Hud et écoute ses événements
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
                    currentLevelSelector.OnLevel2Clicked -= LevelSelector_OnLevel2ButtonClicked;
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
        private void ReturnToTitleCard()
        {
            CloseAllScreens();
            CreateTitleCard();
        }

        private void LoadLevel(string levelName)
        {
            CloseAllScreens();
            Debug.Log("load " + levelName);

            StartCoroutine(LoadAsyncToLevel(levelName, CreateHud));
        }

        IEnumerator LoadAsyncToLevel(string nextScene, Action action)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene,LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByName(nextScene));

            //Faire une nouvelle coroutine pour le déchargement de la scenbe precedente
            SceneManager.UnloadScene(currentScene);
            action();
        }

        //Evenements du TitleCard
        private void TitleCard_OnGameStart(TitleCard title)
        {
            CloseScreen(title);
            CreateLevelSelector();
        }
        private void TitleCard_OnSoundTriggerClicked(TitleCard title)
        {
            Debug.Log("active/désactive le son + change image");
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

        //Evenements de la page de crédits
        private void Credits_OnBackToTitleClicked(Credits credits)
        {
            ReturnToTitleCard();
        }

        //Evenements du LevelSelector
        private void LevelSelector_OnLevelButtonClicked(LevelSelector levelSelector)
        {
            LoadLevel(level1);
        }

        private void LevelSelector_OnLevel2ButtonClicked(LevelSelector levelSelector)
        {
            LoadLevel(level2);
        }

        private void LevelSelector_OnBackToTitleClicked(LevelSelector levelSelector)
        {
            ReturnToTitleCard();
        }

        //Evenements du HUD
        private void Hud_OnPauseButtonPressed(Hud hud) //Fonction callback de l'event de click sur le bouton pause du Hud
        {
            CreatePauseMenu();
        }

        //Evenements du Menu Pause
        private void PauseMenu_OnResumeClicked(PauseMenu pauseMenu)
        {
            CloseScreen(pauseMenu);
            Debug.Log("Resume Level");
        }
        private void PauseMenu_OnRetryClicked(PauseMenu pauseMenu)
        {
            CloseScreen(pauseMenu);
            Debug.Log("Retry level");
        }
        private void PauseMenu_OnHomeClicked(PauseMenu pauseMenu)
        {
            CloseAllScreens();
            StartCoroutine(LoadAsyncToLevel(menu, CreateTitleCard));
        }
    }
}