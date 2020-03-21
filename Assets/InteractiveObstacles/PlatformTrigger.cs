///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 05/02/2020 10:45
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.InteractiveObstacles {
	public class PlatformTrigger : AInteractiveObstacles {

        private static List<PlatformTrigger> _list = new List<PlatformTrigger>();
        public static List<PlatformTrigger> List => _list;

        [SerializeField] private GameObject mobilePlatformGameObject = null;
        private MobilePlatform mobilePlatform = null;
		[SerializeField] private bool mustResetOnDeath = true;

		private void Awake()
        {
            _list.Add(this);
            mobilePlatform = mobilePlatformGameObject.GetComponent<MobilePlatform>();
        }

        protected override void TriggerInteraction()
        {
            mobilePlatform.SetModeNormal();
			mobilePlatform.IsStarted = true;
		}

		private void OnDestroy()
        {
            _list.Remove(this);
        }

        public static void ResetAllOnDeath()
        {
			PlatformTrigger trigger = null;
            for (int i = _list.Count - 1; i >= 0; i--)
            {
				trigger = _list[i];

				if (trigger.mustResetOnDeath)
					trigger.mobilePlatform.IsStarted = false;
			}
		}

		public static void ResetAll()
		{
			for (int i = _list.Count - 1; i >= 0; i--)
			{
				_list[i].mobilePlatform.SetModeWait();
				_list[i].mobilePlatform.IsStarted = false;
			}
		}
	}
}