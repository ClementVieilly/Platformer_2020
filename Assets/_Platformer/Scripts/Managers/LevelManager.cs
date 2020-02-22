///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:37
///-----------------------------------------------------------------

using Cinemachine;
using Com.IsartDigital.Platformer.LevelObjects;
using Com.IsartDigital.Platformer.LevelObjects.Collectibles;
using Com.IsartDigital.Platformer.LevelObjects.InteractiveObstacles;
using Com.IsartDigital.Platformer.LevelObjects.Platforms;
using Com.IsartDigital.Platformer.Screens;
using System.Collections;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Managers
{
	public class LevelManager : MonoBehaviour
	{
		public delegate void LevelManagerEventHandler(LevelManager levelManager);

		[SerializeField] private Player player = null;
		private TimeManager timeManager = null;

		private int _levelNumber = 0;
		public int LevelNumber { get => _levelNumber; }

		private int _score = 0;
		public int Score { get => _score; }
		private float _completionTime = 0f;
		public float CompletionTime { get => _completionTime; }
		public int Lives { get => player.Life; }

		public event LevelManagerEventHandler OnWin;

        private void Start()
        {
            SubscribeAllEvents();
            timeManager = GetComponent<TimeManager>();
            timeManager.StartTimer();
            StartCoroutine(InitHud());
        }

		/// <summary>
		/// Set the level number
		/// </summary>
		public void SetNumber(int level)
		{
			_levelNumber = level;
		}

        private IEnumerator InitHud()
        {
            while (Hud.Instance == null) yield return null;
            UpdateHud();
        }

        private void LifeCollectible_OnCollected(int value)
        {
            player.AddLife(value);
            if(Hud.Instance != null) Hud.Instance.Life = player.Life;
        }

        private void ScoreCollectible_OnCollected(int addScore)
        {
            _score += addScore;
            if(Hud.Instance != null) Hud.Instance.Score = _score;
        }

        private void KillZone_OnCollision()
        {
            if (player.LooseLife())
            {
                if (CheckpointManager.Instance.LastCheckpointPos == Vector2.zero) player.setPosition(player.LastCheckpointPos);
                else player.setPosition(CheckpointManager.Instance.LastCheckpointPos);
                Hud.Instance.Life = player.Life;
            }
        }

        private void DeadZone_OnCollision()
        {
            player.Die();
            Hud.Instance.Life = player.Life;
        }

        private void Player_OnDie()
        {
            _completionTime = timeManager.Timer;
            timeManager.SetModeVoid();

            UIManager.Instance.CreateLoseScreen();
        }

        private void CheckpointManager_OnFinalCheckPointTriggered()
        {
            Win();
            CheckpointManager.OnFinalCheckPointTriggered -= CheckpointManager_OnFinalCheckPointTriggered;
        }

        private void Win()
        {
			_completionTime = timeManager.Timer;
            timeManager.SetModeVoid();

			OnWin?.Invoke(this);

            UnsubscribeAllEvents();

			if (UIManager.Instance != null) UIManager.Instance.CreateWinScreen();
            else Debug.LogError("Pas d'UImanager sur la scène");
            player.gameObject.SetActive(false);
        }

        private void Retry()
        {
            player.Reset();
            _score = 0;
            UpdateHud();

            timeManager.SetModeVoid();

            CheckpointManager.Instance.ResetColliders();

            LifeCollectible.ResetAll();
            ScoreCollectible.ResetAll();
            DestructiblePlatform.ResetAll();
            MobilePlatform.ResetAll();
            PlatformTrigger.ResetAll();
            TimedDoor.ResetAll();

            timeManager.StartTimer();
        }

        private void Resume()
        {
            player.SetModeResume();
            timeManager.SetModeTimer();
            DestructiblePlatform.ResumeAll();
            MobilePlatform.ResumeAll();
            TimedDoor.ResumeAll();
            SoundManager.Instance.ResumeAll();
        }

        private void PauseGame()
        {
            player.SetModePause();
            timeManager.SetModePause();
            DestructiblePlatform.PauseAll();
            MobilePlatform.PauseAll();
            TimedDoor.PauseAll();
            SoundManager.Instance.PauseAll();
        }

        private void UpdateHud()
        {
            Hud.Instance.Score = _score;
            Hud.Instance.Life = player.Life;
        }

        private void OnDestroy()
        {
            UnsubscribeAllEvents();
        }

        #region Events subscribtions
        private void SubscribeAllEvents()
        {
            for(int i = LifeCollectible.List.Count - 1; i >= 0; i--)
            {
                LifeCollectible.List[i].OnCollected += LifeCollectible_OnCollected; 
            }

            for(int i = KillZone.List.Count - 1; i >= 0; i--)
            {
                KillZone.List[i].OnCollision += KillZone_OnCollision; 
            }

            for(int i = DeadZone.List.Count - 1; i >= 0; i--)
            {
                DeadZone.List[i].OnCollision += DeadZone_OnCollision; 
            }

            for(int i = ScoreCollectible.List.Count - 1; i >= 0; i--)
            {
                ScoreCollectible.List[i].OnCollected += ScoreCollectible_OnCollected; 
            }


            CheckpointManager.OnFinalCheckPointTriggered += CheckpointManager_OnFinalCheckPointTriggered;
            player.OnDie += Player_OnDie;

            if (UIManager.Instance != null)
            {
				UIManager uiManager = UIManager.Instance;
				uiManager.OnRetry += Retry;
                uiManager.OnResume += Resume;
                uiManager.OnPause += PauseGame;
            }
        }
        #endregion

        #region Events unsubscriptions
        private void UnsubscribeAllEvents()
        {
            for(int i = LifeCollectible.List.Count - 1; i >= 0; i--)
            {
                LifeCollectible.List[i].OnCollected -= LifeCollectible_OnCollected;
            }

            for(int i = KillZone.List.Count - 1; i >= 0; i--)
            {
                KillZone.List[i].OnCollision -= KillZone_OnCollision;
            }

            for(int i = ScoreCollectible.List.Count - 1; i >= 0; i--)
            {
                ScoreCollectible.List[i].OnCollected -= ScoreCollectible_OnCollected;
            }

            for (int i = DeadZone.List.Count - 1; i >= 0; i--)
            {
                DeadZone.List[i].OnCollision -= DeadZone_OnCollision;
            }

            CheckpointManager.OnFinalCheckPointTriggered -= CheckpointManager_OnFinalCheckPointTriggered;
            player.OnDie -= Player_OnDie;

            if (UIManager.Instance != null) 
            {
                UIManager.Instance.OnRetry -= Retry;
                UIManager.Instance.OnResume -= Resume;
                UIManager.Instance.OnPause -= PauseGame;
            }

			OnWin = null;
        }
        #endregion
    }
}