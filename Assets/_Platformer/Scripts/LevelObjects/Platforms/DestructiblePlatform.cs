///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:40
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.Platforms {
	public class DestructiblePlatform : Platform {

        private bool startCount = false;
        [SerializeField] private float duration;
        [SerializeField] private Collider2D triggeredCollider;
        private float elapsedTime;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            startCount = true;
            Debug.Log("je suis dessus"); 
        }

        private void Update()
        {
            if (startCount)
            {
                elapsedTime += Time.deltaTime;

                if(elapsedTime > duration) triggeredCollider.enabled = false;
            }
        }


    }
}