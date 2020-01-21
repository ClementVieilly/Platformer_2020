///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:37
///-----------------------------------------------------------------

using System.Collections;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Managers
{
	public class TimeManager : MonoBehaviour
	{
		[SerializeField] private float slowDownFactor = .05f;

		public bool waiting = false;

		public void SlowTime()
		{
			Time.timeScale = slowDownFactor;
			Time.fixedDeltaTime = Time.timeScale * .02f;
		}

		public void ResetTime()
		{
			Time.timeScale = 1f;
		}

		public void HitStop(float duration)
		{
			if (waiting) return;

			Time.timeScale = 0;
			StartCoroutine(Wait(duration));
		}

		private IEnumerator Wait(float duration)
		{
			waiting = true;

			yield return new WaitForSecondsRealtime(duration);

			Time.timeScale = 1;
			waiting = false;
		}
	}
}