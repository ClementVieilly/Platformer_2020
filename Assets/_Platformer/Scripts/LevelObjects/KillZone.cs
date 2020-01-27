///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 24/01/2020 10:42
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects {
    
    public class KillZone : ACollisionableObject {

        protected override void EffectOnCollision()
        {
            if(collidedObject.CompareTag(playerTag)) collidedObject.GetComponent<Player>().Die();
            base.EffectOnCollision();
        }
    }
}