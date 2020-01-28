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

        [SerializeField] Player player;

        //private float _score = 0;
        private void Start()
        {
            subscribeAllEvents();
        }

        private void LifeCollectible_OnCollected(int value)
        {
            player.AddLife(value);
        }

        private void KillZone_OnCollision()
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
        private void lScoreCollectible_OnCollected(float score)
        {
            //Hud.Score = score; 
            // Hud.UpdateScore(); 
        }

        private void OnDestroy()
        {
            unsubscribeAllEvents();
        }

        #region Events subscribtions
        private void subscribeAllEvents()
        {
           /* foreach (LifeCollectible lifeCollectible in LifeCollectible.List)
            {
                lifeCollectible.Collected += OnLifeCollectible;
            }*/

            /*foreach (KillZone killzone in KillZone.List)
            {
                killzone.OnCollision += OnKillZone;
            }*/

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
                ScoreCollectible.List[i].OnCollected += lScoreCollectible_OnCollected; 
            }
        }
        #endregion

        #region Events unsubscriptions
        private void unsubscribeAllEvents()
        {
            /* foreach (var lifeCollectible in LifeCollectible.List)
             {
                 lifeCollectible.Collected -= OnLifeCollectible;
             }*/

            /* foreach (KillZone killzone in KillZone.List)
             {
                 killzone.OnCollision -= OnKillZone;
             }*/

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
                ScoreCollectible.List[i].OnCollected -= lScoreCollectible_OnCollected;
            }
        }
        #endregion

    }
}