///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 19/02/2020 10:09
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects {
	public class DeadZone : ACollisionableObject
	{
		private static List<DeadZone> _list = new List<DeadZone>();
		public static List<DeadZone> List => _list;
		public Action OnCollision;

		private void Awake()
		{
			_list.Add(this);
		}

		protected override void EffectOnCollision()
		{
			OnCollision?.Invoke();
		}

		private void OnDestroy()
		{
			_list.Remove(this);
		}

	}
}