///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 27/01/2020 12:24
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.Collectibles {

	public delegate void LifeCollectibleEventHandler(int healValue);

	public class LifeCollectible : ACollectible {

		[SerializeField] private int WinLife = 1;

		private static List<LifeCollectible> _list = new List<LifeCollectible>();
		public static List<LifeCollectible> List => _list;

		public event LifeCollectibleEventHandler OnCollected;

		private void Awake()
		{
			_list.Add(this);
		}

        protected override void EffectOfTheCollectible()
        {
            OnCollected?.Invoke(WinLife);
        }

        private void OnDestroy()
		{
			_list.Remove(this);
			OnCollected = null;
		}

	}
}