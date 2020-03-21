///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/03/2020 15:44
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer
{
	public class Eye : MonoBehaviour
	{
		[SerializeField] private Transform pupil = null;

		[SerializeField] private float speed = 1f;
		[SerializeField] private float distanceToCenter = 1f;

		private Transform target = null;

		private Vector3 startPosition = Vector3.zero;
		private Vector3 endPosition = Vector3.zero;

		private void Start()
		{
			startPosition = pupil.position;
		}

		private void Update()
		{
			if (target)
				endPosition = startPosition + Vector3.Normalize(target.position - startPosition) * distanceToCenter;
			else
				endPosition = startPosition;

			pupil.position = Vector3.MoveTowards(pupil.position, endPosition, speed * Time.deltaTime);
		}

		public void SetTarget(Transform target)
		{
			this.target = target;
		}

		public void ClearTarget(Transform target)
		{
			if (target == this.target)
				this.target = null;
		}
	}
}