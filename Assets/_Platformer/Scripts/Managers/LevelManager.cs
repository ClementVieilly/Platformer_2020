///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:37
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects;
using Com.IsartDigital.Platformer.LevelObjects.Collectibles;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Managers {


    public class LevelManager : MonoBehaviour {

        // private Vector2 _lastCheckpointPos; Envoyé par CheckPoint Manager 

        private void Start()
        {
            LifeCollectible.Collected += OnLifeCollectible;
        }

        private void OnLifeCollectible(Collider2D collider,int value)
        {
            collider.GetComponent<Player_LS>().AddLife(value);
        }

        private void OnDestroy()
        {
            LifeCollectible.Collected -= OnLifeCollectible;
        }


    }
}