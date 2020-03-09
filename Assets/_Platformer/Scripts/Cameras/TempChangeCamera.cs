///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 03/03/2020 15:57
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects;
using System.Collections;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Cameras {
	public class TempChangeCamera : MonoBehaviour
	{
		[SerializeField] private GameObject vCam;
		[SerializeField] private float camDuration = 1;
		[SerializeField] private Player player;
		private float counter = 0;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			StartCoroutine(ChangeCamera(camDuration));
		}

		private IEnumerator ChangeCamera (float time)
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
	}
}