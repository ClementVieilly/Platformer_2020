///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 25/02/2020 11:41
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer {
	public class Parallax : MonoBehaviour
	{
		private Transform cam;

		private Vector3 previousCamPos;
		private bool isParallaxOnY;

		[SerializeField] private float scrollingSpeed;
		[SerializeField] private float parallaxScale;

		private void Awake()
		{
			cam = Camera.main.transform;
		}
		private void Start()
		{
			previousCamPos = cam.position;
		}

		private void LateUpdate()
		{
			float parallax = (previousCamPos.x - cam.position.x) * parallaxScale;
			float posX = transform.position.x + parallax;
			Vector3 nextPosition = new Vector3(posX,transform.position.y,transform.position.z);

			transform.position = Vector3.Lerp(transform.position, nextPosition, scrollingSpeed * Time.deltaTime);
			previousCamPos = cam.position;
		}

	}
}