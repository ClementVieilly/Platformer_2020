///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 21/01/2020 12:07
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.Controllers
{
	public class TouchController : AController
	{
		private float _horizontalAxis = 0f;
		private float _jump = 0f;

		public override float HorizontalAxis { get => _horizontalAxis; }
		public override float Jump { get => _jump; }
	}
}