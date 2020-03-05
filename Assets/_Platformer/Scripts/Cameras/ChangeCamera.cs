///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 03/03/2020 01:52
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Cameras {
	public class ChangeCamera : MonoBehaviour
	{
		[SerializeField] private GameObject vCam;
		[SerializeField] private float timeLockFirstActivation;
		[SerializeField] private Player player;

		[SerializeField] private bool isFirstActivation = true;

		private void Start()
		{
			//disable vCam if forget to disable it on the scene
			if (vCam.activeSelf) vCam.SetActive(false);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			vCam.SetActive(true);
			if (isFirstActivation)
			{
				StartCoroutine(player.Lock(timeLockFirstActivation));
				isFirstActivation = false;
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			vCam.SetActive(false);
		}
	}
}