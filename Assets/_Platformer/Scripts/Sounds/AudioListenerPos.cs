///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 13/03/2020 15:43
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.Sounds {
	public class AudioListenerPos : MonoBehaviour
	{
		[SerializeField] private float distance = -10;
		[SerializeField] private Transform targetToFollow = null;
		private void Start()
	    {
			transform.position = new Vector3(0, 0, distance);
	    }

		private void FixedUpdate()
		{
			if (targetToFollow == null) return;

			transform.position = new Vector3(targetToFollow.position.x, targetToFollow.position.y, distance); 
		}
	}
}