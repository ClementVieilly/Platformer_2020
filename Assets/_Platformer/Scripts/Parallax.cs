///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 25/02/2020 11:41
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer {
	public class Parallax : MonoBehaviour
	{
		public static List<Parallax> list = new List<Parallax>();

		[SerializeField] private float scrollingSpeed;
		[SerializeField] private float parallaxScaleX;
		[SerializeField] private float parallaxScaleY;
		[SerializeField ]private bool isParallaxOnY;

		private Transform cam;
		private Vector3 previousCamPos;


		private Vector3 startPos;

		private void Awake()
		{
			cam = Camera.main.transform;
			list.Add(this);
		}
		private void Start()
		{
			//patch replace background
			cam.position = Vector3.zero;

			previousCamPos = cam.position;
			startPos = transform.position;
		}

		private void Update()
		{
			Vector3 nextPosition;
			float xParallax = (previousCamPos.x - cam.position.x) * parallaxScaleX;
			float posX = transform.position.x + xParallax;

			if (isParallaxOnY)
			{
				float yParallax = (previousCamPos.y - cam.position.y) * - parallaxScaleY;
				float posY = transform.position.y + yParallax;
				nextPosition = new Vector3(posX, posY, transform.position.z);
			}
			else nextPosition = new Vector3(posX, transform.position.y, transform.position.z);

			transform.position = Vector3.Lerp(transform.position, nextPosition, scrollingSpeed * Time.deltaTime);
			previousCamPos = cam.position;
		}

		public static void ResetAll()
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				list[i].transform.position = list[i].startPos;
			}
		}

		private void OnDestroy()
		{
			list.Remove(this);
		}
	}
}