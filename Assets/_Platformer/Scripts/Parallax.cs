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

		[SerializeField] private Transform cam;
		[SerializeField, Range(0,2)] private float relativeSpeed = 0.3f;
		[SerializeField] private bool isLockedY = false;

		private void LateUpdate()
		{
			transform.position =Vector2.Lerp( transform.position,isLockedY ? new Vector2(cam.position.x * - relativeSpeed, transform.position.y) :
										 new Vector2(cam.position.x * - relativeSpeed, cam.position.y * - relativeSpeed), 5 * Time.fixedDeltaTime);	
		}
	}
}