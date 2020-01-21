///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 21/01/2020 12:05
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.Controllers
{
	public abstract class AController
	{
		[SerializeField] protected string horizontalAxisName = "Horizontal";
		[SerializeField] protected string jumpAxisName = "Jump";

		public abstract float HorizontalAxis { get; }
		public abstract float Jump { get; }
	}
}