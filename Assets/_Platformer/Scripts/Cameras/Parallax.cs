///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 25/02/2020 11:41
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Cameras {
	public class Parallax : MonoBehaviour
	{
		public static List<Parallax> list = new List<Parallax>();

		[SerializeField] private Transform cam;
		[SerializeField] private float parallaxRatio = 0.3f;
		[SerializeField] private bool isLockedY = false;

		private Vector3 lastCamPos;
		private Vector3 movementSinceLastFrame;

		private void Start()
		{
			lastCamPos = cam.position;	
		}

		private void LateUpdate()
		{
			movementSinceLastFrame = cam.position - lastCamPos;
			if (isLockedY) movementSinceLastFrame.y = 0;
			transform.position -= movementSinceLastFrame * parallaxRatio;
			lastCamPos = cam.position;
		}
	}
}