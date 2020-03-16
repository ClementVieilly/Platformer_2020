///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 11/03/2020 15:35
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.Collectibles.ParticleSystems {
	public class CollectedCollectibleParticleSystem : MonoBehaviour {

        [SerializeField] private float durationBeforeDestruction;

        private void Start()
        {
            Destroy(gameObject, durationBeforeDestruction);
        }
    }
}