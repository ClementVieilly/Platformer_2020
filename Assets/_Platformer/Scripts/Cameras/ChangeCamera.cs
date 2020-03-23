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
		[SerializeField] protected Player player;

		[Space(10)]
		[SerializeField] protected bool isFirstActivation = true;
		[SerializeField] protected float timeLockFirstActivation;

		virtual protected void Start()
		{
			//disable vCam if forget to disable it on the scene
			if (vCam.activeSelf) vCam.SetActive(false);
		}

		virtual public void Launch()
		{
			vCam.SetActive(true);
			if (isFirstActivation)
			{
				StartCoroutine(player.Lock(timeLockFirstActivation));
				isFirstActivation = false;
			}
		}

		virtual protected void OnTriggerEnter2D(Collider2D collision)
		{
			Launch();
		}

		virtual protected void OnTriggerExit2D(Collider2D collision)
		{
			vCam.SetActive(false);
		}
	}
}