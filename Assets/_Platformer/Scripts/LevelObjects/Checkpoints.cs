///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 24/01/2020 11:35
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects {
	public class Checkpoints : ACollisionableObject {

        protected override void EffectOnCollision()
        {
            base.EffectOnCollision();
            if(collidedObject.CompareTag(playerTag))
            {
                collidedObject.GetComponent<Player>().LastCheckpointPos = transform.position;
                transform.GetComponent<BoxCollider2D>().enabled = false; 
            }
        }
    }
}