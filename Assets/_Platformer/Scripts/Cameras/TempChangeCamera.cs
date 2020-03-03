///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 03/03/2020 15:57
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.Cameras {
	public class TempChangeCamera : MonoBehaviour
	{
		[SerializeField] private GameObject vCam;
		[SerializeField] private float camDuration = 1;
		private float counter = 1;
		private bool isStarted = false;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			Debug.Log("is on");
			isStarted = true;
			vCam.SetActive(true);
		}


		private void Update()
	    {
			if (!isStarted) return;
			counter += Time.deltaTime;
			if (counter >= camDuration)
			{
				vCam.SetActive(false);
				//Destroy(gameObject);
				isStarted = false;
				return;
			}
	    }


	}
}