///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 21/01/2020 12:06
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.Controllers
{
	public class KeyboardController : AController
	{
		public override float HorizontalAxis { get => Input.GetAxis(horizontalAxisName); }
		public override float Jump { get => Input.GetAxis(jumpAxisName); }
	}
}