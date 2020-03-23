///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 23/03/2020 15:58
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects;
using UnityEngine;

namespace Com.IsartDigital.Platformer
{
	public class Follow : MonoBehaviour
	{
		[SerializeField] private Transform playerTransform = null;
		[SerializeField] private Player player = null;
		private float playerDirection = 0f;

		[SerializeField] private Vector3 rightForward = Vector3.zero;
		[SerializeField] private Vector3 leftForward = Vector3.zero;

		private void Update()
		{
			playerDirection = player.Direction;
			transform.position = playerTransform.position;

			if (playerDirection == 1f)
				transform.position += rightForward;
			else if (playerDirection == -1f)
				transform.position += leftForward;
		}
	}
}