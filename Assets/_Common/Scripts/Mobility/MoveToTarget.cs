///-----------------------------------------------------------------
///   Author : Théo Sabattié                    
///   Date   : 23/09/2019 16:56
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Common.Mobility
{
	public class MoveToTarget : MonoBehaviour
	{
		[SerializeField] private Transform target = null;
		[SerializeField] private float speed = 1f;

		private void Update()
		{
			transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
		}
	}
}