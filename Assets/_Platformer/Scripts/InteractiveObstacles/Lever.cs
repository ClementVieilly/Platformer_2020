///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 22/03/2020 16:52
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Cameras;
using UnityEngine;

namespace Com.IsartDigital.Platformer.InteractiveObstacles
{
	public class Lever : MonoBehaviour
	{
		[SerializeField] float duration = 1f;
		[SerializeField] float amplitude = 1f;

		public void ShakeCamera()
		{
			StartCoroutine(CameraShake.Instance.Shake(duration, amplitude));
		}
	}
}