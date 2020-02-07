///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:37
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects;
using Com.IsartDigital.Platformer.LevelObjects.Collectibles;
using Com.IsartDigital.Platformer.Screens;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Managers {

    public class LevelManager : MonoBehaviour {

        [SerializeField] private Player player;
        private TimeManager timeManager;

        private float score = 0;

        private float finalTimer = 0; //Temps du levelComplete
        private void Start()
        {
            subscribeAllEvents();
            timeManager = GetComponent<TimeManager>();
            timeManager.StartTimer();
        }

        private void LifeCollectible_OnCollected(int value)
        {
            player.AddLife(value);
            Hud.Instance.Life = player.Life;
        }

        private void ScoreCollectible_OnCollected(float addScore)
        {
            score += addScore;
            Hud.Instance.Score = score;
        }

        private void KillZone_OnCollision()
        {
            if (player.LooseLife())
            {
                player.setPosition(CheckpointManager.Instance.LastCheckpointPos);
            }
            else 
            {
                player.setPosition(CheckpointManager.Instance.LastSuperCheckpointPos);
                CheckpointManager.Instance.ResetColliders();
            } 
        }

        private void OnDestroy()
        {
            unsubscribeAllEvents();
        }

        #region Events subscribtions
        private void subscribeAllEvents()
        {
            for(int i = LifeCollectible.List.Count - 1; i >= 0; i--)
            {
                LifeCollectible.List[i].OnCollected += LifeCollectible_OnCollected; 
            }

            for(int i = KillZone.List.Count - 1; i >= 0; i--)
            {
                KillZone.List[i].OnCollision += KillZone_OnCollision; 
            }

            for(int i = ScoreCollectible.List.Count - 1; i >= 0; i--)
            {
                ScoreCollectible.List[i].OnCollected += ScoreCollectible_OnCollected; 
            }

            CheckpointManager.OnFinalCheckPointTriggered += CheckpointManager_OnFinalCheckPointTriggered;
            player.OnDie += Player_OnDie; 
        }

        private void Player_OnDie()
        {
            finalTimer = timeManager.Timer; 
            timeManager.SetModeVoid();

            UIManager.Instance.CreateLoseScreen();
            player.gameObject.SetActive(false);
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
            unsubscribeAllEvents();

            UIManager.Instance.CreateWinScreen();
            player.gameObject.SetActive(false);
        }
        #endregion

        #region Events unsubscriptions
        private void unsubscribeAllEvents()
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
        }
        #endregion

    }
}