///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 24/01/2020 11:35
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects
{
	public class Checkpoint : ACollisionableObject
	{
		public Action<Checkpoint> OnCollision;

		[SerializeField] private bool _isFinalCheckPoint = false;
		public bool IsFinalCheckPoint => _isFinalCheckPoint;

		[SerializeField] private Animator animator = null;

		protected override void EffectOnCollision()
		{
			OnCollision?.Invoke(this);
			GetComponent<Collider2D>().enabled = false;

			if (animator) animator.SetBool("IsOpen", true);
		}

		public void Reset()
		{
			GetComponent<Collider2D>().enabled = true;

			if (animator) animator.SetBool("IsOpen", false);
		}
	}
}