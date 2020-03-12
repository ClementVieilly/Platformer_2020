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
		[SerializeField] private float parallaxRatioX = 0.3f;
		[SerializeField] private float parallaxRatioY = 0.3f;
		[SerializeField] private bool isLockedY = false;
		private bool firstUpdate = true;

		private Vector3 lastCamPos;
		private Vector3 movementSinceLastFrame;

		private Action DoAction;

		private void Start()
		{
			lastCamPos = cam.position;

			if (GetComponent<Collider2D>() != null) SetModeVoid();
			else SetModeNormal();
		}

		private void LateUpdate()
		{
			DoAction();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			SetModeNormal();
		}
		private void OnTriggerExit2D(Collider2D collision)
		{
			SetModeVoid();
		}

		private void SetModeVoid()
		{
			Debug.Log("is void");
			DoAction = DoActionVoid;
		}

		private void SetModeNormal()
		{
			DoAction = DoActionNormal;
		}

		private void DoActionVoid()	{}

		private void DoActionNormal()
		{
			UpdatePos();
			if (transform.position != Vector3.zero && firstUpdate)
			{
				SetStartPos();
			}
		}

		private void UpdatePos()
		{
			movementSinceLastFrame = cam.position - lastCamPos;
			if (isLockedY) movementSinceLastFrame.y = 0;
			transform.position -= new Vector3(movementSinceLastFrame.x * parallaxRatioX, movementSinceLastFrame.y * parallaxRatioY);
			lastCamPos = cam.position;
		}



		private void SetStartPos()
		{
			transform.position = Vector3.zero;
			firstUpdate = false;
		}

	}
}