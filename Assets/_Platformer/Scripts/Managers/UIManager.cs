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
        [Header("Tiles")]
        [SerializeField] private GameObject hudPrefab;
        [SerializeField] private GameObject pausePrefab;
        [SerializeField] private GameObject titleCardPrefab;
        [SerializeField] private GameObject creditPrefab;
        [SerializeField] private GameObject levelSelectorPrefab;
        [SerializeField] private GameObject loadingScreenPrefab;
        [SerializeField] private GameObject winScreenPrefab;
        [SerializeField] private GameObject loseScreenPrefab;
        [SerializeField] private GameObject loginScreenPrefab;
        [SerializeField] private GameObject leaderboardPrefab;
        [SerializeField] private GameObject confirmScreenPrefab;

        [Header("Level names")]
        [SerializeField] private string menu;
        [SerializeField] private string level1;
        [SerializeField] private string level2;

        private Hud currentHud;             //correspond au hud actuel utilisé (PC ou mobile)
        private PauseMenu currentPauseMenu; //correspond au menu pause actuel utilisé 
        private TitleCard currentTitleCard; //correspond au titlecard actuel
        private Credits currentCredits;     //correspond à la page de crédits actuelle utilisée
        private LevelSelector currentLevelSelector; //correspond au levelSelector actuel utilisé
        private WinScreen currentWinScreen; //correspond au winScreen actuel utilisé
        private LoseScreen currentLoseScreen; //correspond au winScreen actuel utilisé
        private LoginScreen currentLoginScreen; //correspond au loginScreen actuel utilisé
        private Leaderboard currentLeaderboard; //correspond au leaderboard actuel utilisé
        private ConfirmScreen currentConfirmScreen; //correspond au leaderboard actuel utilisé

        private List<AScreen> allScreens = new List<AScreen>();

        private static UIManager _instance;
        public static UIManager Instance => _instance;

        public delegate void UIManagerEventHandler();
        public UIManagerEventHandler OnRetry;

        private void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;

            CreateTitleCard();
            DontDestroyOnLoad(this.gameObject);
        }

        private void OnDestroy()
        {
            if (this == _instance) _instance = null;
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

        private GameObject CreateLoadingScreen()
        {
            return  Instantiate(loadingScreenPrefab);
        }

        public void CreateWinScreen()
        {
            currentWinScreen = Instantiate(winScreenPrefab).GetComponent<WinScreen>();

            currentWinScreen.OnMenuClicked += WinScreen_OnMenuClicked;
            currentWinScreen.OnLevelSelectorClicked += WinScreen_OnLevelSelectorClicked;

            allScreens.Add(currentWinScreen);
        }

        public void CreateLoseScreen()
        {
            currentLoseScreen = Instantiate(loseScreenPrefab).GetComponent<LoseScreen>();

            currentLoseScreen.OnRetryClicked += LoseScreen_OnRetryClicked;
            currentLoseScreen.OnLevelSelectorClicked += LoseScreen_OnLevelSelector;

            allScreens.Add(currentLoseScreen);
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
                else if (screen == currentWinScreen)
                {
                    currentWinScreen.UnsubscribeEvents();
                }
                else if (screen == currentLoseScreen) 
                {
                    currentLoseScreen.UnsubscribeEvents();
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
            StartCoroutine(LoadAsyncToNextScene(levelName, CreateHud));
        }

        //Coroutines de chargement asynchrone de scenes  
        #region Loading Coroutines
        IEnumerator LoadAsyncToNextScene(string nextScene, Action methodToLaunch)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene,LoadSceneMode.Additive);

            ////Creation ecran de chargement
            //LoadingScreen loader = CreateLoadingScreen().GetComponent<LoadingScreen>();

            while (!asyncLoad.isDone)
            {
                ////Barre de chargement
                //float progress = Mathf.Clamp01(asyncLoad.progress / .9f);
                //loader.LoadingBar.value = progress;
                //Debug.Log(progress);
                yield return null;
            }
            StartCoroutine(UnloadAsyncOfCurrentScene(currentScene, methodToLaunch));
        }

        IEnumerator UnloadAsyncOfCurrentScene(Scene scene, Action action)
        {
            Scene currentScene = scene;
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(currentScene);

            while (!asyncUnload.isDone)
            {
                yield return null;
            }
            action();
        }
        #endregion

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
            OnRetry?.Invoke();
            Debug.Log("Retry level");
        }
        private void PauseMenu_OnHomeClicked(PauseMenu pauseMenu)
        {
            CloseAllScreens();
            StartCoroutine(LoadAsyncToNextScene(menu, CreateTitleCard));
        }

        //Evenements du WinScreen
        private void WinScreen_OnMenuClicked(WinScreen winScreen)
        {
            CloseAllScreens();
            StartCoroutine(LoadAsyncToNextScene(menu, CreateTitleCard));
        }

        private void WinScreen_OnLevelSelectorClicked(WinScreen winScreen)
        {
            CloseAllScreens();
            StartCoroutine(LoadAsyncToNextScene(menu, CreateLevelSelector));
        }

        //Evenements du LoseScreen
        private void LoseScreen_OnRetryClicked(LoseScreen loseScreen)
        {
            Debug.Log("Retry from LoseScreen");
        }

        private void LoseScreen_OnLevelSelector(LoseScreen loseScreen)
        {
            CloseAllScreens();
            StartCoroutine(LoadAsyncToNextScene(menu, CreateLevelSelector));
        }
    }
}