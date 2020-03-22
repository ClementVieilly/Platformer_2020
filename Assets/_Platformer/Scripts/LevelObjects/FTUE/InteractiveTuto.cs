///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 11/03/2020 10:44
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.InteractiveObstacles;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.FTUE
{
	public class InteractiveTuto : AInteractiveObstacles
	{
		[SerializeField] private Animator animator = null;
		[SerializeField ]private float leftLifeTimeAfterUse = 0;

		protected override void TriggerInteraction()
		{
			animator.SetBool("isStart",true);
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			animator.SetBool("isStart", false);
			//Destroy(gameObject,leftLifeTimeAfterUse);
		}
	}
}