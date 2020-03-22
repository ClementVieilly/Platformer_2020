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
		public void ShakeCamera(float duration, float magnitude)
		{
			CameraShake.Instance.Shake(duration, magnitude);
		}
	}
}