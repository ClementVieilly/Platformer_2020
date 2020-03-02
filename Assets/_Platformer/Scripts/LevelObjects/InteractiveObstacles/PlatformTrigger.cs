///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 05/02/2020 10:45
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.InteractiveObstacles {
	public class PlatformTrigger : AInteractiveObstacles {

        private static List<PlatformTrigger> _list = new List<PlatformTrigger>();
        public static List<PlatformTrigger> List => _list;

        [SerializeField] private GameObject mobilePlatformGameObject = null;
        private MobilePlatform mobilePlatform = null;

        private void Awake()
        {
            _list.Add(this);
            mobilePlatform = mobilePlatformGameObject.GetComponent<MobilePlatform>();
        }

        protected override void TriggerInteraction()
        {
            mobilePlatform.SetModeNormal(); ;
        }

        private void OnDestroy()
        {
            _list.Remove(this);
        }

        public static void ResetAll()
        {
            for (int i = List.Count - 1; i >= 0; i--)
            {
                List[i].mobilePlatform.SetModeWait();
            }
        }

    }
}