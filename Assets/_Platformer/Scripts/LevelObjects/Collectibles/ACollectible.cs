///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:39
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.Collectibles {
	abstract public class ACollectible : ACollisionableObject {
        [SerializeField] protected GameObject GFX;
        abstract protected void EffectOfTheCollectible(); 
        
        protected override void EffectOnCollision()
        {
            EffectOfTheCollectible();
            //Destroy(gameObject); 
            //gameObject.SetActive(false);
            GFX.SetActive(false);
            GetComponent<Collider2D>().enabled = false;
        }

        protected void ResetObject()
        {
            GFX.SetActive(true);
            GetComponent<Collider2D>().enabled = true;
        }
        
    }
}