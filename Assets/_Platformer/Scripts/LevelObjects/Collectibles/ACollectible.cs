///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:39
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.Collectibles {
	abstract public class ACollectible : ACollisionableObject {

        abstract protected void EffectOfTheCollectible(); 
        
        protected override void EffectOnCollision()
        {
            EffectOfTheCollectible();
            Destroy(gameObject); 
        }
    }
}