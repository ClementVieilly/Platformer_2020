///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 11/03/2020 10:44
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.InteractiveObstacles;
using Com.IsartDigital.Platformer.Managers;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.FTUE
{
	public class InteractiveTuto : AInteractiveObstacles
	{
		[SerializeField] private Animator animator = null;
		[SerializeField ]private float leftLifeTimeAfterUse = 0;
		[SerializeField] InteractiveType animSound;

		protected string soundToPlay = null;

		private void Start()
		{
			switch (animSound)
			{
				case InteractiveType.NONE:
					soundToPlay = null;
					break;
				case InteractiveType.Chouette:
					soundToPlay = sounds.Birds_owl;
					break;
				case InteractiveType.Gyapete:
					soundToPlay = sounds.Birds_gyapede;
					break;
				case InteractiveType.Flamme_Bougie:
					soundToPlay = null;
					break;
				case InteractiveType.Hiboux:
					soundToPlay = null;
					break;
				case InteractiveType.Marmite:
					soundToPlay = sounds.Env_Cooking_Pot;
					break;
				case InteractiveType.Porte:
					soundToPlay = null;
					break;
				case InteractiveType.Table_Cage:
					break;
				default:
					soundToPlay = null;
					break;
			}
		}

		protected override void TriggerInteraction()
		{
			animator.SetBool("isStart",true);
			if (soundToPlay != null) SoundManager.Instance.Play(soundToPlay, this);
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			animator.SetBool("isStart", false);
			if (soundToPlay != null) SoundManager.Instance.Stop(soundToPlay, this);
			//Destroy(gameObject,leftLifeTimeAfterUse);
		}
	}

	public enum InteractiveType
	{
		NONE,
		Chouette,
		Flamme_Bougie,
		Gyapete,
		Hiboux,
		Marmite,
		Porte,
		Table_Cage
	}
}