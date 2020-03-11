///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 11/03/2020 10:44
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects.InteractiveObstacles;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.FTUE {
	public class InteractiveTuto : AInteractiveObstacles
	{
		[SerializeField] private Animator animator;
		protected override void TriggerInteraction()
		{
			animator.SetTrigger("start");
		}
	}
}