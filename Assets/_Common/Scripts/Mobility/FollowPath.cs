///-----------------------------------------------------------------
///   Author : Théo Sabattié                    
///   Date   : 24/09/2019 09:45
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Common.Mobility
{
	public class FollowPath : MonoBehaviour
	{
		[SerializeField, Range(1f, 7f)] private float speed = 1f;
		[SerializeField] private Transform[] waypoints = null;

		private int index;
		private int moveIndex = 1;

		private void Update()
		{
			Vector3 nextPosition = waypoints[index].position;

			transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);

			if (transform.position == nextPosition)
			{
				index += moveIndex;

				if (index == 0 || index == waypoints.Length - 1)
					moveIndex *= -1;
			}
		}
	}
}