///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 22/01/2020 15:05
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Screens;
using Com.IsartDigital.Platformer.WebScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.IsartDigital.Platformer.Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("Tiles")]
        [SerializeField] private GameObject hudPrefab = null;
        [SerializeField] private GameObject pausePrefab = null;
        [SerializeField] private GameObject titleCardPrefab = null;
        [SerializeField] private GameObject creditPrefab = null;
        [SerializeField] private GameObject levelSelectorPrefab = null;
        [SerializeField] private GameObject loadingScreenPrefab = null;
        [SerializeField] private GameObject winScreenPrefab = null;
        [SerializeField] private GameObject loseScreenPrefab = null;
        [SerializeField] private GameObject loginScreenPrefab = null;
        [SerializeField] private GameObject titleLeaderboardPrefab = null;
        [SerializeField] private GameObject winLeaderboardPrefab = null;
        [SerializeField] private GameObject confirmScreenPrefab = null;
        [SerializeField] private GameObject preloadPrefab = null;

		[Header("Level names")]
		[SerializeField] private List<string> sceneNames = new List<string>();

        //Screens
        private Hud currentHud;             //correspond au hud actuel utilisé (PC ou mobile)
        private PauseMenu currentPauseMenu; //correspond au menu pause actuel utilisé 
        private TitleCard currentTitleCard; //correspond au titlecard actuel
        private Credits currentCredits;     //correspond à la page de crédits actuelle utilisée
        private LevelSelector currentLevelSelector; //correspond au levelSelector actuel utilisé
        private WinScreen currentWinScreen; //correspond au winScreen actuel utilisé
        private LoseScreen currentLoseScreen; //correspond au winScreen actuel utilisé
        private LoginScreen currentLoginScreen; //correspond au loginScreen actuel utilisé
        private Leaderboard currentLeaderboard; //correspond au leaderboard actuel utilisé
        private ConfirmScreen currentConfirmScreen; //correspond à l'écran de confirmation actuel utilisé
        private PreLoad currentPreload; 

        //List of all screens
        private List<AScreen> allScreens = new List<AScreen>();

        //Singleton
        private static UIManager _instance;
        public static UIManager Instance => _instance;

		private WebClient webClient = null;

		private bool isPreviousCoroutineEnded = false;

		//Events
		public delegate void UIManagerEventHandler();
        public event UIManagerEventHandler OnRetry = null;
        public event UIManagerEventHandler OnResume = null;
        public event UIManagerEventHandler OnPause = null;

		public delegate void UIManagerLevelManagerEventHandler(LevelManager levelManager);
        public event UIManagerLevelManagerEventHandler OnLevelLoaded = null;

		public delegate void UIManagerLeaderboardEventHandler(Leaderboard leaderboard);
        public event UIManagerLeaderboardEventHandler OnLeaderboardStart = null;
        public event UIManagerLeaderboardEventHandler OnLeaderBoardChangeLevel = null;

		private void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }
            else _instance = this;

            // CreateTitleCard();
            //Je lance PreLoad 
          

            CreatePreload(); 
            DontDestroyOnLoad(gameObject);
        }

        private void CreatePreload()
        {
            currentPreload = Instantiate(preloadPrefab).GetComponent<PreLoad>();
            currentPreload.OnLauchTitleCard += currentPreload_OnLaunchTitleCard;
            allScreens.Add(currentPreload);
            currentPreload.LauchLoadText(); 
        }

        private void currentPreload_OnLaunchTitleCard()
        {
            CloseScreen(currentPreload); 
            CreateTitleCard(); 
        }

        private void OnDestroy()
        {
            if (this == _instance) _instance = null;
        }

		public void SetWebClient(WebClient webClient)
		{
			this.webClient = webClient;
			this.webClient.OnFeedback += WebClient_OnFeedback;
		}

		private void WebClient_OnFeedback(string message)
		{
			if (!currentLoginScreen) return;

			currentLoginScreen.SendFeedback(message);
		}

		public void WebClient_OnLogged(WebClient webClient)
		{
			CloseScreen(currentLoginScreen);
			if (currentLeaderboard) currentLeaderboard.StartLeaderboard();
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

            currentLevelSelector.OnLevelClicked += LevelSelector_OnLevelButtonClicked;
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

		public void CreateTitleLeaderboard()
		{
			currentLeaderboard = Instantiate(titleLeaderboardPrefab).GetComponent<Leaderboard>();

			currentLeaderboard.OnStart += Leaderboard_OnStart;
			currentLeaderboard.OnMenuClicked += Leaderboard_OnBackToTitleClicked;
			currentLeaderboard.OnNextClicked += Leaderboard_OnNextClicked;
			currentLeaderboard.OnPreviousClicked += Leaderboard_OnPreviousClicked;

			allScreens.Add(currentLeaderboard);

			if (webClient.wantToLog)
				CreateLoginScreen();
			else
				currentLeaderboard.StartLeaderboard();
		}

		public void CreateWinLeaderboard()
		{
			currentLeaderboard = Instantiate(winLeaderboardPrefab).GetComponent<Leaderboard>();
			LevelManager levelManager = FindObjectOfType<LevelManager>();
			currentLeaderboard.LevelToDisplay = levelManager ? levelManager.LevelNumber : 1;

			currentLeaderboard.OnStart += Leaderboard_OnStart;
			currentLeaderboard.OnMenuClicked += Leaderboard_OnMenuClicked;
			currentLeaderboard.OnBackClicked += Leaderboard_OnBackClicked;
			currentLeaderboard.OnSkipClicked += Leaderboard_OnSkipClicked;

			allScreens.Add(currentLeaderboard);

			if (webClient.IsLogged)
				currentLeaderboard.StartLeaderboard();
		}

		public void CreateLoginScreen()
		{
			currentLoginScreen = Instantiate(loginScreenPrefab).GetComponent<LoginScreen>();

			currentLoginScreen.OnConnectClicked += LoginScreen_OnConnectClicked;
			currentLoginScreen.OnSkipClicked += LoginScreen_OnSkipClicked;

			allScreens.Add(currentLoginScreen);
		}

		public void CreateConfirmScreen()
		{
			currentConfirmScreen = Instantiate(confirmScreenPrefab).GetComponent<ConfirmScreen>();

			currentConfirmScreen.OnBackClicked += ConfirmScreen_OnBackClicked;
			currentConfirmScreen.OnSkipClicked += ConfirmScreen_OnSkipClicked;

			allScreens.Add(currentConfirmScreen);
		}

		private GameObject CreateLoadingScreen()
        {
            return Instantiate(loadingScreenPrefab);
        }

        public void CreateWinScreen()
        {
            currentWinScreen = Instantiate(winScreenPrefab).GetComponent<WinScreen>();

            currentWinScreen.OnMenuClicked += WinScreen_OnMenuClicked;
            currentWinScreen.OnLevelSelectorClicked += WinScreen_OnLevelSelectorClicked;
            currentWinScreen.OnLeaderboardClicked += WinScreen_OnLeaderboardClicked;

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
            if (screen == null) return;

            for (int i = allScreens.Count - 1; i >= 0; i--)
            {
                AScreen currentScreen = allScreens[i];
                if (screen == currentScreen)
                {
                    currentScreen.UnsubscribeEvents();
                    break;
                }
            }
            Destroy(screen.gameObject);
            allScreens.RemoveAt(allScreens.IndexOf(screen));
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

		#region Loading Coroutines
		//Coroutines de chargement asynchrone de scenes  
		private IEnumerator LoadLevelCoroutine(string levelName, int level)
		{
            CloseAllScreens();
            StartCoroutine(LoadAsyncToNextScene(levelName, CreateHud));

			while (!isPreviousCoroutineEnded)
				yield return null;

			LevelManager levelManager = FindObjectOfType<LevelManager>();
			levelManager.SetNumber(level);
			OnLevelLoaded?.Invoke(levelManager);
        }

        IEnumerator LoadAsyncToNextScene(string nextScene, Action methodToLaunch)
        {
			isPreviousCoroutineEnded = false;

            Scene currentScene = SceneManager.GetActiveScene();
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene,LoadSceneMode.Additive);

            ////Creation ecran de chargement
            LoadingScreen loader = CreateLoadingScreen().GetComponent<LoadingScreen>();

            while (!asyncLoad.isDone)
            {
                ////Barre de chargement
                float progress = Mathf.Clamp01(asyncLoad.progress / .9f);
                loader.LoadingBar.value = progress;
                //Debug.Log(progress);
                yield return null;
            }
            StartCoroutine(UnloadAsyncOfCurrentScene(currentScene, methodToLaunch));
            isPreviousCoroutineEnded = true;
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

			if (webClient.wantToLog)
				CreateLoginScreen();
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
            CloseScreen(title);
            CreateTitleLeaderboard();
		}

		private void TitleCard_OnCreditsClicked(TitleCard title)
        {
            CloseScreen(title);
            CreateCredits();
		}

		//Evenements du Leaderboard
		private void Leaderboard_OnStart(Leaderboard leaderboard)
		{
			OnLeaderboardStart?.Invoke(leaderboard);
		}

		private void Leaderboard_OnBackToTitleClicked(Leaderboard leaderboard)
		{
            ReturnToTitleCard();
		}

		private void Leaderboard_OnMenuClicked(Leaderboard leaderboard)
		{
			CloseAllScreens();
			StartCoroutine(LoadAsyncToNextScene(sceneNames[0], CreateTitleCard));
		}

		private void Leaderboard_OnBackClicked(Leaderboard leaderboard)
		{
			CloseScreen(currentLeaderboard);
		}

		private void Leaderboard_OnSkipClicked(Leaderboard leaderboard)
		{
			CloseAllScreens();
			StartCoroutine(LoadAsyncToNextScene(sceneNames[0], CreateLevelSelector));
		}

		private void Leaderboard_OnNextClicked(Leaderboard leaderboard)
		{
			if (++leaderboard.LevelToDisplay >= sceneNames.Count)
				leaderboard.LevelToDisplay = 1;
			
			OnLeaderBoardChangeLevel?.Invoke(leaderboard);
		}

		private void Leaderboard_OnPreviousClicked(Leaderboard leaderboard)
		{
			if (--leaderboard.LevelToDisplay <= 0)
				leaderboard.LevelToDisplay = sceneNames.Count - 1;

			OnLeaderBoardChangeLevel?.Invoke(leaderboard);
		}

		//Evenements du LoginScreen
		private void LoginScreen_OnConnectClicked(LoginScreen loginScreen)
		{
			if (!webClient.CanTryToLog) return;

			webClient.Credentials = new WebClient.WebCredentials(currentLoginScreen.Username, currentLoginScreen.Password);
			webClient.TryToLog();
		}

		private void LoginScreen_OnSkipClicked(LoginScreen loginScreen)
		{
			CreateConfirmScreen();
		}

		private void ConfirmScreen_OnSkipClicked(ConfirmScreen confirmScreen)
		{
			CloseScreen(currentConfirmScreen);
			CloseScreen(currentLoginScreen);

			webClient.wantToLog = false;
			if (currentLeaderboard) currentLeaderboard.StartLeaderboard();
		}

		private void ConfirmScreen_OnBackClicked(ConfirmScreen confirmScreenµ)
		{
			CloseScreen(currentConfirmScreen);
		}

		//Evenements de la page de crédits
		private void Credits_OnBackToTitleClicked(Credits credits)
        {
            ReturnToTitleCard();
        }

        //Evenements du LevelSelector
        private void LevelSelector_OnLevelButtonClicked(LevelSelector levelSelector, int level)
        {
            StartCoroutine(LoadLevelCoroutine(sceneNames[level], level));
        }

        private void LevelSelector_OnBackToTitleClicked(LevelSelector levelSelector, int level)
        {
            ReturnToTitleCard();
        }

        //Evenements du HUD
        private void Hud_OnPauseButtonPressed(Hud hud) //Fonction callback de l'event de click sur le bouton pause du Hud
        {
            CreatePauseMenu();
            OnPause?.Invoke();
        }

        //Evenements du Menu Pause
        private void PauseMenu_OnResumeClicked(PauseMenu pauseMenu)
        {
            CloseScreen(pauseMenu);
            OnResume?.Invoke();
        }
        private void PauseMenu_OnRetryClicked(PauseMenu pauseMenu)
        {
            CloseScreen(pauseMenu);
            OnRetry?.Invoke();
        }
        private void PauseMenu_OnHomeClicked(PauseMenu pauseMenu)
        {
            CloseAllScreens();
            StartCoroutine(LoadAsyncToNextScene(sceneNames[0], CreateTitleCard));
        }

        //Evenements du WinScreen
        private void WinScreen_OnMenuClicked(WinScreen winScreen)
        {
            CloseAllScreens();
            StartCoroutine(LoadAsyncToNextScene(sceneNames[0], CreateTitleCard));
        }

        private void WinScreen_OnLevelSelectorClicked(WinScreen winScreen)
        {
            CloseAllScreens();
            StartCoroutine(LoadAsyncToNextScene(sceneNames[0], CreateLevelSelector));
        }

		private void WinScreen_OnLeaderboardClicked(WinScreen winScreen)
		{
			CreateWinLeaderboard();
		}

		//Evenements du LoseScreen
		private void LoseScreen_OnRetryClicked(LoseScreen loseScreen)
        {
            CloseScreen(loseScreen);
            OnRetry?.Invoke();
        }

        private void LoseScreen_OnLevelSelector(LoseScreen loseScreen)
        {
            CloseAllScreens();
            StartCoroutine(LoadAsyncToNextScene(sceneNames[0], CreateLevelSelector));
        }
    }
}