///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 15/01/2020 14:40
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Controllers;
using Com.IsartDigital.Platformer.Managers;
using UnityEngine;

namespace Com.IsartDigital.Platformer.ScriptableObjects
{
	[CreateAssetMenu(menuName = "Platformer/Player Controller")]
	public class PlayerController : ScriptableObject
	{
		private AController controller = null;

		public float HorizontalAxis { get => controller.HorizontalAxis; }
		public bool Jump { get => controller.Jump; }

		public void Init()
		{
			controller = InputManager.Instance.Controller;
		}
	}
}