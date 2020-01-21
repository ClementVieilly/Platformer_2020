///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 15/01/2020 14:40
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.ScriptableObjects
{
	[CreateAssetMenu(menuName = "Platformer/Player Controller")]
	public class PlayerController : ScriptableObject
	{
		[SerializeField] private string horizontalAxis = "Horizontal";
		[SerializeField] private string jump = "Jump";

		public float HorizontalAxis { get { return Input.GetAxis(horizontalAxis); } }
		public bool Jump { get { return Input.GetButton(jump); } }
	}
}