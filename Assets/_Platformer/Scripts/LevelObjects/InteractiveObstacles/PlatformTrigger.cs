///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 05/02/2020 10:45
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.InteractiveObstacles {
	public class PlatformTrigger : AInteractiveObstacles {

        [SerializeField] private GameObject mobilePlatformGameObject;
        private MobilePlatform mobilePlatform;

        private void Awake()
        {
            mobilePlatform = mobilePlatformGameObject.GetComponent<MobilePlatform>();
        }

        protected override void TriggerInteraction()
        {
            mobilePlatform.IsStarted = true;
        }

    }
}