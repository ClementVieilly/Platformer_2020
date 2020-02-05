///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:37
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects;
using Com.IsartDigital.Platformer.LevelObjects.Collectibles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Managers {

    public class LevelManager : MonoBehaviour {

        [SerializeField] Player player = null;
        [SerializeField] Level currentLevel = null;

        private void Start()
        {
            subscribeAllEvents();
        }

        private void OnLifeCollectible(int value)
        {
            player.AddLife(value);
        }

        private void OnKillZone()
        {
            if (player.LooseLife())
            {
                player.setPosition(CheckpointManager.Instance.LastCheckpointPos);
            }
            else {
                player.setPosition(CheckpointManager.Instance.LastSuperCheckpointPos);
                CheckpointManager.Instance.ResetColliders();
            } 
        }

        private void OnWin()
        {
            UIManager.Instance.CreateWinScreen();
            player.gameObject.SetActive(false);
        }

        private void OnLose()
        {
            UIManager.Instance.CreateLoseScreen();
            player.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            unsubscribeAllEvents();
        }

        #region Events subscribtions
        private void subscribeAllEvents()
        {
            foreach (LifeCollectible lifeCollectible in LifeCollectible.List)
            {
                lifeCollectible.Collected += OnLifeCollectible;
            }

            foreach (KillZone killzone in KillZone.List)
            {
                killzone.OnCollision += OnKillZone;
            }

            //currentLevel.LvlWinFlag.OnCollision += OnWin;
            
            player.OnDie += OnLose;
        }


        private void unsubscribeAllEvents()
        {
            foreach (var lifeCollectible in LifeCollectible.List)
            {
                lifeCollectible.Collected -= OnLifeCollectible;
            }

            foreach (KillZone killzone in KillZone.List)
            {
                killzone.OnCollision -= OnKillZone;
            }
            //currentLevel.LvlWinFlag.OnCollision -= OnWin;
            player.OnDie -= OnLose;
        }
        #endregion

    }
}