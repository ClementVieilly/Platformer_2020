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

    public class LevelManager : MonoBehaviour {

        [SerializeField] private Player player;
        private TimeManager timeManager;

        private float score = 0;

        private float finalTimer = 0; //Temps du levelComplete
        private void Start()
        {
            SubscribeAllEvents();
            timeManager = GetComponent<TimeManager>();
            timeManager.StartTimer();
            StartCoroutine(InitHud());
        }

        IEnumerator InitHud()
        {
            while (Hud.Instance == null) yield return null;
            UpdateHud();
        }

        private void LifeCollectible_OnCollected(int value)
        {
            player.AddLife(value);
            if(Hud.Instance != null) Hud.Instance.Life = player.Life;
        }

        private void ScoreCollectible_OnCollected(float addScore)
        {
            score += addScore;
            if(Hud.Instance != null) Hud.Instance.Score = score;
        }

        private void KillZone_OnCollision()
        {
            if (player.LooseLife())
            {
                player.setPosition(CheckpointManager.Instance.LastCheckpointPos);
                Hud.Instance.Life = player.Life;
            }
            else 
            {
                player.setPosition(CheckpointManager.Instance.LastSuperCheckpointPos);
                CheckpointManager.Instance.ResetColliders();
            } 
        }

        private void DeadZone_OnCollision()
        {
            player.Die();
            Hud.Instance.Life = player.Life;
        }

        private void Player_OnDie()
        {
            finalTimer = timeManager.Timer;
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
            finalTimer = timeManager.Timer;
            timeManager.SetModeVoid();
            UnsubscribeAllEvents();

            if (UIManager.Instance != null) UIManager.Instance.CreateWinScreen();
            else Debug.LogError("Pas d'UImanager sur la scène");
            player.gameObject.SetActive(false);
        }

        private void Retry()
        {
            player.Reset();
            score = 0;
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
            Hud.Instance.Score = score;
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
                UIManager.Instance.OnRetry += Retry;
                UIManager.Instance.OnResume += Resume;
                UIManager.Instance.OnPause += PauseGame;
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
        }
        #endregion

    }
}