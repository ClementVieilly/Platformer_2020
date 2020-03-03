///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 03/03/2020 01:52
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Cameras {
	public class ChangeCamera : MonoBehaviour
	{
		[SerializeField] protected GameObject vCam;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			vCam.SetActive(true);
		}
		private void OnTriggerExit2D(Collider2D collision)
		{
			vCam.SetActive(false);
		}
	}
}