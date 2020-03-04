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
		private float counter = 1;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			StartCoroutine(ChangeCamera(camDuration));
		}

		private IEnumerator ChangeCamera (float time)
		{
			Rigidbody2D rigidbody = player.GetComponent<Rigidbody2D>();
			vCam.SetActive(true);

			player.isLocked = true;

			while (counter <= time)
			{
				rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
				counter += Time.deltaTime;
				yield return null;
			}

			player.isLocked = false;

			vCam.SetActive(false);
		}
	}
}