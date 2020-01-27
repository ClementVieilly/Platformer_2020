///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 21/01/2020 12:05
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Controllers
{
	public abstract class AController
	{
		[SerializeField] protected string horizontalAxisName = "Horizontal";
		[SerializeField] protected string jumpAxisName = "Jump";

		public abstract float HorizontalAxis { get; }
		public abstract bool Jump { get; }

		/// <summary>
		/// Initialize controller
		/// </summary>
		public virtual void Init()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Update controller inputs
		/// </summary>
		public virtual void Update()
		{
			throw new NotImplementedException();
		}
	}
}