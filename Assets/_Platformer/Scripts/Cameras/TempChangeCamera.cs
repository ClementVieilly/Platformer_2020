///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 03/03/2020 15:57
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Cameras {
	public class TempChangeCamera : MonoBehaviour
	{
		[SerializeField] private GameObject vCam;
		[SerializeField] private float camDuration = 1;
		[SerializeField] private Player player;
		[SerializeField] private bool doItOnce = false;
		
		private bool triggered = false;
		private float counter = 0;

		private static List<TempChangeCamera> _list = new List<TempChangeCamera>();

		private void Start()
		{
            _list.Add(this);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (doItOnce && triggered) return;
			else if (doItOnce) triggered = true;

			StartCoroutine(ChangeCamera(camDuration));
		}

		private IEnumerator ChangeCamera(float time)
		{
			StartCoroutine(player.Lock(time));

			vCam.SetActive(true);
			while (counter <= time)
			{
				counter += Time.deltaTime;
				yield return null;
			}

			vCam.SetActive(false);
		}

		public static void ResetAll()
		{
			for (int i = _list.Count - 1; i >= 0; i--)
			{
				if (_list[i].doItOnce)
					_list[i].triggered = false;
			}
		}

		private void OnDestroy()
		{
            _list.Remove(this);
		}
	}
}