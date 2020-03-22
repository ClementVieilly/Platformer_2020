///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 22/03/2020 16:48
///-----------------------------------------------------------------

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Cameras
{
	public class CameraShake : MonoBehaviour
	{
		private static CameraShake instance;
		public static CameraShake Instance { get => instance; }

		[SerializeField] private List<CinemachineVirtualCamera> vCams = null;

		private void Awake()
		{
			if (instance)
			{
				Destroy(gameObject);
				return;
			}

			instance = this;
		}

		public IEnumerator Shake(float duration, float amplitude)
		{
			Debug.Log("Start " + vCams.Count);

			Parallax.isShaking = true;

			foreach (CinemachineVirtualCamera vCam in vCams)
				vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;

			float elapsed = 0f;

			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				yield return null;
			}

			foreach (CinemachineVirtualCamera vCam in vCams)
				vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;

			Parallax.isShaking = false;
			Debug.Log("Done");
		}

		private void OnDestroy()
		{
			if (this == instance) instance = null;
		}
	}
}